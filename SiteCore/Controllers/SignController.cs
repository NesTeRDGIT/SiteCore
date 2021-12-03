using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Hubs;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    
    [Authorize(Roles = "SignAdmin, SignMO")]
    public class SignController : Controller
    {
        private MyOracleSet myOracleSet { get; }
        private IX509CertificateManager x509CertificateManager;
        private WCFCryptoConnect wcfCryptoConnect;
        private IZipArchiver ZipArchiver;
        private IHubContext<NotificationHub> notificationHub;
      
        private UserInfoHelper userInfoHelper;
        private UserInfo _userInfo;
        private UserInfo userInfo => _userInfo ?? (_userInfo = userInfoHelper.GetInfo(User.Identity.Name));

        public SignController(MyOracleSet myOracleSet, IX509CertificateManager x509CertificateManager, WCFCryptoConnect wcfCryptoConnect, IZipArchiver ZipArchiver, IHubContext<NotificationHub> notificationHub, UserInfoHelper userInfoHelper)
        {
            this.myOracleSet = myOracleSet;
            this.x509CertificateManager = x509CertificateManager;
            this.wcfCryptoConnect = wcfCryptoConnect;
            this.ZipArchiver = ZipArchiver;
            this.notificationHub = notificationHub;
            this.userInfoHelper = userInfoHelper;


        }
        public IActionResult SignSPR()
        {
            return View();
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> AddRole(AddRoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await myOracleSet.SIGN_ROLE.CountAsync(x => x.CAPTION == model.CAPTION.ToUpper() || x.PREFIX == model.PREFIX.ToUpper()) != 0)
                        throw new Exception($"Роль '{model.CAPTION.ToUpper()}' или префикс '{model.PREFIX.ToUpper()}' уже присутствует в справочнике");
                    myOracleSet.SIGN_ROLE.Add(new SIGN_ROLE { CAPTION = model.CAPTION.ToUpper(), PREFIX = model.PREFIX.ToUpper()});
                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception e)
            {
                return  CustomJsonResult.Create(e.Message, false);
            }
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetRole()
        {
            try
            {
                var result =  await myOracleSet.SIGN_ROLE.OrderBy(x=>x.SIGN_ROLE_ID).Select(x=>new {x.SIGN_ROLE_ID, x.CAPTION, x.PREFIX}).ToListAsync();
                return CustomJsonResult.Create(result);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> RemoveRole([FromBody] int SIGN_ROLE_ID)
        {
            try
            {
                var item = await myOracleSet.SIGN_ROLE.FirstOrDefaultAsync(x => x.SIGN_ROLE_ID == SIGN_ROLE_ID);
                if (item == null)
                    throw new Exception($"Элемент с SIGN_ROLE_ID={SIGN_ROLE_ID} не найден");
                myOracleSet.Remove(item);
                await myOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetSignList()
        {
            try
            {
                var result = await myOracleSet.SIGN_LIST
                    .Include(x => x.ROLE)
                    .Include(x => x.MO_NAME)
                    .OrderBy(x => x.CODE_MO).ThenByDescending(x => x.DATE_E ?? DateTime.Now)
                    .Select(x => new SING_LIST { CODE_MO = x.CODE_MO, DATE_B = x.DATE_B, DATE_E = x.DATE_E, MO_NAME = x.MO_NAME.NAM_MOK, ROLE = x.ROLE.CAPTION, PublicKey = x.PUBLICKEY_ISSUER, ID = x.ID, PublicKey_ISSUER =x.PUBLICKEY_ISSUER})
                    .ToListAsync();
           
                return CustomJsonResult.Create(result);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> GetCertificateINFO(IFormFile file, bool isIssuer = false)
        {
            try
            {
                if (file != null)
                {
                    byte[] data;
                    await using (var st = file.OpenReadStream())
                    {
                        data = st.ReadFull();
                    }
                    var info = await ValidateCertAsync(data, isIssuer);
                    return CustomJsonResult.Create(info);
                }

                return CustomJsonResult.Create(true);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message,false);
            }
         
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetF003()
        {
            return CustomJsonResult.Create(await myOracleSet.CODE_MO.OrderBy(x => x.MCOD).Select(x => new { x.MCOD, NAME = $"{x.NAM_MOK}({x.MCOD})" }).ToListAsync());
        }


        private async Task<CertificateInfo> ValidateCertAsync(byte[] data,bool isIssuer)
        {
            var info = x509CertificateManager.GetInfo(data);
            var cert = info.Data.FirstOrDefault();
            var issuer = info.Data.LastOrDefault();
            if (cert == null)
                throw new Exception($"Не удалось найти сертификат в файле данных");
            if (issuer == null)
                throw new Exception($"Не удалось найти сертификат издателя в файле данных");
            if (!isIssuer)
            {
                if (!await x509CertificateManager.CheckIssuerAsync(issuer))
                {
                    issuer.Error.Add("Издатель отсутствует в списке доверенных ТФОМС Забайкальского края");
                }
            }
            else
            {
                if(cert!= issuer)
                    foreach (var d in info.Data)
                    {
                        d.Error.Add("Сертификат издателя должен быть единственным в цепочке");
                    }
            }
            return info;
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> AddCert(AddCertModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await myOracleSet.CODE_MO.FirstOrDefaultAsync(x => x.MCOD == model.CODE_MO) == null)
                        throw new Exception($"Медицинская организация с кодом \"{model.CODE_MO}\" не найдена в справочнике");
                    if (await myOracleSet.SIGN_ROLE.FirstOrDefaultAsync(x => x.SIGN_ROLE_ID == model.ROLE_ID) == null)
                        throw new Exception($"Роль с кодом \"{ model.ROLE_ID}\" не найдена в справочнике");
                    if (System.IO.Path.GetExtension(model.File.FileName).ToUpper() != ".CER")
                        throw new Exception("Файл сертификата должен быть формата .CER");
                    if (model.File.Length.ToMegaBytes() > 2)
                        throw new Exception("Размер фала сертификата не может превышать 2Мб");
                    if (model.File.Length.ToMegaBytes() > 20)
                        throw new Exception("Размер фала подтверждения не может превышать 20Мб");



                    byte[] certFile;
                    await using (var st = model.File.OpenReadStream())
                    {
                        certFile = st.ReadFull();
                    }
                    byte[] fileConfirm;
                    await using (var st = model.FileConfirm.OpenReadStream())
                    {
                        fileConfirm = st.ReadFull();
                    }

                    

                    var info = await ValidateCertAsync(certFile, false);

                    if (info.Valid)
                    {
                        myOracleSet.SIGN_LIST.Add(new SIGN_LIST()
                        {
                            CODE_MO = model.CODE_MO,
                            DATE_B = model.DATE_B,
                            DATE_E = model.DATE_E,
                            FILE_CERT = certFile,
                            FILE_CONFIRM = fileConfirm,
                            FILE_CONFIRM_EXT = System.IO.Path.GetExtension(model.FileConfirm.FileName).ToUpper(),
                            SIGN_ROLE_ID = model.ROLE_ID,
                            PUBLICKEY = info.Data.First().PublicKey,
                            PUBLICKEY_ISSUER = info.Data.Last().PublicKey
                        });
                        await myOracleSet.SaveChangesAsync();
                        return CustomJsonResult.Create(true);
                    }
                    return CustomJsonResult.Create(info.Errors, false);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception ex)
            {
               return CustomJsonResult.Create(new List<string>{ex.Message}, false);
            }
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> DownloadCert(int id)
        {
            try
            {
                var sign = await myOracleSet.SIGN_LIST.Include(x=>x.ROLE).FirstOrDefaultAsync(x => x.ID == id);
                if (sign != null)
                {
                    var fileName = $"Сертификат [{sign.CODE_MO}] [{sign.ROLE.CAPTION}] c {sign.DATE_B:dd.MM.yyyy}{(sign.DATE_E.HasValue ? $" по {sign.DATE_E:dd.MM.yyyy}" : "")}.zip".ToValidFileName();
                    var data = ZipArchiver.Zip(new ZipArchiverEntry("Сертификат.crt", sign.FILE_CERT), new ZipArchiverEntry($"Подтверждение сертификата{sign.FILE_CONFIRM_EXT}", sign.FILE_CONFIRM));
                    var file = File(data, System.Net.Mime.MediaTypeNames.Application.Zip, fileName);
                    return CustomJsonResult.Create(file);
                }
                return CustomJsonResult.Create($"Подпись ID={id} не найден на сервере", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> RemoveSign([FromBody]int ID)
        {
            try
            {
                var item = await myOracleSet.SIGN_LIST.FirstOrDefaultAsync(x => x.ID == ID);
                if (item != null)
                {
                    myOracleSet.SIGN_LIST.Remove(item);
                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create($"Не удалось найти подпись с номером {ID}", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetSignCertInfo(int ID)
        {
            try
            {
                var item = await myOracleSet.SIGN_LIST.FirstOrDefaultAsync(x => x.ID == ID);
                if (item != null)
                {
                    var info = await ValidateCertAsync(item.FILE_CERT, false);
                    return CustomJsonResult.Create(info);
                }
                return CustomJsonResult.Create($"Не удалось найти подпись с номером {ID}", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetISSUERCertInfo(int ID)
        {
            try
            {
                var item = await myOracleSet.SING_ISSUER.FirstOrDefaultAsync(x => x.SING_ISSUER_ID == ID);
                if (item != null)
                {
                    var info = await ValidateCertAsync(item.FILE_CERT, true);
                    return CustomJsonResult.Create(info);
                }
                return CustomJsonResult.Create($"Не удалось найти подпись издателя с номером {ID}", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }


        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetISSUER()
        {
            try
            {
                var result = await myOracleSet.SING_ISSUER
                    .Select(x => new ISSUER_LIST { CAPTION = x.CAPTION, DATE_B = x.DATE_B, DATE_E = x.DATE_E,SING_ISSUER_ID = x.SING_ISSUER_ID})
                    .ToListAsync();
                return CustomJsonResult.Create(result);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }
        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> AddISSUER(AddISSUERModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    byte[] data;
                    await using (var st = model.File.OpenReadStream())
                    {
                        data = st.ReadFull();
                    }

                    var info = await ValidateCertAsync(data, true);
                    if (info.Valid)
                    {
                        var issuer = info.Data.LastOrDefault();
                        

                        myOracleSet.SING_ISSUER.Add(new SIGN_ISSUER()
                        {
                         
                            DATE_B = model.DATE_B,
                            DATE_E = model.DATE_E,
                            CAPTION = model.CAPTION,
                            FILE_CERT =data,
                            PUBLICKEY = issuer.PublicKey
                        });
                        await myOracleSet.SaveChangesAsync();
                        return CustomJsonResult.Create(true);
                    }
                    return CustomJsonResult.Create(info.Errors, false);
                }
              
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> RemoveISSUER([FromBody] int ID)
        {
            try
            {
                var item = await myOracleSet.SING_ISSUER.FirstOrDefaultAsync(x => x.SING_ISSUER_ID == ID);
                if (item != null)
                {
                    myOracleSet.SING_ISSUER.Remove(item);
                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create($"Не удалось найти подпись с номером {ID}", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }

   
        [HttpGet]
        public async Task<CustomJsonResult> GetDoc(int THEME_ID)
        {
            try
            {
                var items = await myOracleSet.DOC_FOR_SIGN
                    .Include(x => x.CODE_MO_NAME)
                    .Include(x => x.SIGNs).ThenInclude(x => x.ROLE)
                    .Where(x=> (isSignAdmin || x.CODE_MO == userInfo.CODE_MO) && x.THEME_ID == THEME_ID)
                    .Select(x => new DOCViewModel()
                    {
                        FILENAME = x.FILENAME,
                        MO_NAME = $"{x.CODE_MO_NAME.NAM_MOK}({x.CODE_MO})",
                        DateCreate = x.DATE_CREATE,
                        DOC_FOR_SIGN_ID = x.DOC_FOR_SIGN_ID,
                        SIGNS = x.SIGNs.Select(s => new DOCSignViewModel()
                        {
                            ROLE_ID = s.SIGN_ROLE_ID,
                            ROLE_NAME = s.ROLE.CAPTION,
                            IsSIGN = s.SIGN_LIST_ID.HasValue
                        }).ToList()
                    }).ToListAsync();
                return CustomJsonResult.Create(items);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }
        [HttpGet]
        public async Task<CustomJsonResult> GetTheme()
        {
            try
            {
                List<DOC_THEME> items;
                if(User.IsInRole("SignAdmin"))
                {
                     items = await myOracleSet.DOC_THEME.OrderByDescending(x=>x.THEME_ID).ToListAsync();
                }
                else
                {
                    items = await myOracleSet.DOC_FOR_SIGN.Include(x => x.THEME).Where(x => x.CODE_MO == userInfo.CODE_MO).Select(x => x.THEME).Distinct().ToListAsync();
                }
            
                return CustomJsonResult.Create(items);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> AddTheme(string THEME)
        {
            try
            {
                if (string.IsNullOrEmpty(THEME))
                    throw new Exception("Не возможно добавить пустую тему");
                myOracleSet.DOC_THEME.Add(new DOC_THEME() { CAPTION = THEME });
                await myOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> RemoveTheme(int THEME_ID)
        {
            try
            {
                var item = await myOracleSet.DOC_THEME.FirstOrDefaultAsync(x => x.THEME_ID == THEME_ID);
                if (item != null)
                {
                    myOracleSet.DOC_THEME.Remove(item);
                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }

                throw new Exception($"Не удалось найти тему с THEME_ID={THEME_ID}");
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> AddFileForSign(AddFilesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var themeItem = await myOracleSet.DOC_THEME.FirstOrDefaultAsync(x => x.THEME_ID == model.THEME_ID);
                    if (themeItem == null)
                        throw new Exception($"Тема с кодом {model.THEME_ID} не найдена на сервере");

                    byte[] data;
                    await using (var st = model.FILE.OpenReadStream())
                    {
                        data = st.ReadFull();
                    }

                    myOracleSet.DOC_FOR_SIGN.Add(new DOC_FOR_SIGN()
                    {
                        DATE_CREATE = DateTime.Now.Date,
                        CODE_MO = model.CODE_MO,
                        DATA = data,
                        FILENAME = model.FILE.FileName,
                        THEME = themeItem,
                        SIGNs = new List<DOC_SIGN>(model.ROLE_ID.Select(x => new DOC_SIGN() { SIGN_ROLE_ID = x }))
                    });

                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);

            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(new List<string> { ex.Message }, false);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> DownloadFileForSign(int DOC_FOR_SIGN_ID)
        {
            try
            {
                var doc =await myOracleSet.DOC_FOR_SIGN.FirstOrDefaultAsync(x => x.DOC_FOR_SIGN_ID == DOC_FOR_SIGN_ID && (isSignAdmin || x.CODE_MO == userInfo.CODE_MO));
                if (doc != null)
                {
                    var file = File(doc.DATA,System.Net.Mime.MediaTypeNames.Application.Octet, doc.FILENAME);
                    return CustomJsonResult.Create(file);
                }
                return CustomJsonResult.Create("Файл не найден на сервере", false);

            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> DownloadFileForSignAndSign(int DOC_FOR_SIGN_ID)
        {
            try
            {
                var doc = await myOracleSet.DOC_FOR_SIGN.Include(x=>x.SIGNs).ThenInclude(x=>x.ROLE).FirstOrDefaultAsync(x => x.DOC_FOR_SIGN_ID == DOC_FOR_SIGN_ID && (isSignAdmin || x.CODE_MO == userInfo.CODE_MO));
                if (doc != null)
                {
                    var files = new List<ZipArchiverEntry>();
                    files.Add(new ZipArchiverEntry(doc.FILENAME, doc.DATA));
                    files.AddRange(doc.SIGNs.Where(x => x.DATE_SIGN.HasValue).Select(docSign => new ZipArchiverEntry($"{doc.FILENAME}_{docSign.ROLE.PREFIX}.sig", docSign.SIGN)));
                    var data = ZipArchiver.Zip(files.ToArray());
                    var file = File(data, System.Net.Mime.MediaTypeNames.Application.Zip, $"{System.IO.Path.GetFileNameWithoutExtension(doc.FILENAME)}.zip");
                    return CustomJsonResult.Create(file);
                }
                return CustomJsonResult.Create("Файл не найден на сервере", false);

            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }

        [Authorize(Roles = "SignAdmin")]
        [HttpGet]
        public async Task<IActionResult> DownloadAllFileTheme(int THEME_ID, string ConnectionId)
        {
            try
            {

                notificationHub.Progress(ConnectionId, 0, 0, "Запрос файлов из БД");
                var docTheme = await myOracleSet.DOC_THEME.Include(x => x.DOCs).ThenInclude(x => x.SIGNs).ThenInclude(x => x.ROLE).Where(x => x.THEME_ID == THEME_ID).FirstOrDefaultAsync();
                if (docTheme == null)
                    throw new Exception($"Не удалось найти THEME_ID={THEME_ID}");
                var entry = new List<ZipArchiverEntry>();
                var filenameDictionary = new Dictionary<string, string>();
                var i = 0;
                foreach (var doc in docTheme.DOCs)
                {
                    i++;
                    notificationHub.Progress(ConnectionId, i, docTheme.DOCs.Count, $"Упаковка файла в архив: {doc.FILENAME}");
                    var docEntry = new List<ZipArchiverEntry>();
                    docEntry.Add(new ZipArchiverEntry(doc.FILENAME, doc.DATA));
                    docEntry.AddRange(doc.SIGNs.Where(x => x.DATE_SIGN.HasValue).Select(docSign => new ZipArchiverEntry($"{doc.FILENAME}_{docSign.ROLE.PREFIX}.sig", docSign.SIGN)));
                    var basefilename = $"{System.IO.Path.GetFileNameWithoutExtension(doc.FILENAME)}";
                    var filename = basefilename;
                    var suf = 0;
                    while (filenameDictionary.ContainsKey(filename))
                    {
                        suf++;
                        filename = $"{basefilename}({suf})";
                    }
                    filenameDictionary.Add(filename, filename);
                    filename = $"{filename}.zip";
                    entry.Add(new ZipArchiverEntry(filename, ZipArchiver.Zip(docEntry.ToArray())));

                }
                notificationHub.Progress(ConnectionId, i, docTheme.DOCs.Count, $"Формирование файла");
                var file = File(ZipArchiver.Zip(entry.ToArray()), System.Net.Mime.MediaTypeNames.Application.Zip, $"{docTheme.CAPTION}.zip");
                notificationHub.Progress(ConnectionId, i, docTheme.DOCs.Count, $"Передача файла");
                return file;

            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }


        [Authorize(Roles = "SignAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> RemoveFileForSign([FromBody]int DOC_FOR_SIGN_ID)
        {
            try
            {
                var doc = await myOracleSet.DOC_FOR_SIGN.Include(x=>x.SIGNs).FirstOrDefaultAsync(x => x.DOC_FOR_SIGN_ID == DOC_FOR_SIGN_ID);
                if (doc != null)
                {
                    myOracleSet.DOC_SIGN.RemoveRange(doc.SIGNs);
                    myOracleSet.DOC_FOR_SIGN.Remove(doc);
                    await myOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create("Файл не найден на сервере", false);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }

        private async Task SignValidateAndSave(int docForSignId, string sign)
        {
            var doc = await myOracleSet.DOC_FOR_SIGN.Include(x => x.SIGNs).FirstOrDefaultAsync(x => x.DOC_FOR_SIGN_ID == docForSignId);
            if (doc == null)
                throw new Exception("Файл не найден на сервере");
            var signature = await wcfCryptoConnect.CheckSignatureBase64Async(doc.DATA, sign);
            if (signature.IsValidate)
            {
                if (!signature.DateSign.HasValue)
                    throw new Exception("Не найдена дата подписи");
                var signListItem = await myOracleSet.SIGN_LIST.FirstOrDefaultAsync(x => (x.CODE_MO == doc.CODE_MO || x.CODE_MO == "75") && x.PUBLICKEY == signature.PublicKey && signature.DateSign >= x.DATE_B && signature.DateSign <= (x.DATE_E ?? DateTime.Now));
                if (signListItem == null)
                    throw new Exception("Не найден владелец подписи");
                var docSign = doc.SIGNs.FirstOrDefault(x => x.SIGN_ROLE_ID == signListItem.SIGN_ROLE_ID);
                if (docSign == null)
                    throw new Exception("Данная подпись не ожидается для данного документа");
                if (docSign.DATE_SIGN.HasValue)
                    throw new Exception("Документ уже подписан данной подписью");
                docSign.DATE_SIGN = signature.DateSign;
                docSign.SIGN_LIST_ID = signListItem.ID;
                docSign.SIGN = Convert.FromBase64String(sign);
                await myOracleSet.SaveChangesAsync();
                return;
            }
            throw new Exception(string.Join(",", signature.ErrorList));
        }

        [HttpPost]
        public async Task<CustomJsonResult> SendSign(AddSignModel model)
        {
            try
            {
                await SignValidateAndSave(model.DOC_FOR_SIGN_ID, model.SIGN);
                return CustomJsonResult.Create(true);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }


        private string FindSignInString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Replace("-----BEGIN CMS-----\r\n", "").Replace("\r\n-----END CMS-----\r\n", "");
        }
        [HttpPost]
        public async Task<CustomJsonResult> UploadFileSig(AddSigFileModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await using var st = model.File.OpenReadStream();
                    var data = st.ReadFull();
                    var fileStringData = Encoding.UTF8.GetString(data);
                    var sign = FindSignInString(fileStringData);
                    await SignValidateAndSave(model.DOC_FOR_SIGN_ID, sign);
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(string.Join(Environment.NewLine, ModelState.GetErrors()), false);
           
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }


     


        #region Private
      
        private bool? _isSignAdmin;
        private bool isSignAdmin
        {
            get
            {
                if (_isSignAdmin.HasValue)
                    return _isSignAdmin.Value;
                _isSignAdmin = User.IsInRole("SignAdmin");
                return _isSignAdmin.Value;
            }
        }
        #endregion

    }


}


public static class Extentions
{
    /// <summary>
    /// Перевести байты в мегабайты
    /// </summary>
    /// <param name="value">Кол-во байт</param>
    /// <returns></returns>
    public static long ToMegaBytes(this long value)
    {
        return value / 1024 / 1024;
    }


    public static string ToValidFileName(this string value, char replace = '$')
    {
        var invalidChar = System.IO.Path.GetInvalidFileNameChars();
        if (invalidChar.Contains(replace))
            replace = '$';
        var replaceList = value.Where(ch => invalidChar.Contains(ch)).ToList();
        replaceList.ForEach(x=>value = value.Replace(x,replace));
        return value;
    }
}
