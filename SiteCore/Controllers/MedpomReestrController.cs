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

        private string _CODE_MO;
        private string CODE_MO
        {
            get
            {
                if (string.IsNullOrEmpty(_CODE_MO))
                    _CODE_MO = User.CODE_MO();
                return _CODE_MO;
            }
        }
        public MedpomReestrController(WCFConnect WcfConnect, UserManager<ApplicationUser> userManager, MyOracleSet myOracleSet, ILogger logger, IZipArchiver zipArchiver, IMedpomRepository medpomFileManager, IHasher hasher)
        {
            this.WcfConnect = WcfConnect;
            this.userManager = userManager;
            this.MyOracleSet = myOracleSet;
            this.logger = logger;
            this.zipArchiver = zipArchiver;
            this.medpomFileManager = medpomFileManager;
            this.hasher = hasher;
        }

        #region Просмотр реестров
        public IActionResult ViewReestr()
        {
            return View();
        }

        public IActionResult ViewReestrAjax()
        {

            return PartialView("_ViewReestrPartial", GetViewReestrModel);
        }
        ViewReestrViewModel GetViewReestrModel
        {
            get
            {
                var VRVM = new ViewReestrViewModel { ConnectWCFon = WcfConnect.Ping() };
                if (!VRVM.ConnectWCFon) return VRVM;
                var CODE_MO = User.CODE_MO();
                if (!string.IsNullOrEmpty(CODE_MO))
                {
                    var t = WcfConnect.GetPackForMO(CODE_MO);
                    VRVM.FP = t.FP;
                    VRVM.Order = t.ORDER;
                }
                return VRVM;
            }
            
        }
        [HttpGet]
        public async Task<IActionResult> DownloadProtocol()
        {
            try
            {
                var pack = await MyPACK();
                var pserv = WcfConnect.GetPackForMO(CODE_MO);

                var file = File(WcfConnect.GetProtocol(CODE_MO), System.Net.Mime.MediaTypeNames.Application.Zip, Path.GetFileName(pserv.FP.PATH_ZIP));
                pack.DOWNPROT_LAST = DateTime.Now;
                await MyOracleSet.SaveChangesAsync();
                return file;
            }
            catch (Exception ex)
            {
                var t = new List<string> { ex.Message };
                TempData["Error"] = t;
                TempData["ReturnURL"] = Url.Action(nameof(ViewReestr));
                return RedirectToAction("Error", "Manage");
            }
        }

        #endregion
        #region Загрузка реестров
        private async Task<FILEPACK> MyPACK()
        {
            try
            {
                var p = await MyOracleSet.FILEPACK.Include(x => x.FILES).FirstOrDefaultAsync(x => x.CODE_MO == CODE_MO && x.STATUS == STATUS_FILEPACK.CURRENT);
                //Если нет до вставляем
                if (p != null) return p;
                p = new FILEPACK { CODE_MO = CODE_MO, STATUS = STATUS_FILEPACK.CURRENT };
                MyOracleSet.FILEPACK.Add(p);
                await MyOracleSet.SaveChangesAsync();
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка в MyPACK:{ex.Message}", ex);
            }
        }
        public IActionResult LoadReestr()
        {
            return View();
        }

        public async Task<IActionResult> LoadReestrData()
        {
            return PartialView("_LoadReestrPartial", await getLoadReestViewModel());
        }

        async Task<LoadReestViewModel> getLoadReestViewModel(List<ErrorItem> Error = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var lvm = new LoadReestViewModel
                {
                    PACKET = await GetFile(),
                    ConnectWCFon = WcfConnect.Ping(),
                    SNILS_SIGN = await GetListSIGN(),
                    CODE_MO = CODE_MO,
                    NAME_OK = user.CODE_MO_NAME?.NAM_MOK
                };
                if (Error != null)
                    lvm.ListError = Error;

                if (lvm.ConnectWCFon)
                {
                    lvm.ReestrEnabled = WcfConnect.ReestrEnabled();
                    lvm.TypePriem = WcfConnect.GetTypePriem();
                }
                else
                {
                    lvm.ReestrEnabled = false;
                }
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
            var CurrentPack = await MyPACK();
            foreach (var f in CurrentPack.FILES)
            {
                var path = medpomFileManager.GetPath(CODE_MO, f.FILENAME);
                if (System.IO.File.Exists(path)) continue;
                f.STATUS = STATUS_FILE.NOT_INVITE;
                f.COMENT = "Файл не найден на сервере!";
            }
            await MyOracleSet.SaveChangesAsync();
            return CurrentPack;
        }
        private Task<List<SNILS_SIGN>> GetListSIGN()
        {
            return Task.Run(() => { return MyOracleSet.SNILS_SIGN.Where(x => x.CODE_MO == CODE_MO).ToList(); });
        }


        /// <summary>
        /// Загрузка файлов на сервер
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var Error = new List<ErrorItem>();
            try
            {
                var PACK = await MyPACK();

                if (Request.Form.Files.Count > 0)
                {

                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(file.FileName).ToUpper();
                        var FileName = Path.GetFileName(file.FileName).ToUpper();
                        if (ext != ".ZIP" && ext != ".XML")
                        {
                            Error.Add(new ErrorItem(ErrorT.TextRed, $"Файл {Path.GetFileName(file.FileName)} имеет неверный формат. Файл не загружен!"));
                            continue;
                        }

                        if (PACK.FILES.Any(x => x.FILENAME == FileName))
                        {
                            Error.Add(new ErrorItem(ErrorT.TextRed, $"Файл {Path.GetFileName(file.FileName)} уже присутствует в списке. Файл не загружен!"));
                            continue;
                        }

                        var PathInRepo = await medpomFileManager.AddFile(CODE_MO, FileName, file.OpenReadStream());
                        //Разархивировать
                        if (ext == ".ZIP")
                        {
                            var files = await zipArchiver.UnZip(PathInRepo);
                            foreach (var zipArchiverItem in files)
                            {
                                if (!string.IsNullOrEmpty(zipArchiverItem.FilePath))
                                    PACK.FILES.Add(NewFile(zipArchiverItem.FilePath));
                                if (!string.IsNullOrEmpty(zipArchiverItem.Error))
                                    Error.Add(new ErrorItem(ErrorT.TextRed, zipArchiverItem.Error));
                            }
                        }
                        else
                        {
                            PACK.FILES.Add(NewFile(PathInRepo));
                        }
                    }
                    FindL(PACK);
                    checkPack(PACK);
                    await MyOracleSet.SaveChangesAsync();
                }
                CheckCatalog(PACK);
                Error.Add(new ErrorItem(ErrorT.TextGreen, "Файлы загружены успешно"));
                return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel(Error));
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Upload: {ex.FullError()}", LogType.Error);
                return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel(new List<ErrorItem> { new ErrorItem(ErrorT.TextRed, "Внутренняя ошибка сервиса!") }));

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
                var dir_path = medpomFileManager.GetDir(PAC.CODE_MO);
                var dir = new DirectoryInfo(dir_path);
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
                    var code_mo = SchemaChecking.GetELEMENT(medpomFileManager.GetPath(FP.CODE_MO, fs.FILENAME), "CODE_MO");
                    if (code_mo != CODE_MO)
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



        [HttpGet]
        public async Task<IActionResult> GetFileSig(string FileName)
        {
            try
            {
                var p = await MyPACK();
                var file = p.FILES.FirstOrDefault(x => x.FILENAME == FileName);
                if (file != null)
                {
                    await using var st = System.IO.File.OpenRead(medpomFileManager.GetPath(CODE_MO, file.FILENAME));

                    var bufISP = Encoding.UTF8.GetBytes(file.SIGN_ISP_STR);
                    var bufBUH = Encoding.UTF8.GetBytes(file.SIGN_BUH_STR);
                    var bufDIR = Encoding.UTF8.GetBytes(file.SIGN_DIR_STR);
                    var zip = zipArchiver.Zip(
                        new ZipArchiverEntry(file.FILENAME, st.ReadFull()),
                        new ZipArchiverEntry(Path.GetFileNameWithoutExtension(file.FILENAME) + ".ISP.SIG", bufISP),
                        new ZipArchiverEntry(Path.GetFileNameWithoutExtension(file.FILENAME) + ".DIR.SIG", bufDIR),
                        new ZipArchiverEntry(Path.GetFileNameWithoutExtension(file.FILENAME) + ".BUH.SIG", bufBUH));

                    var file_r = File(zip.ToArray(), System.Net.Mime.MediaTypeNames.Application.Zip, $"{Path.GetFileNameWithoutExtension(file.FILENAME)}.zip");
                    return file_r;
                }
                return View("LoadReestr", await getLoadReestViewModel(new List<ErrorItem> { new ErrorItem(ErrorT.TextRed, $"Не удалось найти файл {FileName}") }));
            }
            catch (Exception ex)
            {
                return View("LoadReestr", await getLoadReestViewModel(new List<ErrorItem> { new ErrorItem(ErrorT.TextRed, ex.FullError()) }));
            }
        }

   

        [HttpPost]
        public async Task<IActionResult> DeleteFile(decimal id)
        {
            var pac = await MyPACK();
            var t = pac.FILES.FirstOrDefault(x => x.ID == id);
            if (t != null)
            {
                if (t.FILE_L != null)
                {
                    pac.FILES.Remove(t.FILE_L);
                    medpomFileManager.DeleteFile(CODE_MO, t.FILE_L.FILENAME);
                }
                pac.FILES.Remove(t);
                medpomFileManager.DeleteFile(CODE_MO, t.FILENAME);
                await MyOracleSet.SaveChangesAsync();
            }
            return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel());

        }




        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var pac = await MyPACK();
            if (pac != null)
            {
                pac.FILES.Clear();
                medpomFileManager.Clear(CODE_MO);
                await MyOracleSet.SaveChangesAsync();
            }
            return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignData(Dictionary<string, string> SIGN)
        {
            try
            {
                var PACK = await MyPACK();
                var err = new List<ErrorItem>();
                foreach (var item in SIGN)
                {
                    var f = PACK.FILES.FirstOrDefault(x => String.Equals(x.FILENAME, item.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (f != null)
                    {
                        var sign = Convert.FromBase64String(item.Value);
                        var hash = await System.IO.File.ReadAllBytesAsync(medpomFileManager.GetPath(CODE_MO, f.FILENAME));
                        var check = hasher.Viryfi(sign, hash);

                        if (!check.Valid)
                        {
                            err.Add(new ErrorItem(ErrorT.TextRed, $"Для файла {f.FILENAME} подпись не действительна: {check.Comment}"));
                            continue;
                        }

                        var certResult = CheckCertificate(check.PublicKey.HexToString(), check.SN);
                        if (!certResult.Result)
                        {
                            err.Add(new ErrorItem(ErrorT.TextRed, $"Для файла {f.FILENAME}: {certResult.Exception}"));
                            continue;
                        }

                        if (certResult.Owner.HasFlag(SIGN_OWNER.BUH))
                        {
                            f.SIGN_BUH_STR = item.Value;
                            f.SIGN_BUH_VALID = true;
                        }
                        if (certResult.Owner.HasFlag(SIGN_OWNER.DIR))
                        {
                            f.SIGN_DIR_STR = item.Value;
                            f.SIGN_DIR_VALID = true;
                        }
                        if (certResult.Owner.HasFlag(SIGN_OWNER.ISP))
                        {
                            f.SIGN_ISP_STR = item.Value;
                            f.SIGN_ISP_VALID = true;
                        }
                    }
                }
                await MyOracleSet.SaveChangesAsync();
                return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel(err));
            }
            catch (Exception ex)
            {
                return PartialView("_LoadReestrFileListPartial", await getLoadReestViewModel(new List<ErrorItem> { new ErrorItem(ErrorT.TextRed, ex.FullError()) }));
            }
        }

        class CheckCertificateResult
        {
            public bool Result { get; set; }
            public string Exception { get; set; }
            public SIGN_OWNER Owner { get; set; }
        }




        private CheckCertificateResult CheckCertificate(string PublicKey, string SN)
        {
            var dtnow = DateTime.Now.Date;
            var valid = new CheckCertificateResult { Result = false };
            var SNILS_SIGNs = MyOracleSet.SNILS_SIGN.Where(row => row.CODE_MO == CODE_MO && (row.DATE_B <= dtnow && (row.DATE_E ?? dtnow) >= dtnow) && row.SN.ToUpper() == SN && row.PUBLICKEY == PublicKey);

            if (!SNILS_SIGNs.Any())
            {
                valid.Result = false;
                valid.Exception = $"Подпись действительна, но подписавший не является уполномоченным лицом для сдачи реестров(SN={SN})";
            }
            else
            {
                var owner = SIGN_OWNER.NONE;
                foreach (var t in SNILS_SIGNs.Select(x => x.OWNER).Distinct())
                {
                    owner |= t;
                }
                valid.Owner = owner;
                valid.Result = true;
            }
            return valid;
        }


        #endregion
        #region Инструкция
        public IActionResult InstructionView()
        {
            return View();
        }
        #endregion
        #region Справочник ошибок
        public IActionResult ErrorSPR()
        {
            return View("ErrorSPR/ErrorSPR", new EditErrorSPRViewModel { Section = MyOracleSet.ErrorSPRSection.Include(x=>x.Error).ToList() });
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditErrorSPR(int? ID_ERR = null)
        {
            var model = new EditErrorSPRViewModel()
            {
                Section = MyOracleSet.ErrorSPRSection.ToList(),
                Error = ID_ERR.HasValue ? MyOracleSet.ErrorSPR.Find(ID_ERR) : new ErrorSPR()
            };
            return PartialView("ErrorSPR/EditError", model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditErrorSPR(string TEXT_STR, string EXAMPLE, string OSN_TFOMS, int ID_SECTION, int? ID_ERR = null)
        {
            var Err = ID_ERR.HasValue ? await MyOracleSet.ErrorSPR.FirstOrDefaultAsync(x=>x.ID_ERR == ID_ERR.Value) : new ErrorSPR();
            if (Err != null)
            {
                Err.TEXT_STR = TEXT_STR;
                Err.EXAMPLE = EXAMPLE;
                Err.OSN_TFOMS = OSN_TFOMS;
                Err.ID_SECTION = ID_SECTION;
                Err.D_EDIT = DateTime.Now;
                if (!Err.ID_ERR.HasValue)
                    MyOracleSet.ErrorSPR.Add(Err);
                await MyOracleSet.SaveChangesAsync();
            }
            return ErrorList();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteError(int ID_ERR)
        {
            var item = await MyOracleSet.ErrorSPR.FirstOrDefaultAsync(x => x.ID_ERR == ID_ERR);
            if(item!=null)
                MyOracleSet.ErrorSPR.Remove(item);
            await MyOracleSet.SaveChangesAsync();
            return ErrorList();
        }


        public IActionResult ErrorList()
        {
            return PartialView("ErrorSPR/ErrorList", new EditErrorSPRViewModel { Section = MyOracleSet.ErrorSPRSection.Include(x=>x.Error).ToList() });
        }

        public IActionResult ViewDetailErrorSPR(int ID_ERR)
        {
            var item = MyOracleSet.ErrorSPR.Include(x=>x.Section).FirstOrDefault(x => x.ID_ERR == ID_ERR);
            return PartialView("ErrorSPR/ViewDetailErrorSPR", item);
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
            var R = new StreamReader(BASE);
            return R.ReadToEnd();
        }


        public static string HexToString(this byte[] hashValue)
        {
            return hashValue.Aggregate("", (current, t) => current + $"{t:X2}");
        }
    }
}

