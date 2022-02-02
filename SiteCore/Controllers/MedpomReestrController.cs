using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Models;
using System.Text;
using Castle.Components.DictionaryAdapter;
using FileItem = ServiceLoaderMedpomData.FileItem;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "MO, Admin")]
    public class MedpomReestrController : Controller
    {
        private WCFConnect WcfConnect { get; }
        private UserManager<ApplicationUser> userManager { get; }
        private MyOracleSet MyOracleSet { get; }
        private  ILogger logger { get; }
        private IMedpomRepository medpomFileManager;
        private IZipArchiver zipArchiver;
        private IHasher hasher;

        private UserInfoHelper userInfoHelper;
        private UserInfo _userInfo;
        private UserInfo userInfo => _userInfo ?? (_userInfo = userInfoHelper.GetInfo(User.Identity.Name));


        public MedpomReestrController(WCFConnect WcfConnect, UserManager<ApplicationUser> userManager, MyOracleSet myOracleSet, ILogger logger, IZipArchiver zipArchiver, IMedpomRepository medpomFileManager, IHasher hasher, UserInfoHelper userInfoHelper)
        {
            this.WcfConnect = WcfConnect;
            this.userManager = userManager;
            this.MyOracleSet = myOracleSet;
            this.logger = logger;
            this.zipArchiver = zipArchiver;
            this.medpomFileManager = medpomFileManager;
            this.hasher = hasher;
            this.userInfoHelper = userInfoHelper;
        }

        public IActionResult ReestrMed()
        {
            return View();
        }

        private CustomJsonResult InternalError(Exception e,bool toList=false)
        {
            object err;
            if (e is ModelException)
            {
                err = toList ? new List<string> { e.Message } : e.Message;
            }
            else
            {
                err = toList ? new List<string> { "Внутренняя ошибка сервиса!" } : "Внутренняя ошибка сервиса!";
                logger?.AddLogExtension(e);
            }
            return CustomJsonResult.Create(err, false);
        }
        private CustomJsonResult UserError(Exception e)
        {
            return CustomJsonResult.Create(e.Message, false);
        }

        #region Просмотр реестров
        [HttpGet]
        public async Task<CustomJsonResult>  GetViewReestrModel()
        {
            try
            {
                var VRVM = new ViewReestrModel { ConnectWCFon = await WcfConnect.PingAsync() };
                if (!string.IsNullOrEmpty(userInfo.CODE_MO) && VRVM.ConnectWCFon)
                {
                    var pac = WcfConnect.GetPackForMO(userInfo.CODE_MO);
                    VRVM.FP = pac.FP==null? null: new FilePacketNew
                    {
                        CaptionMO = pac.FP.CaptionMO,
                        CodeMO = pac.FP.CodeMO,
                        CommentSite = pac.FP.CommentSite,
                        Date = pac.FP.Date,
                        IST = pac.FP.IST,
                        Order = pac.ORDER,
                        Status = pac.FP.Status,
                        WARNNING = pac.FP.WARNNING,
                        isResult = !string.IsNullOrEmpty(pac.FP.PATH_ZIP) && (pac.FP.Status == StatusFilePack.FLKOK || pac.FP.Status == StatusFilePack.FLKERR),
                        FileList = pac.FP.Files.Select(x=>new FileView()
                        {
                            Comment = x.Comment,
                            FileName = x.FileName,
                            Process = x.Process,
                            Type = x.Type,
                            FILE_L = x.filel==null ? null: new FileViewBase()
                            {
                                Type = x.filel.Type,
                                Comment = x.filel.Comment,
                                FileName = x.filel.FileName,
                                Process = x.filel.Process
                            }
                        }).ToList()
                    };
                  
                }
                return CustomJsonResult.Create(VRVM);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
           
        }
        [HttpGet]
        public async Task<CustomJsonResult> DownloadProtocol()
        {
            try
            {
                var pack = await MyPACK();
                var pserv = WcfConnect.GetPackForMO(userInfo.CODE_MO);
                var file = File(WcfConnect.GetProtocol(userInfo.CODE_MO), System.Net.Mime.MediaTypeNames.Application.Zip, Path.GetFileName(pserv.FP.PATH_ZIP));
                pack.DOWNPROT_LAST = DateTime.Now;
                await MyOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(file);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        #endregion
        #region Загрузка реестров
        private async Task<FILEPACK> MyPACK()
        {
            try
            {
                var p = await MyOracleSet.FILEPACK.Include(x => x.FILES).ThenInclude(x=>x.FILE_L).FirstOrDefaultAsync(x => x.CODE_MO == userInfo.CODE_MO && x.STATUS == STATUS_FILEPACK.CURRENT);
                //Если нет до вставляем
                if (p != null) return p;
                p = new FILEPACK { CODE_MO = userInfo.CODE_MO, STATUS = STATUS_FILEPACK.CURRENT };
                MyOracleSet.FILEPACK.Add(p);
                await MyOracleSet.SaveChangesAsync();
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка в MyPACK:{ex.Message}", ex);
            }
        }

        public async Task<CustomJsonResult> GetLoadReestrModel()
        {
            try
            {
                return CustomJsonResult.Create(await getLoadReestViewModel());
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        /// <summary>
        /// Новая
        /// </summary>
        /// <param name="Error"></param>
        /// <returns></returns>
        async Task<LoadReestViewModel> getLoadReestViewModel()
        {
            try
            {
                var pack = await GetFile();
                var files = pack.FILES.Where(x => x.PARENT == null).Select(x=> new Models.FileItem
                {
                    ID = x.ID,
                    STATUS = x.STATUS,
                    COMENT = x.COMENT,
                    FILENAME = x.FILENAME,
                    TYPE_FILE = x.TYPE_FILE,
                    FILE_L = x.FILE_L!=null?new Models.FileItemBase()
                    {
                        ID=x.FILE_L.ID,
                        STATUS = x.FILE_L.STATUS,
                        COMENT = x.FILE_L.COMENT,
                        FILENAME = x.FILE_L.FILENAME,
                        TYPE_FILE = x.FILE_L.TYPE_FILE
                    } : null
                }).ToList();
                var lvm = new LoadReestViewModel
                {
                    FileList = files,
                    SNILS_SIGN = new EditableList<SNILS_SIGN>(),
                    CODE_MO = userInfo.CODE_MO,
                    NAME_OK = userInfo.CODE_MO_NAME
                };
                var wcfStatus = await WcfConnect.GetStatusAsync();

                lvm.ReestrEnabled = wcfStatus.ReestrEnabled;
                lvm.ConnectWCFon = wcfStatus.ConnectWCFon;
                lvm.TypePriem = wcfStatus.TypePriem;

               
                return lvm;
            }
            catch (Exception ex)
            {
                var lvm = new LoadReestViewModel();
                lvm.ListError.Add(new ErrorItem(ErrorT.TextRed, ex.FullError()));
                return lvm;
            }
        }

        private async Task<FILEPACK> GetFile()
        {
            var currentPack = await MyPACK();
            foreach (var f in currentPack.FILES)
            {
                var path = medpomFileManager.GetPath(userInfo.CODE_MO, f.FILENAME);
                if (System.IO.File.Exists(path)) continue;
                f.STATUS = STATUS_FILE.NOT_INVITE;
                f.COMENT = "Файл не найден на сервере!";
            }
            await MyOracleSet.SaveChangesAsync();
            return currentPack;
        }
       


        /// <summary>
        /// Загрузка файлов на сервер
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<CustomJsonResult> Upload()
        {
            var error = new List<ErrorItem>();
            try
            {
                var pack = await MyPACK();
                if (Request.Form.Files.Count > 0)
                {
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(file.FileName).ToUpper();
                        var fileName = Path.GetFileName(file.FileName).ToUpper();
                        if (ext != ".ZIP" && ext != ".XML")
                        {
                            error.Add(new ErrorItem(ErrorT.TextRed, $"Файл {Path.GetFileName(file.FileName)} имеет неверный формат. Файл не загружен!"));
                            continue;
                        }

                        if (pack.FILES.Any(x => x.FILENAME == fileName))
                        {
                            error.Add(new ErrorItem(ErrorT.TextRed, $"Файл {Path.GetFileName(file.FileName)} уже присутствует в списке. Файл не загружен!"));
                            continue;
                        }

                        var pathInRepo = await medpomFileManager.AddFile(userInfo.CODE_MO, fileName, file.OpenReadStream());
                        //Разархивировать
                        if (ext == ".ZIP")
                        {
                            var files = await zipArchiver.UnZip(pathInRepo);
                            foreach (var zipArchiverItem in files)
                            {
                                if (!string.IsNullOrEmpty(zipArchiverItem.FilePath))
                                    pack.FILES.Add(NewFile(zipArchiverItem.FilePath));
                                if (!string.IsNullOrEmpty(zipArchiverItem.Error))
                                    error.Add(new ErrorItem(ErrorT.TextRed, zipArchiverItem.Error));
                            }
                        }
                        else
                        {
                            pack.FILES.Add(NewFile(pathInRepo));
                        }
                    }
                    FindL(pack);
                    checkPack(pack);
                    await MyOracleSet.SaveChangesAsync();
                }
                CheckCatalog(pack);
                return CustomJsonResult.Create(error);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        private FILES NewFile(string FilePath)
        {
            var filename = Path.GetFileName(FilePath).ToUpper();
            var fs = new FILES { FILENAME = filename, DATECREATE = DateTime.Now, STATUS = STATUS_FILE.NOT_INVITE };
            TYPEFILE? tf = null;
            var FT = ParseFileName.Parse(filename);
            if (!FT.IsNull)
                tf = (TYPEFILE)FT.FILE_TYPE.ToFileType();
            fs.TYPE_FILE = tf;
            fs.HASH = hasher.GetHash(FilePath);
            return fs;
        }


        void CheckCatalog(FILEPACK PAC)
        {
            try
            {
                var dirPath = medpomFileManager.GetDir(PAC.CODE_MO);
                var dir = new DirectoryInfo(dirPath);
                foreach (var dirin in dir.GetDirectories())
                    Directory.Delete(dirin.FullName, true);

                foreach (var fi in dir.GetFiles())
                {
                    if (!PAC.FILES.Any(x => string.Equals(x.FILENAME, Path.GetFileName(fi.Name), StringComparison.CurrentCultureIgnoreCase)))
                        System.IO.File.Delete(fi.FullName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка в CheckCatalog:{ex.Message}");
            }
        }

        void FindL(FILEPACK FP)
        {
            try
            {
                for (var i = 0; i < FP.FILES.Count; i++)
                {
                    var fi = FP.FILES.ToList()[i];

                    var findfile = fi.FILENAME;
                    if (!fi.TYPE_FILE.HasValue) continue;
                    switch (fi.TYPE_FILE.Value)
                    {
                        case TYPEFILE.DD:
                        case TYPEFILE.DF:
                        case TYPEFILE.DO:
                        case TYPEFILE.DP:
                        case TYPEFILE.DR:
                        case TYPEFILE.DS:
                        case TYPEFILE.DU:
                        case TYPEFILE.DV:
                        case TYPEFILE.DA:
                        case TYPEFILE.DB:
                        case TYPEFILE.H:

                            findfile = $"L{findfile.Remove(0, 1)}";
                            break;
                        case TYPEFILE.T:
                        case TYPEFILE.C:
                            findfile = $"L{findfile}";
                            break;
                        default:
                            continue;
                    }
                    //Имя файла и не принадлежит уже ни кому
                    var lf = FP.FILES.FirstOrDefault(x => x.FILENAME == findfile && x.FILE_L == null);
                    if (lf != null)
                    {
                        fi.STATUS = STATUS_FILE.INVITE;
                        lf.STATUS = STATUS_FILE.INVITE;
                        fi.FILE_L = lf;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка в FindL:{ex.Message}");
            }

        }
        void checkPack(FILEPACK FP)
        {
            try
            {
                foreach (var fs in FP.FILES.Where(x => x.STATUS == STATUS_FILE.INVITE && !x.TYPE_FILE.Contains(TYPEFILE.LD, TYPEFILE.LF, TYPEFILE.LH, TYPEFILE.LO, TYPEFILE.LP, TYPEFILE.LR, TYPEFILE.LS, TYPEFILE.LT, TYPEFILE.LU, TYPEFILE.LV, TYPEFILE.LC, TYPEFILE.LA, TYPEFILE.LB)))
                {
                    var codeMo = SchemaChecking.GetELEMENT(medpomFileManager.GetPath(FP.CODE_MO, fs.FILENAME), "CODE_MO");
                    if (codeMo != userInfo.CODE_MO)
                    {
                        fs.STATUS = STATUS_FILE.NOT_INVITE;
                        fs.COMENT = "Код МО в файле(CODE_MO) не соответствует Вашей организации";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка в checkPack:{ex.Message}");
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> DeleteFile(int id)
        {
            try
            {
                var pac = await MyPACK();
                var item = pac.FILES.FirstOrDefault(x => x.ID == id);
                if (item == null)
                    throw new ModelException("",$"Файл с ID = {id} не найден");
                if (item.FILE_L != null)
                {
                    medpomFileManager.DeleteFile(userInfo.CODE_MO, item.FILE_L.FILENAME);
                    MyOracleSet.FILES.Remove(item.FILE_L);
                }

                if (item.PARENT != null)
                    item.PARENT.FILE_L = null;

                MyOracleSet.FILES.Remove(item);
                medpomFileManager.DeleteFile(userInfo.CODE_MO, item.FILENAME);
                await MyOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> Clear()
        {
            try
            {
                var pac = await MyPACK();
                if (pac != null)
                {
                    MyOracleSet.FILES.RemoveRange(MyOracleSet.FILES.Where(x => x.ID_PACK == pac.ID));
                    medpomFileManager.Clear(userInfo.CODE_MO);
                    await MyOracleSet.SaveChangesAsync();
                }
                return CustomJsonResult.Create(true);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> Send()
        {
            try
            {
                var usInfo = userInfoHelper.GetInfo(User.Identity.Name);
                var folderPath = medpomFileManager.GetDir(usInfo.CODE_MO);
                var err = new List<ErrorItem>();
                var wcfStatus = await WcfConnect.GetStatusAsync();

                //Отправить реестры на сервер
                if (wcfStatus.ConnectWCFon && wcfStatus.ReestrEnabled)
                {
                    var fp = new FilePacket
                    {
                        Status = StatusFilePack.Close,
                        StopTime = true,
                        Priory = 0,
                        CodeMO = usInfo.CODE_MO,
                        Files = new List<FileItem>()
                    };
                    var pac = await GetFile();
                    var files = pac.FILES.Where(x => x.PARENT == null).ToList();
                    fp.ID = pac.ID;
                    if (files.Count == 0)
                    {
                        err.Add(new ErrorItem(ErrorT.TextRed, $"Отсутствуют файлы на отправку!"));
                    }
                    foreach (var f in files)
                    {
                        if (f.STATUS != STATUS_FILE.INVITE)
                        {
                            err.Add(new ErrorItem(ErrorT.TextRed, $"Файл {f.FILENAME} не принят к загрузке. Исключите его из посылки!"));
                            continue;
                        }

                        if (f.FILE_L!=null && f.FILE_L.STATUS != STATUS_FILE.INVITE)
                        {
                            err.Add(new ErrorItem(ErrorT.TextRed, $"Файл {f.FILE_L.FILENAME} не принят к загрузке. Исключите его из посылки!"));
                            continue;
                        }

                        if (usInfo.WithSing == false)
                        {
                            if ((f.SIGN_ISP_VALID && f.SIGN_BUH_VALID && f.SIGN_DIR_VALID) != true && wcfStatus.TypePriem)
                            {
                                err.Add(new ErrorItem(ErrorT.TextRed, $"Файл {f.FILENAME} не подписан. Исключите его из посылки!"));
                                continue;
                            }

                            if (f.FILE_L != null && (f.FILE_L.SIGN_ISP_VALID && f.FILE_L.SIGN_BUH_VALID && f.FILE_L.SIGN_DIR_VALID) != true && wcfStatus.TypePriem)
                            {
                                err.Add(new ErrorItem(ErrorT.TextRed, $"Файл {f.FILE_L.FILENAME} не подписан. Исключите его из посылки!"));
                                continue;
                            }
                        }
                        var fi = new FileItem
                        {
                            FileName = f.FILENAME,
                            FilePach = Path.Combine(folderPath, f.FILENAME),
                            Process = StepsProcess.Invite,
                            Type = f.TYPE_FILE.ToFileType(),
                            DateCreate = DateTime.Now,
                            SIGN_ISP = f.SIGN_ISP_STR,
                            SIGN_DIR = f.SIGN_DIR_STR,
                            SIGN_BUH = f.SIGN_BUH_STR
                        };

                        var fl = new FileL
                        {
                            FileName = f.FILE_L.FILENAME,
                            SIGN_ISP = f.FILE_L.SIGN_ISP_STR,
                            SIGN_DIR = f.FILE_L.SIGN_DIR_STR,
                            SIGN_BUH = f.FILE_L.SIGN_BUH_STR,
                            FilePach = Path.Combine(folderPath, f.FILE_L.FILENAME),
                            Process = StepsProcess.Invite,
                            Type = (FileType)(f.FILE_L.TYPE_FILE),
                            DateCreate = DateTime.Now
                        };


                        fi.filel = fl;
                        fp.Files.Add(fi);
                    }

                    if (err.Count == 0)
                    {
                        WcfConnect.AddFilePacketForMO(fp);
                        pac.STATUS = STATUS_FILEPACK.SEND;
                        await MyOracleSet.SaveChangesAsync();
                        return CustomJsonResult.Create(true);
                    }
                    return CustomJsonResult.Create(err,false);
                }

                err.Add(new ErrorItem(ErrorT.TextRed, "Прием реестров неактивен"));
                return CustomJsonResult.Create(err, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка при отправке файлов: {ex.FullError()}", LogType.Error);
                return CustomJsonResult.Create(new List<ErrorItem> { new(ErrorT.TextRed, "Ошибка при отправке файлов: попробуйте еще раз или обратитесь в тех. поддержку") }, false);
            }
        }

        #endregion
      
        #region Справочник ошибок
  
        public async Task<CustomJsonResult> ErrorList(bool isShowClose = false)
        {
            try
            {
                var model = new EditErrorSPRViewModel()
                {
                    Sections = await MyOracleSet.ErrorSPRSection.Include(x => x.Error).OrderBy(x=>x.ORD).Select(x => new SectionSprModel
                    {
                        SECTION_NAME = x.SECTION_NAME,
                        Errors = x.Error.Where(x=>!x.D_END.HasValue || isShowClose).OrderByDescending(y => y.D_EDIT).ThenByDescending(y => y.ID_ERR).Select(y => new ErrorSprModel
                        {
                            D_EDIT = y.D_EDIT,
                            EXAMPLE = y.EXAMPLE,
                            OSN_TFOMS = y.OSN_TFOMS,
                            D_BEGIN = y.D_BEGIN,
                            D_END = y.D_END,
                            ISMEK = y.ISMEK,
                            ID_ERR = y.ID_ERR
                        }).ToList()
                    }).ToListAsync()
                };
                return CustomJsonResult.Create(model);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        public async Task<CustomJsonResult> GetError(int ID_ERR)
        {
            try
            {
                var model = await MyOracleSet.ErrorSPR.FirstOrDefaultAsync(x => x.ID_ERR == ID_ERR);
                if (model == null)
                    throw new ModelException($"Ошибка с кодом ID_ERR={ID_ERR} не найдена!");
                return CustomJsonResult.Create(new ErrorSprModel()
                {
                    D_EDIT = model.D_EDIT,
                    EXAMPLE = model.EXAMPLE,
                    ID_ERR = model.ID_ERR,
                    OSN_TFOMS = model.OSN_TFOMS,
                    TEXT = model.TEXT_STR,
                    ID_SECTION = model.ID_SECTION,
                    D_BEGIN = model.D_BEGIN,
                    D_END = model.D_END,
                    ISMEK = model.ISMEK
                });
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        public async Task<CustomJsonResult> GetSections()
        {
            try
            {
                var model = await MyOracleSet.ErrorSPRSection.OrderBy(x=>x.ORD).Select(x=>new SectionSprModel()
                {
                    SECTION_NAME = x.SECTION_NAME,
                    ID_SECTION = x.ID_SECTION
                }).ToListAsync();
              
                return CustomJsonResult.Create(model);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CustomJsonResult> AddErrorSPR(ErrorSprModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MyOracleSet.ErrorSPR.Add(new ErrorSPR()
                    {
                        D_EDIT = DateTime.Now,
                        ID_SECTION = model.ID_SECTION.Value,
                        EXAMPLE = model.EXAMPLE,
                        OSN_TFOMS = model.OSN_TFOMS,
                        TEXT_STR = model.TEXT,
                        D_BEGIN = model.D_BEGIN,
                        D_END = model.D_END,
                        ISMEK = model.ISMEK                        
                    });
                    await MyOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception e)
            {
                return InternalError(e,true);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CustomJsonResult> EditErrorSPR(ErrorSprModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = await MyOracleSet.ErrorSPR.FirstOrDefaultAsync(x => x.ID_ERR == model.ID_ERR);
                    if (item == null)
                        throw new ModelException($"Не удалось найти запись с ID_ERR={model.ID_ERR}");
                    item.D_EDIT = DateTime.Now;
                    item.ID_SECTION = model.ID_SECTION.Value;
                    item.EXAMPLE = model.EXAMPLE;
                    item.OSN_TFOMS = model.OSN_TFOMS;
                    item.TEXT_STR = model.TEXT;
                    item.D_BEGIN = model.D_BEGIN;
                    item.D_END = model.D_END;
                    item.ISMEK = model.ISMEK;
                    await MyOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(new List<string> { e.Message }, false);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<CustomJsonResult> RemoveErrorSPR(int ID_ERR)
        {
            try
            {
                var item = await MyOracleSet.ErrorSPR.FirstOrDefaultAsync(x => x.ID_ERR == ID_ERR);
                if (item == null)
                    throw new ModelException($"Не удалось найти запись с ID_ERR={ID_ERR}");
                MyOracleSet.ErrorSPR.Remove(item);
                await MyOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception e)
            {
                return InternalError(e, true);
            }
        }
        
        #endregion
    }

    public static class Ext
    {
        public static byte[] ReadFull(this Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
        public static string ReadToEnd(this MemoryStream BASE)
        {
            BASE.Position = 0;
            var r = new StreamReader(BASE);
            return r.ReadToEnd();
        }


        public static string HexToString(this byte[] hashValue)
        {
            return hashValue.Aggregate("", (current, t) => current + $"{t:X2}");
        }
    }
}

