using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using DocumentFormat.OpenXml.EMMA;
using ExcelManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "MSEAdmin, MSESmo")]
    public class MSEController : Controller
    {
        private IMSEExcelCreator mseExcelCreator { get; }
        private MSEOracleSet dbo { get; }
        private ILogger logger;
        private UserInfoHelper userInfoHelper;
        private UserInfo _userInfo;
        private UserInfo userInfo
        {
            get
            {
                if (_userInfo == null)
                {
                    _userInfo = userInfoHelper.GetInfo(User.Identity.Name);
                }
                return _userInfo;
            }
        }
        public MSEController(MSEOracleSet dbo, ILogger logger, IMSEExcelCreator mseExcelCreator, UserInfoHelper userInfoHelper)
        {
            this.dbo = dbo;
            this.logger = logger;
            this.mseExcelCreator = mseExcelCreator;
            this.userInfoHelper = userInfoHelper;
        }
        private async Task<CustomJsonResult> CreateInternalError(bool errorList = true)
        {
            return errorList ? CustomJsonResult.Create(await this.RenderViewAsync("_errorList", new List<string>{"Внутренняя ошибка сервиса" }, true), false) : CustomJsonResult.Create("Внутренняя ошибка сервиса", false);
        }
        // GET: MSE
        public async Task<IActionResult> MSE_REESTR()
        {
            return View(await GetIndexModel());
        }
        private string FindF014(IEnumerable<F014> F014, DateTime? DATEACT, int? S_OSN)
        {
            var dateB = DATEACT ?? DateTime.Now.Date;
            var v2 = F014.FirstOrDefault(x => x.KOD == S_OSN && dateB >= x.DATEBEG && dateB <= (x.DATEEND ?? DateTime.Now));
            return v2 != null ? v2.FullName : $"Нет значения из справочника F014, код = {S_OSN}";
        }
        public async Task<CustomJsonResult> GetMSEList(MSEFillter filter = null, int Page = 1, int CountOnPage = 200)
        {
            try
            {
                var Nodes = GetNodes(filter);
                var listq = GetNodesPage(GetNodes(filter), Page, CountOnPage);
                var count = Nodes.Count();
                var list = await GetMSE_ITEMModel(listq);
                return CustomJsonResult.Create(new GetMSEListModel(){ items = list, count = count});
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetMSEList)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        private async Task<List<MSE_ITEMModel>> GetMSE_ITEMModel(IQueryable<MSE_TF01> Nodes)
        {
            var F014 = await dbo.F014.ToListAsync();
            var V002 = await dbo.V002.ToListAsync();
            return (await Nodes
                .Include(x => x.SMO_NAME)
                .Include(x => x.Sluch).ThenInclude(x => x.Expertizes).ThenInclude(x => x.OSN)
                .Include(x=>x.DS_NAME)
                .ToListAsync()).Select(x =>
                new MSE_ITEMModel()
                {
                    MSE_ID = x.MSE_ID,
                    ENP = x.ENP_IDENT,
                    NAM_MOK = x.NAM_MO,
                    FIO = x.FIO,
                    SMO = x.SMO_IDENT=="75" ? "ТФОМС Забайкальского края":  x.SMO_NAME?.NAM_SMOK,
                    DATE_MEK = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.MEK).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_MEK = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.MEK).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    DATE_MEE = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.MEE).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_MEE = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.MEE).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    DATE_EKMP = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.EKMP).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_EKMP = string.Join(";", x.Sluch.SelectMany(x => x.Expertizes).Where(y => y.S_TIP == ExpertTip.EKMP).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    D_FORM = x.D_FORM,
                    D_PROT = x.D_PROT,
                    DS_NAME = x.DS_NAME?.NAME,
                    PRNAME = V002.FirstOrDefault(y=>y.IDPR == x.PROFIL && y.DATEBEG<=x.DATE_LOAD && (y.DATEEND??DateTime.Now)>=x.DATE_LOAD)?.PRNAME,
                    N_PROT = x.N_PROT,
                    REASON_R = x.REASON_R,
                    SNILS = x.SNILS
                }
            ).ToList();
        }

        public async Task<CustomJsonResult> ViewMSEItem()
        {
            try
            {
                return CustomJsonResult.Create(await this.RenderViewAsync("ViewMSEItem", new{}, true));
            }
            catch (Exception ex)
            {
               logger?.AddLog($"Ошибка в {nameof(View)}: {ex.Message}",  LogType.Error);
               return await CreateInternalError(false);
            }
        }

        public async Task<CustomJsonResult> GetSPR()
        {
            try
            {
                var PROFIL = await dbo.V002.OrderBy(x=>x.PRNAME).ToListAsync();
                var MKB = await dbo.MKB.OrderBy(x=>x.MKB).ToListAsync();
                var EXPERTS = await dbo.EXPERTS.Where(x=>x.N_EXPERT.StartsWith("75")).OrderBy(x => x.N_EXPERT).ToListAsync();
                var F014 = await dbo.F014.OrderBy(x=>x.OSN).ToListAsync();
                return CustomJsonResult.Create(new {PROFIL, MKB, EXPERTS, F014 });
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetSPR)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        public async Task<CustomJsonResult> GetMSEItem(int MSE_ID)
        {
            try
            {
                var item = await dbo.MSE_TF01
                    .Include(x=>x.SMO_NAME)
                    .Include(x=>x.DS_NAME)
                    .Include(x=>x.Sluch).ThenInclude(x=>x.Expertizes).ThenInclude(x=>x.OSN)
                    .FirstOrDefaultAsync(x => x.MSE_ID == MSE_ID);
             
                if (User.IsInRole("MSESmo"))
                {
                    if (item.SMO_IDENT != userInfo.CODE_SMO)
                        throw new ModelException("", "Данные не доступны для Вашей СМО");
                }
                if (item == null)
                    throw new ModelException("", "Не удалось найти запись");

                return CustomJsonResult.Create(new MSE_Item
                {
                    DATE_LOAD = item.DATE_LOAD,
                    ENP = item.ENP_IDENT,
                    NAM_MO = item.NAM_MO,
                    FAM = item.FAM,
                    IM = item.IM,
                    OT = item.OT,
                    DR = item.DR,
                    N_PROT = item.N_PROT,

                    D_PROT = item.D_PROT,
                    D_FORM = item.D_FORM,
                    SMO_NAME =  item.SMO_IDENT=="75"? "ТФОМС Забайкальского края": item.SMO_NAME?.NAM_SMOK,
                    MSE_ID = item.MSE_ID,
                    SNILS = item.SNILS,
                    REASON_R = item.REASON_R,
                    SMO = item.SMO_IDENT,
                    
                    SMO_DATA = new MSE_SMO_DATAModel
                    {
                        DS_NAME = item.DS_NAME?.NAME,
                        DS =  item.DS,
                        PROFIL_NAME = !item.PROFIL.HasValue? "Нет профиля" : (await dbo.V002.FirstOrDefaultAsync(x=>x.IDPR == item.PROFIL.Value && item.DATE_LOAD >= x.DATEBEG && item.DATE_LOAD <= (x.DATEEND ?? DateTime.Now)))?.PRNAME,
                        PROFIL = item.PROFIL,
                        SMO_COM = item.SMO_COM,
                        MSE_ID = item.MSE_ID
                    },
                    SLUCH = item.Sluch.Select(x=>
                        new MSE_SLUCHModel
                        {
                            DATE_1 = x.DATE_1,
                            DATE_2 = x.DATE_2,
                            MSE_SLUCH_ID = x.MSE_SLUCH_ID,
                            N_HISTORY = x.N_HISTORY,
                            Expertizes = x.Expertizes.Select(x=>new MSE_ExpertizeModel()
                            {
                                DATEACT = x.DATEACT,
                                EXPERTIZE_ID = x.EXPERTIZE_ID,
                                FIO = x.FIO,
                                NUMACT = x.NUMACT,
                                N_EXP = x.N_EXP,
                                MSE_SLUCH_ID = x.MSE_SLUCH_ID,
                                S_TIP = x.S_TIP,
                                OSN = x.OSN.Select(x=>new MSE_ExpertizeOSNModel()
                                {
                                    S_OSN = x.S_OSN,
                                    OSN_ID = x.OSN_ID,
                                    S_SUM = x.S_SUM,
                                    S_FINE = x.S_FINE,
                                    S_COM = x.S_COM
                                }).ToList()
                            }).ToList()
                        }).ToList()
                });
               
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetMSEItem)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        
        private async  Task<MSEIndexModel> GetIndexModel()
        {
            var sm = await dbo.MSE_TF01.Include(x=>x.SMO_NAME).Select(x => x.SMO_NAME).Distinct().Where(x => x != null).ToListAsync();
            sm.InsertRange(0, new List<SMO> { new() { NAM_SMOK = "Без СМО", SMOCOD = "null" }, new() { NAM_SMOK = "ТФОМС ЗК", SMOCOD = "75" } });
            var model = new MSEIndexModel
            {
                CODE_SMO = sm
            };
            return model;
        }
        private IQueryable<MSE_TF01> GetNodesPage(IQueryable<MSE_TF01> Nodes, int Page, int CountOnPage)
        {
            return Nodes.Skip((Page - 1) * CountOnPage).Take(CountOnPage);
        }
        private IQueryable<MSE_TF01> GetNodes(MSEFillter filter = null)
        {
            IQueryable<MSE_TF01> nodes = null;
            if (User.IsInRole("MSESmo"))
            {
                nodes = dbo.MSE_TF01.Where(x => !string.IsNullOrEmpty(x.SMO_IDENT) && x.SMO_IDENT == userInfo.CODE_SMO);
            }

            if (User.IsInRole("MSEAdmin"))
            {
                nodes = dbo.MSE_TF01;
            }

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.ENP))
                    nodes = nodes.Where(x => x.ENP_IDENT.ToUpper().Contains(filter.ENP.ToUpper()));
                if (!string.IsNullOrEmpty(filter.FAM))
                    nodes = nodes.Where(x => x.FAM.ToUpper().Contains(filter.FAM.ToUpper()));
                if (!string.IsNullOrEmpty(filter.IM))
                    nodes = nodes.Where(x => x.IM.ToUpper().Contains(filter.IM.ToUpper()));
                if (!string.IsNullOrEmpty(filter.OT))
                    nodes = nodes.Where(x => x.OT.ToUpper().Contains(filter.OT.ToUpper()));
                if (filter.DR.HasValue)
                    nodes = nodes.Where(x => x.DR == filter.DR);

                if (filter.D_PROT_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.D_PROT >= filter.D_PROT_BEGIN.Value);
                if (filter.D_PROT_END.HasValue)
                    nodes = nodes.Where(x => x.D_PROT <= filter.D_PROT_END.Value);

                if (filter.D_FORM_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.D_FORM >= filter.D_FORM_BEGIN.Value);
                if (filter.D_FORM_END.HasValue)
                    nodes = nodes.Where(x => x.D_FORM <= filter.D_FORM_END.Value);

                if (!string.IsNullOrEmpty(filter.N_PROT))
                    nodes = nodes.Where(x => x.N_PROT.ToUpper().Contains(filter.N_PROT.ToUpper()));

                if (!string.IsNullOrEmpty(filter.CODE_MO))
                    nodes = nodes.Where(x => x.NAM_MO.ToUpper().Contains(filter.CODE_MO.ToUpper()));

                if (!string.IsNullOrEmpty(filter.SNILS))
                    nodes = nodes.Where(x => x.SNILS.ToUpper().Contains(filter.SNILS.ToUpper()));

                if (filter.SMO != null)
                {
                    nodes = nodes.Where(x => filter.SMO.Contains(x.SMO_IDENT ?? "null"));
                }
            }

            return nodes.OrderByDescending(x => x.MSE_ID);
        }
        [Authorize(Roles = "MSEAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> SetAsMTR([FromBody]int MSE_ID)
        {
            try
            {
                var item = await dbo.MSE_TF01.FirstOrDefaultAsync(x => x.MSE_ID == MSE_ID);
                if (item == null)
                    throw new ModelException("", $"Не удалось найти случай с №{MSE_ID}");
                item.SMO_IDENT = "75";
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(SetAsMTR)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpGet]
        public async Task<CustomJsonResult> EditSluch(int MSE_ID = -1, int? MSE_SLUCH_ID = null)
        {
            var model = new AddSLUCH_MSEModel
            {
                SLUCH = new MSE_TF01_SLUCHModel(),
                MSE_ID = MSE_ID,
                MSE_SLUCH_ID = MSE_SLUCH_ID
            };
            if (MSE_SLUCH_ID.HasValue)
            {
                var item = await dbo.SLUCH.FirstOrDefaultAsync(x => x.MSE_SLUCH_ID == MSE_SLUCH_ID.Value);
                model.SLUCH = new MSE_TF01_SLUCHModel()
                {
                    DATE_1 = item.DATE_1,
                    DATE_2 = item.DATE_2,
                    N_HISTORY = item.N_HISTORY
                };
            }
            return CustomJsonResult.Create(await this.RenderViewAsync("EditSluch", model, true));

        }

        [HttpPost]
        public async Task<CustomJsonResult> EditSluch(AddSLUCH_MSEModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var r = await dbo.MSE_TF01.FirstOrDefaultAsync(x => x.MSE_ID == model.MSE_ID && x.SMO_IDENT == userInfo.CODE_SMO);
                    if (r == null)
                    {
                        throw new ModelException("", $"Запись МСЭ не найдена для СМО ={userInfo.CODE_SMO}");
                    }

                    if (!model.MSE_SLUCH_ID.HasValue)
                    {
                        r.Sluch.Add(new MSE_TF01_SLUCH()
                        {
                            DATE_1 = model.SLUCH.DATE_1.Value,
                            DATE_2 = model.SLUCH.DATE_2.Value,
                            N_HISTORY = model.SLUCH.N_HISTORY,
                            USER_ID = userInfo.USER_ID
                        });
                    }
                    else
                    {
                        var t = await dbo.SLUCH.FirstOrDefaultAsync(x => x.MSE_SLUCH_ID == model.MSE_SLUCH_ID.Value);
                        if (t != null)
                        {
                            t.DATE_1 = model.SLUCH.DATE_1.Value;
                            t.DATE_2 = model.SLUCH.DATE_2.Value;
                            t.N_HISTORY = model.SLUCH.N_HISTORY;
                        }
                    }
                    await dbo.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }
                throw new ModelException(null, null);
            }
            catch (ModelException ex)
            {
                if (ex.Key != null)
                    ModelState.AddModelError(ex.Key, ex.Message);
                return CustomJsonResult.Create(await this.RenderViewAsync("EditSluch", model, true), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditSluch)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }

        }

        [HttpPost]
        public async Task<CustomJsonResult> DeleteSluch([FromBody] int MSE_SLUCH_ID)
        {
            try
            {
                var sl = await dbo.SLUCH.FirstOrDefaultAsync(x => x.MSE_SLUCH_ID == MSE_SLUCH_ID);
                if (sl != null)
                {
                    dbo.SLUCH.Remove(sl);
                    await dbo.SaveChangesAsync();
                }
                return CustomJsonResult.Create(null);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(DeleteSluch)}: {ex.Message}", LogType.Error);
                return await CreateInternalError();
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> SetSMO_DATA([FromBody] MSE_SMO_DATAModel model)
        {
            try
            {
                var mse = await dbo.MSE_TF01.FirstOrDefaultAsync(x => x.MSE_ID == model.MSE_ID);
                if (mse == null)
                {
                    throw new ModelException("", $"Запись №{model.MSE_ID} не найдена в реестре");
                }
                mse.DS = model.DS;
                mse.PROFIL = model.PROFIL;
                mse.SMO_COM = model.SMO_COM;
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(SetSMO_DATA)}: {ex.Message}", LogType.Error);
                return await CreateInternalError();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditExpertize()
        {
            try
            {
                return CustomJsonResult.Create(await this.RenderViewAsync("EditExpertize", new {}, true));
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditExpertize)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> EditExpertize([FromBody] MSE_ExpertizeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sl = await dbo.SLUCH.Include(x => x.MSE_TF01).FirstOrDefaultAsync(x => x.MSE_TF01.SMO_IDENT == userInfo.CODE_SMO && x.MSE_SLUCH_ID == model.MSE_SLUCH_ID);
                    if (sl == null)
                    {
                        throw new ModelException("", $"Случай №{model.MSE_SLUCH_ID} не найден для СМО ={userInfo.CODE_SMO}");
                    }

                    if (!string.IsNullOrEmpty(model.N_EXP))
                    {
                        var expert = await dbo.EXPERTS.FirstOrDefaultAsync(x => x.N_EXPERT == model.N_EXP);
                        if (expert != null)
                        {
                            model.FIO = expert.FIO;
                        }
                    }

                    MSEExpertize exp = null;
                    if (model.EXPERTIZE_ID == 0)
                    {
                        exp = new MSEExpertize()
                        {
                            DATEACT = model.DATEACT,
                            DATE_INVITE = DateTime.Now,
                            FIO = model.FIO,
                            MSE_SLUCH = sl,
                            NUMACT = model.NUMACT,
                            N_EXP = model.N_EXP,
                            S_TIP = model.S_TIP,
                            USER_ID = userInfo.USER_ID,
                            OSN = model.OSN?.Select(x => new MSEExpertizeOSN()
                            {
                                S_COM = x.S_COM,
                                S_FINE = x.S_FINE,
                                S_OSN = x.S_OSN,
                                S_SUM = x.S_SUM
                            }).ToList()
                        };
                        dbo.Expertizes.Add(exp);
                    }
                    else
                    {
                        exp = await dbo.Expertizes.Where(x => x.EXPERTIZE_ID == model.EXPERTIZE_ID).FirstOrDefaultAsync();
                        if (exp == null) throw new ModelException("", "Исходная экспертиза не найдена для редактирования");
                        exp.CopyFrom(model);
                    }

                    await dbo.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }

                throw new ModelException(null, null);
            }
            catch (ModelException ex)
            {
                if (ex.Key != null)
                    ModelState.AddModelError(ex.Key, ex.Message);
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditExpertize)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpGet]
        public async Task<CustomJsonResult> GetExpertize(int EXPERTIZE_ID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = await dbo.Expertizes.Include(x=>x.OSN).FirstOrDefaultAsync(x => x.EXPERTIZE_ID == EXPERTIZE_ID);
                    if (item == null)
                    {
                        throw new ModelException("", $"Запись экспертиза не найдена для СМО ={userInfo.CODE_SMO}");
                    }
                    return CustomJsonResult.Create(new MSE_ExpertizeModel()
                    {
                        DATEACT = item.DATEACT,
                        FIO = item.FIO,
                        NUMACT =  item.NUMACT,
                        N_EXP =  item.N_EXP,
                        S_TIP = item.S_TIP,
                        EXPERTIZE_ID = item.EXPERTIZE_ID,
                        MSE_SLUCH_ID =  item.MSE_SLUCH_ID,
                        OSN = item.OSN.Select(x=>new MSE_ExpertizeOSNModel
                        {
                            S_OSN = x.S_OSN,
                            S_COM = x.S_COM,
                            S_FINE =  x.S_FINE,
                            S_SUM =  x.S_SUM,
                            OSN_ID = x.OSN_ID
                        }).ToList()
                    });
                }
                throw new ModelException(null, null);
            }
            catch (ModelException ex)
            {
                if (ex.Key != null)
                    ModelState.AddModelError(ex.Key, ex.Message);
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetExpertize)}: {ex.Message}", LogType.Error);
                return await CreateInternalError();
            }

        }



        [HttpPost]
        public async Task<CustomJsonResult> DeleteExpertize([FromBody]int EXPERTIZE_ID)
        {
            try
            {
                var item = await dbo.Expertizes.Include(x => x.MSE_SLUCH).ThenInclude(x => x.MSE_TF01).Include(x => x.OSN).FirstOrDefaultAsync(x => x.EXPERTIZE_ID == EXPERTIZE_ID && x.MSE_SLUCH.MSE_TF01.SMO_IDENT == userInfo.CODE_SMO);
                if (item == null)
                    throw new ModelException("", $"Не удалось найти экспертизу №{EXPERTIZE_ID} для СМО {userInfo.CODE_SMO}");
                dbo.Expertizes.Remove(item);
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(null);

            }
            catch (ModelException ex)
            {
                if (ex.Key != null)
                    ModelState.AddModelError(ex.Key, ex.Message);
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(DeleteExpertize)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }

        }



        public async Task<IActionResult> GetMSEReestrFile()
        {
            var file = File(mseExcelCreator.GetMSEReestrXLSX(await GetMSE_ITEMModel(GetNodes())), System.Net.Mime.MediaTypeNames.Application.Octet, $"Реестр МСЭ от {DateTime.Now:dd.MM.yyyy}.xlsx");
            return file;
        }



      



        #region Report
        [Authorize(Roles = "MSEAdmin")]
        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }
        [Authorize(Roles = "MSEAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetReportData(DateTime Date1, DateTime Date2, bool IsSMO)
        {
            var rep = await dbo.GetReport(Date1, Date2, IsSMO);
            HttpContext.Session.Set("ReportResult", rep);
            HttpContext.Session.Set("Date1MSE", Date1);
            HttpContext.Session.Set("Date2MSE", Date2);
            return PartialView("_ReportTable", rep);
        }

        [Authorize(Roles = "MSEAdmin")]
        [HttpGet]
        public ActionResult GetReportXLSXFile()
        {
            var Items = HttpContext.Session.Get<List<ReportMSERow>>("ReportResult");
            var Date1MSE = HttpContext.Session.Get<DateTime>("Date1MSE");
            var Date2MSE = HttpContext.Session.Get<DateTime>("Date2MSE");
            if (Items != null)
            {
                var file = File(mseExcelCreator.GetReportXLS(Items), System.Net.Mime.MediaTypeNames.Application.Octet, $"Отчет МСЭ c {Date1MSE.Date:dd.MM.yyyy} по {Date2MSE.Date:dd.MM.yyyy}.xlsx");
                return file;
            }
            return BadRequest();
        }
        #endregion


    
    }
}
