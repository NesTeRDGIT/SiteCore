using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "TMKSMO, TMKUser, TMKAdmin, TMKReader")]
    public class TMKReestrController : Controller
    {
        private TMKOracleSet dbo { get; }
        private ILogger logger { get; }
        private ITMKExcelCreator tmkExcelCreator { get; }
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

        public TMKReestrController(ILogger logger, TMKOracleSet dbo, ITMKExcelCreator tmkExcelCreator, UserInfoHelper userInfoHelper)
        {
            this.logger = logger;
            this.dbo = dbo;
            this.tmkExcelCreator = tmkExcelCreator;
            this.userInfoHelper = userInfoHelper;
        }
        public IActionResult TMKReestr()
        {
            return View(GetIndexModel());
        }
        private async Task<CustomJsonResult> CreateInternalError(bool errorList = true)
        {
            return errorList ? CustomJsonResult.Create(await this.RenderViewAsync("_errorList", "Внутренняя ошибка сервиса", true), false) : CustomJsonResult.Create("Внутренняя ошибка сервиса", false);
        }
        private string GetStatusCom(TMKReestr r)
        {
            var isExpert = r.Expertizes.Any();
            return r.STATUS switch
            {
                StatusTMKRow.Closed => $"Закрыта{(isExpert ? ", есть экспертиза" : "")}",
                StatusTMKRow.Open => $"Открыта{(isExpert ? ", есть экспертиза" : "")}",
                StatusTMKRow.Error => $"Ошибочная:{r.STATUS_COM} {(isExpert ? ", есть экспертиза" : "")}",
                _ => null
            };
        }
        private IndexModel GetIndexModel()
        {
            var model = new IndexModel
            {
                NMIC_OPLATA = dbo.OPLATA.ToList(),
                NMIC_VID_NHISTORY = dbo.VID_NHISTORY.OrderBy(x => x.ORD).ToList(),
                CODE_MO = dbo.TMKReestr.Select(x => x.CODE_MO_NAME).Distinct().ToList(),
                CODE_SMO = dbo.TMKReestr.Select(x => x.SMO_NAME).Distinct().Where(x => x != null).ToList()
            };
            model.CODE_SMO.InsertRange(0, new List<SMO> { new() { NAM_SMOK = "Без СМО", SMOCOD = "null" }, new() { NAM_SMOK = "ТФОМС ЗК", SMOCOD = "75" } });
            return model;
        }

        private string FindF014(IEnumerable<F014> F014, DateTime? DATEACT, int? S_OSN)
        {
            var dateB = DATEACT ?? DateTime.Now.Date;
            var v2 = F014.FirstOrDefault(x => x.KOD == S_OSN && dateB >= x.DATEBEG && dateB <= (x.DATEEND ?? DateTime.Now));
            return v2 != null ? v2.FullName : $"Нет значения из справочника F014, код = {S_OSN}";
        }
        private IQueryable<TMKReestr> GetNodesPage(IQueryable<TMKReestr> Nodes, int Page, int CountOnPage)
        {
            return Nodes.Skip((Page - 1) * CountOnPage).Take(CountOnPage);
        }


        private async Task<IEnumerable<TMKListModel>> GetTMKListModel(IQueryable<TMKReestr> Nodes)
        {
            var F014 = await dbo.F014.ToListAsync();
            return (await Nodes
                    .Include(x => x.CODE_MO_NAME)
                    .Include(x => x.N_VID_NHISTORY)
                    .Include(x => x.N_OPLATA)
                    .Include(x => x.Expertizes)
                    .Include(x => x.Expertizes).ThenInclude(x => x.OSN)
                    .Include(x => x.CONTACT_INFO)
                    .Include(x => x.TMIS_NAME)
                    .Include(x => x.NMIC_NAME)
                    .ToListAsync())
                .Select(x => new TMKListModel
                {
                    ENP = x.ENP,
                    NAM_MOK = x.CODE_MO_NAME?.NAM_MOK,
                    CODE_MO = x.CODE_MO,
                    FIO = x.FIO,
                    DATE_B = x.DATE_B,
                    DATE_QUERY = x.DATE_QUERY,
                    DATE_PROTOKOL = x.DATE_PROTOKOL,
                    DATE_TMK = x.DATE_TMK,
                    SMO = x.SMO,
                    VID_NHISTORY = x.N_VID_NHISTORY?.VID_NHISTORY,
                    OPLATA = x.N_OPLATA?.OPLATA,
                    CONTACT_INFO = x.CONTACT_INFO.Count == 0 ? "Нет контактов МО" : string.Join(Environment.NewLine, x.CONTACT_INFO.Select(y => y.TelAndFio)),
                    DATE_MEK = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.MEK).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_MEK = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.MEK).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    DATE_MEE = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.MEE).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_MEE = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.MEE).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    DATE_EKMP = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.EKMP).Select(y => y.DATEACT.ToString("dd.MM.yyyy"))),
                    DEF_EKMP = string.Join(";", x.Expertizes.Where(y => y.S_TIP == ExpertTip.EKMP).SelectMany(y => y.OSN, (y, z) => new {y.DATEACT, z.S_OSN}).Select(y => FindF014(F014, y.DATEACT, y.S_OSN))),
                    STATUS = x.STATUS.ToString(),
                    STATUS_COM = GetStatusCom(x),
                    isEXP = x.Expertizes.Any(),
                    TMK_ID = x.TMK_ID,
                    TMIS_NAME = x.TMIS_NAME?.TMS_NAME,
                    NMIC_NAME = x.NMIC_NAME?.NMIC_NAME
                });
        }


        [HttpGet]
        public async Task<CustomJsonResult> GetTMKList(int Page, int CountOnPage, TMKFillter filter = null)
        {
            try
            {
                var Nodes = GetNodes(filter);
                var listq = GetNodesPage(GetNodes(filter), Page, CountOnPage);
                var count = Nodes.Count();
                var list = await GetTMKListModel(listq);
                return CustomJsonResult.Create(new { count, items = list });
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в GetTMKList:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }

        }
        /// <summary>
        /// Получить список в соответсвии с правами пользователя
        /// </summary>
        /// <returns></returns>
        private IQueryable<TMKReestr> GetNodes(TMKFillter filter = null)
        {
            IQueryable<TMKReestr> nodes = null;
            if (User.IsInRole("TMKSMO"))
            {
                nodes = dbo.TMKReestr.Where(x => !string.IsNullOrEmpty(x.SMO) && x.SMO == userInfo.CODE_SMO && x.STATUS == StatusTMKRow.Closed);
            }

            if (User.IsInRole("TMKUser"))
            {
                nodes = dbo.TMKReestr.Where(x => x.CODE_MO == userInfo.CODE_MO);
            }

            if (User.IsInRole("TMKReader"))
            {
                nodes = dbo.TMKReestr.Where(x => x.STATUS == StatusTMKRow.Closed);
            }

            if (User.IsInRole("TMKAdmin"))
            {
                nodes = dbo.TMKReestr;
            }

            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.ENP))
                    nodes = nodes.Where(x => x.ENP.ToUpper().Contains(filter.ENP.ToUpper()));
                if (!string.IsNullOrEmpty(filter.FAM))
                    nodes = nodes.Where(x => x.FAM.ToUpper().Contains(filter.FAM.ToUpper()));
                if (!string.IsNullOrEmpty(filter.IM))
                    nodes = nodes.Where(x => x.IM.ToUpper().Contains(filter.IM.ToUpper()));
                if (!string.IsNullOrEmpty(filter.OT))
                    nodes = nodes.Where(x => x.OT.ToUpper().Contains(filter.OT.ToUpper()));
                if (filter.DR.HasValue)
                    nodes = nodes.Where(x => x.DR == filter.DR);

                if (filter.DATE_B_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.DATE_B >= filter.DATE_B_BEGIN.Value);
                if (filter.DATE_B_END.HasValue)
                    nodes = nodes.Where(x => x.DATE_B <= filter.DATE_B_END.Value);

                if (filter.DATE_PROTOKOL_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.DATE_PROTOKOL >= filter.DATE_PROTOKOL_BEGIN.Value);
                if (filter.DATE_PROTOKOL_END.HasValue)
                    nodes = nodes.Where(x => x.DATE_PROTOKOL <= filter.DATE_PROTOKOL_END.Value);

                if (filter.DATE_QUERY_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.DATE_QUERY >= filter.DATE_QUERY_BEGIN.Value);
                if (filter.DATE_QUERY_END.HasValue)
                    nodes = nodes.Where(x => x.DATE_QUERY <= filter.DATE_QUERY_END.Value);

                if (filter.DATE_TMK_BEGIN.HasValue)
                    nodes = nodes.Where(x => x.DATE_TMK >= filter.DATE_TMK_BEGIN.Value);
                if (filter.DATE_TMK_END.HasValue)
                    nodes = nodes.Where(x => x.DATE_TMK <= filter.DATE_TMK_END.Value);

                if (filter.SMO != null)
                {
                    nodes = nodes.Where(x => filter.SMO.Contains(x.SMO ?? "null"));
                }


                if (filter.CODE_MO != null)
                    nodes = nodes.Where(x => filter.CODE_MO.Contains(x.CODE_MO));

                if (filter.VID_NHISTORY != null)
                    nodes = nodes.Where(x => filter.VID_NHISTORY.Contains(x.VID_NHISTORY));

                if (filter.OPLATA != null)
                    nodes = nodes.Where(x => filter.OPLATA.Contains(x.OPLATA));


            }

            return nodes.OrderByDescending(x => x.TMK_ID);
        }


        public async Task<IActionResult> GetTMKReestrFile(int Page, int CountOnPage, TMKFillter filter = null)
        {
            var nodes = GetNodesPage(GetNodes(filter), Page, CountOnPage);
            var items = await GetTMKListModel(nodes);
            var stream =  tmkExcelCreator.GetTMKReestrXLSX(items);
            var file = File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, $"Реестр ТМК от {DateTime.Now:dd.MM.yyyy}.xlsx");
            return file;
        }
   

        private void CheckRightTMKReestr(string smo, string code_mo, StatusTMKRow status)
        {
            if (User.IsInRole("TMKAdmin"))
                return;
            if (User.IsInRole("TMKSMO"))
            {
                if (smo != userInfo.CODE_SMO)
                    throw new ModelException("", "Запись не принадлежит Вашей СМО");
                return;
            }
            if (User.IsInRole("TMKUser"))
            {
                if (code_mo != userInfo.CODE_MO)
                    throw new ModelException("", "Запись не принадлежит Вашей МО");
                return;
            }
            if (User.IsInRole("TMKReader"))
            {
                if (status != StatusTMKRow.Closed)
                    throw new ModelException("", "Запись не доступна для просмотра");
              
            }
        }

        [HttpGet]
        public IActionResult View(int TMK_ID)
        {
            ViewModelTMK model = null;
            try
            {
                model = new ViewModelTMK
                {
                    OPLATA = dbo.OPLATA.ToList(),
                    VID_NHISTORY = dbo.VID_NHISTORY.OrderBy(x => x.ORD).ToList()
                };
                return PartialView("_View", model);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в View:{ex.Message}:{ex.StackTrace}", LogType.Error);
                ModelState.AddModelError("", "Внутренняя ошибка сервиса");
                return PartialView("_View", model);
            }
        }
        /// <summary>
        /// Удалить позицию в реестре
        /// </summary>
        /// <param name="tmkId"></param>
        /// <returns></returns>
        [Authorize(Roles = "TMKUser")]
        [HttpPost]
        public async Task<CustomJsonResult> DeleteTmkReestr(params int[] TMK_ID)
        {
            try
            {
                var Error = new List<string>();
                foreach (var ID in TMK_ID)
                {
                    var delItem = dbo.TMKReestr.FirstOrDefault(x => x.TMK_ID == ID);
                    var isERR = false;
                    if (delItem == null)
                    {
                        Error.Add($"Не удалось найти запись №{ID}");
                        isERR = true;
                    }
                    else
                    {
                        if (delItem.CODE_MO != userInfo.CODE_MO)
                        {
                            Error.Add($"Запись №{delItem.TMK_ID} не принадлежит Вашей МО");
                            isERR = true;
                        }
                        if (delItem.STATUS != StatusTMKRow.Open)
                        {
                            Error.Add($"Запись {delItem.TMK_ID} не подлежит редактированию");
                            isERR = true;
                        }
                        if (delItem.Expertizes.Count != 0)
                        {
                            Error.Add($"Запись {delItem.TMK_ID} содержит экспертизы, удаление не возможно");
                            isERR = true;
                        }

                        if (!isERR)
                        {
                            dbo.TMKReestr.Remove(delItem);
                        } 
                    }
                }
                await dbo.SaveChangesAsync();
                return Error.Count == 0 ? CustomJsonResult.Create(null) : CustomJsonResult.Create(string.Join(Environment.NewLine, Error), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в DeleteTmkReestr:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);

            }
            
        }
        /// <summary>
        /// Изменить статус с закрыть/открыть
        /// </summary>
        /// <param name="TMK_ID"></param>
        /// <returns></returns>
        ///
        [Authorize(Roles = "TMKAdmin")]
        [HttpPost]
        public async Task<CustomJsonResult> ChangeTmkReestrStatus([FromBody]int[] TMK_ID)
        {
            try
            {
                var Error = new List<string>();
                foreach (var ID in TMK_ID)
                {
                    var delItem = dbo.TMKReestr.FirstOrDefault(x => x.TMK_ID == ID);
                    if (delItem == null)
                    {
                        Error.Add($"Не удалось найти запись №{ID}");
                    }
                    else
                    {
                        delItem.STATUS = delItem.STATUS == StatusTMKRow.Open ? StatusTMKRow.Closed : StatusTMKRow.Open;
                    }
                }
                await dbo.SaveChangesAsync();
                return Error.Count == 0 ? CustomJsonResult.Create(null) : CustomJsonResult.Create(string.Join(Environment.NewLine, Error), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в ChangeTmkReestrStatus:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> GetTmkReestr(int TMK_ID)
        {
            try
            {
                var item =await GetTMKItem(TMK_ID);
                if (item == null)
                    throw new ModelException("", "Сервис не вернул запись");
                return CustomJsonResult.Create(item);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в GetTmkReestr:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }


     
        private async Task<TMKItem> GetTMKItem(int TMK_ID)
        {
            var query = from tmk in
                    dbo.TMKReestr
                        .Include(x => x.N_VID_NHISTORY)
                        .Include(x => x.CODE_MO_NAME)
                        .Include(x => x.N_OPLATA)
                        .Include(x => x.NMIC_NAME)
                        .Include(x => x.TMIS_NAME)
                        .Include(x => x.SMO_NAME)
                        join prof in dbo.V002 on tmk.PROFIL equals prof.IDPR
                where tmk.TMK_ID == TMK_ID && tmk.DATE_B >= prof.DATEBEG && tmk.DATE_B  <= (prof.DATEEND ?? DateTime.Now)
                        select new TMKItem
            {
             TMK_ID = tmk.TMK_ID,
             NMIC = tmk.NMIC,
             TMIS = tmk.TMIS,
             PROFIL = tmk.PROFIL,
             DATE_TMK = tmk.DATE_TMK,
             DATE_PROTOKOL = tmk.DATE_PROTOKOL,
             DATE_QUERY = tmk.DATE_QUERY,
             DATE_B = tmk.DATE_B,
             NHISTORY = tmk.NHISTORY,
             VID_NHISTORY = tmk.VID_NHISTORY,
             VID_NHISTORY_NAM = tmk.N_VID_NHISTORY.VID_NHISTORY,
             DATE_INVITE = tmk.DATE_INVITE,
             ISNOTSMO = tmk.ISNOTSMO,
             ENP = tmk.ENP,
             FAM = tmk.FAM,
             IM = tmk.IM,
             OT = tmk.OT,
             DR = tmk.DR,
             NOVOR = tmk.NOVOR,
             FAM_P = tmk.FAM_P,
             IM_P = tmk.IM_P,
             OT_P = tmk.OT_P,
             DR_P = tmk.DR_P,
             CODE_MO = tmk.CODE_MO,
             NAM_MOK = tmk.CODE_MO_NAME == null? "" : tmk.CODE_MO_NAME.NAM_MOK,
             STATUS = tmk.STATUS,
             STATUS_COM = tmk.STATUS_COM,
             SMO = tmk.SMO,
             SMO_NAM = tmk.SMO == "75" ? "ТФОМС Забайкальского края" : (tmk.SMO_NAME != null ? tmk.SMO_NAME.NAM_SMOK : tmk.SMO),
             OPLATA = tmk.OPLATA,
             OPLATA_NAM = tmk.N_OPLATA.OPLATA,
             SMO_COM = tmk.SMO_COM,
             PROFIL_NAM = prof.PRNAME,
             NMIC_NAM = tmk.NMIC_NAME.NMIC_NAME,
             TMS_NAM = tmk.TMIS_NAME.TMS_NAME
            };
            var items = await query.ToListAsync();
            if (items.Count > 1)
                throw new Exception($"Ошибка получения записи: На ID = {TMK_ID} вернулось 2 или более записи");
            var item = items.FirstOrDefault();
            if (item != null)
                CheckRightTMKReestr(item.SMO, item.CODE_MO, item.STATUS);
            return item;
        }

        private EditTMKReestrModel GetEditTmkReestrModel(DateTime? dt)
        {
            var model = new EditTMKReestrModel {NMIC = dbo.NMIC.ToList(), TMIS = dbo.TMIS.ToList(), NMIC_VID_NHISTORY = dbo.VID_NHISTORY.OrderBy(x => x.ORD).ToList(), V002 = dbo.V002.Where(x => !x.DATEEND.HasValue).ToList()};
            if (!dt.HasValue || dt > DateTime.Now.Date)
            {
                model.V002 = dbo.V002.Where(x => !x.DATEEND.HasValue).ToList();
            }
            else
            {
                model.V002 = dbo.V002.Where(x => dt.Value >= x.DATEBEG && dt.Value <= (x.DATEEND ?? DateTime.Now)).ToList();
            }
            return model;

        }

        /// <summary>
        /// Редактировать реестр или создать новый GET
        /// </summary>
        /// <param name="TMK_ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "TMKUser")]
        public async Task<IActionResult> EditTmkReestr(int? TMK_ID = null)
        {
            DateTime? dt = null;
            if (TMK_ID != null)
                dt = (await dbo.TMKReestr.FirstOrDefaultAsync(x => x.TMK_ID == TMK_ID.Value)).DATE_B;
            return PartialView(GetEditTmkReestrModel(dt));
        }

        /// <summary>
        /// Редактировать реестр или создать новый POST
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "TMKUser")]
        public async Task<ActionResult> EditTmkReestr([FromBody]TMKItem item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TMKReestr editItem;
                    if (item.TMK_ID != 0)
                    {
                        editItem = await dbo.TMKReestr.FirstOrDefaultAsync(x => x.TMK_ID == item.TMK_ID);
                    }
                    else
                    {
                        editItem = new TMKReestr { CODE_MO = userInfo.CODE_MO, STATUS = StatusTMKRow.Open };
                        dbo.TMKReestr.Add(editItem);
                    }

                    if (editItem == null)
                    {
                        ModelState.AddModelError("", "Не удалось найти случай");
                    }
                    else
                    {
                        if (editItem.CODE_MO != userInfo.CODE_MO)
                        {
                            ModelState.AddModelError("", "Запись не принадлежит Вашей МО");
                        }
                        if (editItem.STATUS == StatusTMKRow.Closed)
                        {
                            ModelState.AddModelError("", "Запись не подлежит редактированию");
                        }
                    }
                    if (ModelState.IsValid && editItem!=null)
                    {
                        editItem.DATE_B = item.DATE_B.Value;
                        editItem.DATE_PROTOKOL = item.DATE_PROTOKOL.Value;
                        editItem.DATE_QUERY = item.DATE_QUERY.Value;
                        editItem.DATE_TMK = item.DATE_TMK;
                        editItem.DR = item.DR.Value;
                        editItem.ENP = item.ENP?.ToUpper();
                        editItem.FAM = item.FAM?.ToUpper();
                        editItem.IM = item.IM?.ToUpper();
                        editItem.OT = item.OT?.ToUpper();
                        editItem.NHISTORY = item.NHISTORY?.ToUpper();
                        editItem.NMIC = item.NMIC.Value;
                        editItem.PROFIL = item.PROFIL.Value;
                        editItem.TMIS = item.TMIS.Value;
                        editItem.USER_ID = userInfo.USER_ID;
                        editItem.NOVOR = item.NOVOR;
                        if (!editItem.NOVOR)
                        {
                            editItem.FAM_P = editItem.IM_P = editItem.OT_P = "";
                            editItem.DR_P = null;
                        }
                        else
                        {
                            editItem.FAM_P = item.FAM_P?.ToUpper();
                            editItem.IM_P = item.IM_P?.ToUpper();
                            editItem.OT_P = item.OT_P?.ToUpper();
                        }
                        editItem.VID_NHISTORY = item.VID_NHISTORY.Value;
                        editItem.ISNOTSMO = item.ISNOTSMO;
                        await dbo.SaveChangesAsync();
                        return CustomJsonResult.Create(null);
                    }
                }
                var err = ModelState.GetErrors();
                return CustomJsonResult.Create(err, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditTmkReestr)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [Authorize(Roles = "TMKAdmin")]
        [HttpPost]
        public async Task<ActionResult> SetAsMTR(int TMK_ID)
        {
            try
            {
                var item = await dbo.TMKReestr.FirstOrDefaultAsync(x => x.TMK_ID == TMK_ID);
                if (item == null) return CustomJsonResult.Create("Не удалось найти запись", false);
                item.SMO = "75";
                item.STATUS = StatusTMKRow.Closed;
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(SetAsMTR)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        [Authorize(Roles = "TMKAdmin, TMKSMO")]
        [HttpGet]
        public async Task<IActionResult> EditExpertize()
        {
            var model = new AddMEE_EKMPModel();
            try
            {
                model.NMIC_CELL =await dbo.NMIC_CELL.ToListAsync();
                model.NMIC_FULL = await dbo.NMIC_FULL.ToListAsync();
                model.EXPERTS = await dbo.EXPERTS.OrderBy(x => x.N_EXPERT).ToListAsync();
                model.F014 = await dbo.F014.OrderBy(x => x.OSN).ToListAsync();
                return PartialView(model);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditExpertize)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                ModelState.AddModelError("", "Внутренняя ошибка сервиса");
                return PartialView(model);
            }
        }
        [HttpGet]
        public async Task<CustomJsonResult> GetSPR()
        {
            try
            {
                var F014 = await dbo.F014.OrderBy(x => x.OSN).ToListAsync();
                return CustomJsonResult.Create(new { F014 });
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetSPR)}: {ex.Message}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        private async Task<CustomJsonResult> GetExpertizeJSON(int? TMK_ID, int? EXPERTIZE_ID)
        {
            var list = new List<object>();
            var exps = new List<TMKReestRExpertize>();

            if (EXPERTIZE_ID.HasValue)
            {
                var exp =await dbo.Expertizes.FirstOrDefaultAsync(x=>x.EXPERTIZE_ID == EXPERTIZE_ID);
                if (exp != null)
                    exps.Add(exp);
                else
                {
                    throw new ModelException("",$"Не удалось найти экспертизу EXPERTIZE_ID={EXPERTIZE_ID}");
                }
            }
            
            if (TMK_ID.HasValue)
            {
                exps = await dbo.Expertizes.Where(x => x.TMK_ID == TMK_ID).OrderByDescending(x => x.EXPERTIZE_ID).ToListAsync();
            }

            var query = from  e in dbo.Expertizes
                join osn in dbo.OSN on e.EXPERTIZE_ID equals osn.EXPERTIZE_ID
                join f14 in dbo.F014 on osn.S_OSN equals f14.KOD
                where (e.TMK_ID == TMK_ID || !TMK_ID.HasValue) && (e.EXPERTIZE_ID == EXPERTIZE_ID || !EXPERTIZE_ID.HasValue) && 
                    (e.DATEACT) >= f14.DATEBEG && (e.DATEACT) <= (f14.DATEEND ?? DateTime.Now)
                      select new
                      {
                          osn.EXPERTIZE_ID,
                          osn.S_COM,
                          osn.S_OSN,
                          osn.S_FINE,
                          osn.S_SUM,
                          S_OSN_NAME = f14.FullName ?? $"Нет значения из справочника F014, код = {osn.S_OSN}"
                      };

            var OSN = await query.ToListAsync();

            foreach (var exp in exps)
            {
                list.Add(
                     new
                     {
                         exp.NUMACT,
                         exp.DATEACT,
                         exp.FIO,
                         exp.FULL,
                         exp.ISCOROLLARY,
                         exp.ISNOTRECOMMEND,
                         exp.CELL,
                         exp.ISOSN,
                         exp.CELL_NAME?.CELL_NAME,
                         exp.FULL_NAME?.FULL_NAME,
                         exp.ISRECOMMENDMEDDOC,
                         exp.NOTPERFORM,
                         exp.N_EXP,
                         exp.S_TIP,
                         exp.EXPERTIZE_ID,
                         exp.TMK_ID,
                         OSN = OSN.Where(x=>x.EXPERTIZE_ID == exp.EXPERTIZE_ID)
                     }
                     );
            }
            return CustomJsonResult.Create(list);
        }

        [HttpGet]
        public async Task<CustomJsonResult> GetExpertize(int? TMK_ID, int? EXPERTIZE_ID)
        {
            try
            {
                return await GetExpertizeJSON(TMK_ID, EXPERTIZE_ID);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditExpertize)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
           
        }
        [Authorize(Roles = "TMKAdmin, TMKSMO")]
        [HttpPost]
        public async Task<CustomJsonResult> EditExpertize([FromBody] ExpertizeModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var r =await dbo.TMKReestr.FirstOrDefaultAsync(x => x.TMK_ID == item.TMK_ID && (x.SMO == userInfo.CODE_SMO || x.ISNOTSMO && userInfo.CODE_SMO == "75"));
                    if (r == null)
                        throw new ModelException("", $"Запись ТМК не найдена для СМО={userInfo.CODE_SMO}");

                    if (!string.IsNullOrEmpty(item.N_EXP))
                    {
                        var expert = await dbo.EXPERTS.FirstOrDefaultAsync(x => x.N_EXPERT == item.N_EXP);
                        if (expert != null)
                        {
                            item.FIO = expert.FIO;
                        }
                    }
                    foreach (var ep in item.Validate(r.DATE_PROTOKOL))
                    {
                        ModelState.AddModelError("", ep);
                    }

                    if (ModelState.IsValid)
                    {
                        if (item.EXPERTIZE_ID == -1)
                        {
                            var exp = new TMKReestRExpertize {USER_ID = userInfo.USER_ID, TMK_ID = r.TMK_ID};
                            item.CopyTo(exp);
                            dbo.Expertizes.Add(exp);
                            if (exp.S_TIP == ExpertTip.MEK)
                            {
                                r.OPLATA = exp.OSN.Count == 0 ? 2 : 1;
                            }
                        }
                        else
                        {
                            var exp = await dbo.Expertizes.FirstOrDefaultAsync(x=>x.EXPERTIZE_ID == item.EXPERTIZE_ID);
                            var items = exp.OSN.ToList();
                            for (var i = exp.OSN.Count; i < items.Count; i++)
                            {
                                dbo.OSN.Remove(items[i]);
                            }
                            item.CopyTo(exp);
                        }
                        await dbo.SaveChangesAsync();
                        return CustomJsonResult.Create(null);
                    }
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(EditExpertize)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }




        [Authorize(Roles = "TMKAdmin, TMKSMO")]
        [HttpPost]
        public async Task<CustomJsonResult> DeleteExpertize(int EXPERTIZE_ID)
        {
            try
            {
                var exp = await dbo.Expertizes.FirstOrDefaultAsync(x => x.EXPERTIZE_ID == EXPERTIZE_ID);
                if (exp == null)
                    throw new ModelException("", "Экспертиза не найдена");
                if (exp.TMKReestr.SMO != userInfo.CODE_SMO) 
                    throw new ModelException("", "Экспертиза принадлежит другой СМО");

                dbo.Expertizes.Remove(exp);
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(DeleteExpertize)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        
        [HttpPost]
        public async Task<CustomJsonResult> SetOPLATAandVID_NHISTORY([FromBody] OPLATAandVID_NHISTORYModel item)
        {
            try
            {
               
                var tmk = await dbo.TMKReestr.FirstOrDefaultAsync(x => x.TMK_ID == item.TMK_ID);
              
                if (tmk == null)
                    throw new ModelException("", "Не удалось найти запись");

                tmk.VID_NHISTORY = item.VID_NHISTORY;
                tmk.OPLATA = item.OPLATA;
                tmk.SMO_COM = item.SMO_COM;
                await dbo.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (ModelException ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(SetOPLATAandVID_NHISTORY)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }



        #region Отчет
        [Authorize(Roles = "TMKAdmin, TMKSMO")]
        [HttpGet]
        public ActionResult Report()
        {
            return View(new TMKReportModel { NMIC_VID_NHISTORY = dbo.VID_NHISTORY.OrderBy(x => x.ORD).ToList() });
        }
        [Authorize(Roles = "TMKAdmin, TMKSMO")]
        [HttpGet]
        public IActionResult GetReport(DateTime Date1, DateTime Date2, bool IsSMO, bool isMO, bool isNMIC, int[] VID_NHISTORY)
        {
            var model = new TMKReportTableModel();
            try
            {
                var smo = "";
              
                if (User.IsInRole("TMKSMO"))
                {
                    smo = userInfo.CODE_SMO;
                }
                if (User.IsInRole("TMKAdmin"))
                {
                    smo = "%";
                }
                model.Report.AddRange(dbo.GetReport(Date1, Date2, isMO, IsSMO, isNMIC, smo, VID_NHISTORY));
                model.Report2.AddRange(dbo.GetReport2(Date1, Date2, isMO, IsSMO, isNMIC, smo, VID_NHISTORY));
                HttpContext.Session.Set("GetReportResult", model);
                HttpContext.Session.Set("Date1TMK", Date1);
                HttpContext.Session.Set("Date2TMK", Date2);
                return PartialView("_ReportTable", model);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetReport)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                ModelState.AddModelError("", "Внутренняя ошибка сервиса");
                return PartialView("_ReportTable", model);
            }
        }
        [HttpGet]
        public IActionResult GetReportXLSXFile()
        {
            try
            {
                var Items = HttpContext.Session.Get<TMKReportTableModel>("GetReportResult");
                var Date1TMK = HttpContext.Session.Get<DateTime>("Date1TMK");
                var Date2TMK = HttpContext.Session.Get<DateTime>("Date2TMK");
                var file = File(tmkExcelCreator.GetReportXLS(Items.Report, Items.Report2), System.Net.Mime.MediaTypeNames.Application.Octet, $"Отчет НМИЦ c {Date1TMK.Date:dd.MM.yyyy} по {Date2TMK.Date:dd.MM.yyyy}.xlsx");
                return file;
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка в {nameof(GetReportXLSXFile)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
                return new StatusCodeResult(500);
            }
        }

        #endregion
        #region  Справочник контактов
        [Authorize(Roles = "TMKAdmin,TMKUser")]
        public async Task<IActionResult> ContactInfo()
        {
            var info = new List<CONTACT_INFO>();
            try
            {
                if (User.IsInRole("TMKAdmin"))
                {
                    info = await dbo.CONTACT_INFO.Include(x=>x.CODE_MO_NAME).OrderBy(x => x.CODE_MO).ThenByDescending(x => x.ID_CONTACT_INFO).ToListAsync();
                }
                else
                {
                    if (User.IsInRole("TMKUser"))
                    {
                        info = await dbo.CONTACT_INFO.Include(x => x.CODE_MO_NAME).Where(x => x.CODE_MO == userInfo.CODE_MO).OrderBy(x => x.CODE_MO).ThenByDescending(x => x.ID_CONTACT_INFO).ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("","Внутренняя ошибка сервиса");
                logger?.AddLog($"Ошибка в {nameof(GetReportXLSXFile)}:{ex.Message}:{ex.StackTrace}", LogType.Error);
            }
            return View("SPR/ContactInfo", info);
        }
        [Authorize(Roles = "TMKAdmin,TMKUser")]
        [HttpGet]
        public async Task<IActionResult> EditCONTACT_INFO(int ID_CONTACT_INFO = 0)
        {
            return View("SPR/EditCONTACT_INFO", await GetEditCONTACT_INFOModel(ID_CONTACT_INFO));
        }
      

        private async  Task<EditCONTACT_INFOModel> GetEditCONTACT_INFOModel(int ID_CONTACT_INFO)
        {
            var item = new CONTACT_INFOModel();
           
            if (User.IsInRole("TMKAdmin"))
            {
                if(ID_CONTACT_INFO != 0)
                    item = new CONTACT_INFOModel(await dbo.CONTACT_INFO.Include(x => x.CODE_MO_NAME).FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == ID_CONTACT_INFO));
            }
            else
            {
                if (User.IsInRole("TMKUser"))
                {
                    if (ID_CONTACT_INFO != 0)
                        item = new CONTACT_INFOModel(await dbo.CONTACT_INFO.FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == ID_CONTACT_INFO && x.CODE_MO == userInfo.CODE_MO));
                }
            }
            return await GetEditCONTACT_INFOModel(item);
        }
        private async Task<EditCONTACT_INFOModel> GetEditCONTACT_INFOModel(CONTACT_INFOModel model)
        {
            var code_mo = new List<CODE_MO>();
            if (User.IsInRole("TMKAdmin"))
            {
                code_mo = await dbo.CODE_MO.Where(x => !x.D_END.HasValue && x.MCOD.StartsWith("75")).OrderBy(x => x.MCOD).ToListAsync();
            }
            else
            {
                if (User.IsInRole("TMKUser"))
                {
                    code_mo = await dbo.CODE_MO.Where(x => !x.D_END.HasValue && x.MCOD == userInfo.CODE_MO).OrderBy(x => x.MCOD).ToListAsync();
                }
            }
            return new EditCONTACT_INFOModel { CODE_MO = code_mo, INFO = model };
        }
        [Authorize(Roles = "TMKAdmin,TMKUser")]
        [HttpPost]
        public async Task<IActionResult> EditCONTACT_INFO(EditCONTACT_INFOModel model)
        {
            var item = model.INFO;
            try
            {
                if (ModelState.IsValid)
                {
                    CONTACT_INFO editItem = null;
                    if (item.ID_CONTACT_INFO != 0)
                    {
                        if (User.IsInRole("TMKAdmin"))
                        {
                            editItem = await dbo.CONTACT_INFO.FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == item.ID_CONTACT_INFO);
                        }

                        if (User.IsInRole("TMKUser"))
                        {
                            editItem = await dbo.CONTACT_INFO.FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == item.ID_CONTACT_INFO && x.CODE_MO == userInfo.CODE_MO);
                        }

                        if (editItem == null)
                        {
                            throw new ModelException("", "Запись не найдена в реестре");
                        }

                        editItem.CODE_MO = item.CODE_MO;
                        editItem.FAM = item.FAM;
                        editItem.OT = item.OT;
                        editItem.IM = item.IM;
                        editItem.TEL = item.TEL;
                    }
                    else
                    {
                        editItem = new CONTACT_INFO();
                        dbo.CONTACT_INFO.Add(editItem);
                    }

                    item.To(editItem);
                    await dbo.SaveChangesAsync();
                    return RedirectToAction("ContactInfo");
                }
                return View("SPR/EditCONTACT_INFO", await GetEditCONTACT_INFOModel(item));
            }
            catch (ModelException e)
            {
                ModelState.AddModelError("", e.Message);
                return View("SPR/EditCONTACT_INFO", await GetEditCONTACT_INFOModel(item));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Внутренняя ошибка сервиса");
                return View("SPR/EditCONTACT_INFO", await GetEditCONTACT_INFOModel(item));
            }
        }

        [Authorize(Roles = "TMKAdmin,TMKUser")]
        [HttpPost]
        public async Task<IActionResult> DeleteCONTACT_INFO(int ID_CONTACT_INFO)
        {
            CONTACT_INFO t = null;
            if (User.IsInRole("TMKAdmin"))
            {
                t = await dbo.CONTACT_INFO.FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == ID_CONTACT_INFO);
            }
            else
            {
                if (User.IsInRole("TMKUser"))
                {
                    t = await dbo.CONTACT_INFO.FirstOrDefaultAsync(x => x.ID_CONTACT_INFO == ID_CONTACT_INFO && x.CODE_MO == userInfo.CODE_MO);
                }
            }
           
            if (t != null)
            {
                dbo.CONTACT_INFO.Remove(t);
                await dbo.SaveChangesAsync();
            }
            return RedirectToAction("ContactInfo");
        }

        #endregion

       
    }
}
