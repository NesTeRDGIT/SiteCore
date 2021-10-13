using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ServiceLoaderMedpomData;
using SiteCore.Class;
using  SiteCore.Data;
using SiteCore.Models;


namespace SiteCore.Controllers
{
    [Authorize(Roles = "CS, Admin")]
    public class IdentificationController : Controller
    {
        private CSOracleSet csOracleSet { get; }
        private WCFIdentiScaner wcfIdentiScaner { get; }
        private ILogger logger { get; }

      

       
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

        public IdentificationController(CSOracleSet csOracleSet, WCFIdentiScaner wcfIdentiScaner, ILogger logger, UserInfoHelper userInfoHelper)
        {
            this.csOracleSet = csOracleSet;
            this.wcfIdentiScaner = wcfIdentiScaner;
            this.logger = logger;
            this.userInfoHelper = userInfoHelper;
        }


        #region Основное

        // GET: Identification
        public IActionResult CreateCSList()
        {
            return View();
        }

        [HttpGet]
        public CustomJsonResult GetServiceState()
        {
            return CustomJsonResult.Create(wcfIdentiScaner.IsEnabled);
        }
        [HttpGet]
        public IActionResult GetInstruction()
        {
            return PartialView("_InstructionPartical");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetServiceLog()
        {
            return PartialView("_LogServicePartical", wcfIdentiScaner.GetLog());
        }
        #endregion
        #region Работа со списками

        [HttpGet]
        public IActionResult NewCSList()
        {
            return PartialView("_NewCSListPartical", new NewCSListViewModel());
        }




        [HttpPost]
        public async Task<CustomJsonResult> NewCSList(NewCSListViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CSItem = new CS_LIST();
                    CSItem.DATE_CREATE = DateTime.Now;
                    CSItem.CODE_MO = userInfo.CODE_MO;
                    CSItem.CAPTION = string.IsNullOrEmpty(model.Caption) ? $"Запрос {CSItem.CODE_MO} от {CSItem.DATE_CREATE:dd.MM.yyyy HH:mm}" : model.Caption;
                    csOracleSet.CS_LIST.Add(CSItem);
                    await csOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }
                return CustomJsonResult.Create(await this.RenderViewAsync("_errorList", ModelToList(ModelState)));
            }
            catch (Exception ex)
            {
                logger.AddLog($"NewCSList:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError();
            }

        }
        [HttpGet]
        public async Task<CustomJsonResult> GetCSList(int Page, int CountOnPage)
        {
            try
            {
                var listq = getCsListsQ();
                var count = listq.Count();
                var list = (await listq.OrderByDescending(x => x.CS_LIST_ID).Skip((Page - 1) * CountOnPage).Take(CountOnPage).ToListAsync()).Select(x => new { x.CS_LIST_ID, x.CAPTION, x.CODE_MO, x.COMM, x.DATE_CREATE, STATUS = x.STATUS.ToString(), STATUS_RUS = $"{x.STATUS.ToRusStr()}{(!string.IsNullOrEmpty(x.COMM) ? $"({x.COMM})" : "")}" });
                return CustomJsonResult.Create(new { count, items = list });
            }
            catch (Exception ex)
            {
                logger.AddLog($"GetCSList:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpGet]
        public async Task<CustomJsonResult> GetCSListByID(List<int> CS_LIST_ID)
        {
            try
            {
                var listq = getCsList(CS_LIST_ID);
                var list = (await listq.ToListAsync()).Select(x => new { x.CS_LIST_ID, x.CAPTION, x.CODE_MO, x.COMM, x.DATE_CREATE, STATUS = x.STATUS.ToString(), STATUS_RUS = x.STATUS.ToRusStr() });
                return CustomJsonResult.Create(new { items = list });
            }
            catch (Exception ex)
            {
                logger.AddLog($"GetCSListByID:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> DeleteCSList([FromBody] int[] CS_LIST_ID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var val in CS_LIST_ID)
                    {
                        var list = await getCsList(val);
                        if (list != null)
                        {
                            var res = list.CS_LIST_IN.SelectMany(x => x.CS_LIST_IN_RESULT).ToList();
                            csOracleSet.CS_LIST_IN_RESULT_SMO.RemoveRange(res.SelectMany(x => x.CS_LIST_IN_RESULT_SMO));
                            csOracleSet.CS_LIST_IN_RESULT.RemoveRange(res);
                            csOracleSet.CS_LIST_IN.RemoveRange(list.CS_LIST_IN);
                            csOracleSet.CS_LIST.Remove(list);
                        }
                    }
                    await csOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }
                return CustomJsonResult.Create(await this.RenderViewAsync("_errorList", ModelToList(ModelState)), false);
            }
            catch (Exception ex)
            {
                logger.AddLog($"DeleteCSList:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> SendCSList([FromBody] int[] CS_LIST_ID)
        {
            try
            {
                foreach (var val in CS_LIST_ID)
                {
                    var list = await getCsList(val);
                    if (list is { STATUS: StatusCS_LIST.New } && list.CS_LIST_IN.Count != 0)
                    {
                        list.STATUS = StatusCS_LIST.OnSend;
                    }
                }
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (Exception ex)
            {
                logger.AddLog($"SendCSList:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> OpenCSList([FromBody] int[] CS_LIST_ID)
        {
            try
            {
                foreach (var val in CS_LIST_ID)
                {
                    var list = await getCsList(val);
                    if (list != null && list.STATUS.In(StatusCS_LIST.Answer, StatusCS_LIST.Error))
                    {
                        list.STATUS = StatusCS_LIST.New;
                        var res = list.CS_LIST_IN.SelectMany(x => x.CS_LIST_IN_RESULT).ToList();
                        csOracleSet.CS_LIST_IN_RESULT_SMO.RemoveRange(res.SelectMany(x => x.CS_LIST_IN_RESULT_SMO));
                        csOracleSet.CS_LIST_IN_RESULT.RemoveRange(res);
                        foreach (var list_in in list.CS_LIST_IN)
                        {
                            list_in.STATUS = null;
                        }
                    }
                }
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (Exception ex)
            {
                logger.AddLog($"OpenCSList:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        #endregion
        #region Работа с элементами списков

        async Task<NewCSItemViewModel> CreateModel(int CS_LIST_ID, NewCSItemModel item)
        {
            return new() {F011 = await csOracleSet.F011.OrderBy(x => x.IDDOC == "14" ? "-1" : x.IDDOC).ToListAsync(), CS_LIST_ID = CS_LIST_ID, ITEM = item};
        }

        [HttpGet]
        public async Task<CustomJsonResult> NewCSItem(int CS_LIST_ID, int? CS_LIST_IN_ID)
        {
            try
            {
                NewCSItemModel item = null;
                if (CS_LIST_IN_ID.HasValue)
                    item = new NewCSItemModel(await getCsItem(CS_LIST_IN_ID.Value));
                return CustomJsonResult.Create(await this.RenderViewAsync("_NewCSItemPartical", await CreateModel(CS_LIST_ID, item), true));
            }
            catch (Exception ex)
            {
                logger.AddLog($"NewCSItem:{ex.Message}||{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> NewCSItem([Bind(Prefix = "ITEM")] NewCSItemModel model)
        {
            try
            {
                model.FAM = model.FAM?.ToUpper().Trim();
                model.IM = model.IM?.ToUpper().Trim();
                model.OT = model.OT?.ToUpper().Trim();
                model.DOC_NUM = model.DOC_NUM?.ToUpper().Trim();
                model.DOC_SER = model.DOC_SER?.ToUpper().Trim();
                model.SNILS = model.SNILS?.ToUpper().Trim();
                model.SPOLIS = model.SPOLIS?.ToUpper().Trim();
                model.NPOLIS = model.NPOLIS?.ToUpper().Trim();

                foreach (var err in model.IsValid)
                {
                    ModelState.AddModelError("", err);
                }

                if (ModelState.IsValid)
                {
                    var list = await getCsList(model.CS_LIST_ID);
                    if (list == null)
                    {
                        throw new ModelException("", "Список для добавления не найден");
                    }

                    if (list.STATUS != StatusCS_LIST.New)
                    {
                        throw new ModelException("", "Невозможно добавить\\изменить данные в список со статусом отличным от \"Новый\"");
                    }

                    if (!model.CS_LIST_IN_ID.HasValue)
                    {
                        list.CS_LIST_IN.Add(model.ToCS_LIST_IN());
                    }
                    else
                    {
                        var itemBD = await getCsItem(model.CS_LIST_IN_ID.Value);
                        itemBD.CopyFrom(model.ToCS_LIST_IN());
                    }
                    await csOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(null);
                }
                throw new ModelException(null, null);
            }
            catch (ModelException ex)
            {
                if(ex.Key!=null)
                    ModelState.AddModelError(ex.Key, ex.Message);
                return CustomJsonResult.Create(await this.RenderViewAsync("_NewCSItemPartical", await CreateModel(model.CS_LIST_ID, model), true), false);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка NewCSItem: {ex.FullError()}:{ex.StackTrace}", LogType.Error);
                ModelState.AddModelError("", "Внутренняя ошибка сервиса");
                return CustomJsonResult.Create(await this.RenderViewAsync("_NewCSItemPartical", await CreateModel(model.CS_LIST_ID, model), true), false);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> DeleteCSItem([FromBody] int[] CS_LIST_IN_ID)
        {
            try
            {
                foreach (var val in CS_LIST_IN_ID)
                {
                    var item = await getCsItem(val);
                    if (item != null)
                    {
                        if (item.CS_LIST.STATUS != StatusCS_LIST.New)
                        {
                            throw new Exception("Невозможно удалить данные из списка со статусом отличным от \"Новый\"");
                        }
                        if (item.CS_LIST_IN_RESULT != null)
                        {
                            csOracleSet.CS_LIST_IN_RESULT_SMO.RemoveRange(item.CS_LIST_IN_RESULT.SelectMany(x => x.CS_LIST_IN_RESULT_SMO));
                            csOracleSet.CS_LIST_IN_RESULT.RemoveRange(item.CS_LIST_IN_RESULT);
                        }
                        csOracleSet.CS_LIST_IN.Remove(item);
                    }
                }
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(null);
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка DeleteCSItem: {ex.FullError()}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }
        }
        [HttpGet]
        public async Task<CustomJsonResult> GetCSListIN(int CS_LIST_ID, int Page, int CountOnPage)
        {
            try
            {
                var listq = getCsList_INQ(CS_LIST_ID);
                var count = listq.Count();
                var list = (await listq.OrderByDescending(x => x.CS_LIST_IN_ID).Skip((Page - 1) * CountOnPage).Take(CountOnPage).ToListAsync()).Select(x => new
                {
                    x.FAM,
                    x.IM,
                    x.OT,
                    x.W,
                    x.DR,
                    x.POLIS,
                    x.DOC,
                    x.STATUS,
                    STATUS_RUS = $"{x.STATUS_RUS}{(!string.IsNullOrEmpty(x.COMM) && x.STATUS == false ? $"({x.COMM})" : "")}",
                    CURRENT_SMO = (x.CurrentSMO != null ? $"{csOracleSet.FindNameSMO(x.CurrentSMO.TYPE_SMO, x.CurrentSMO.SMO, x.CurrentSMO.SMO_OK)} c {x.CurrentSMO.DATE_B.ToStr("dd.MM.yyyy")} {(x.CurrentSMO.DATE_E.HasValue ? $" по {x.CurrentSMO.DATE_E.ToStr("dd.MM.yyyy")}" : "")}" : ""),
                    x.CS_LIST_IN_ID
                }).ToList();
                return CustomJsonResult.Create(new { count, items = list });
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка GetCSListIN: {ex.FullError()}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError();
            }
        }

        [HttpGet]
        public async Task<CustomJsonResult> ViewResult(int CS_LIST_IN_ID)
        {
            try
            {

                var item = await getCsItem(CS_LIST_IN_ID);
                foreach (var smo in item.CS_LIST_IN_RESULT.SelectMany(x => x.CS_LIST_IN_RESULT_SMO))
                {
                    smo.SMO_NAME = await csOracleSet.FindNameSMO(smo.TYPE_SMO, smo.SMO, smo.SMO_OK);
                }
                return CustomJsonResult.Create(await this.RenderViewAsync("_ViewResultPartical", item, true));
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка ViewResult: {ex.FullError()}:{ex.StackTrace}", LogType.Error);
                return await CreateInternalError(false);
            }

        }

        #endregion
        #region Private
        private async Task<CustomJsonResult> CreateInternalError(bool errorList = true)
        {
            return errorList ? CustomJsonResult.Create(await this.RenderViewAsync("_errorList", "Внутренняя ошибка сервиса", true), false) : CustomJsonResult.Create("Внутренняя ошибка сервиса", false);
        }

        private List<string> ModelToList(ModelStateDictionary model)
        {
            return model.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
        }

        private IQueryable<CS_LIST_IN> getCsList_INQ(int CS_LIST_ID)
        {
            return !User.IsInRole("Admin") ? csOracleSet.CS_LIST_IN.Where(x => x.CS_LIST.CS_LIST_ID == CS_LIST_ID && x.CS_LIST.CODE_MO == userInfo.CODE_MO) : csOracleSet.CS_LIST_IN.Where(x => x.CS_LIST_ID == CS_LIST_ID);
        }

        private IQueryable<CS_LIST> getCsListsQ()
        {
            return !User.IsInRole("Admin") ? csOracleSet.CS_LIST.Where(x => x.CODE_MO == userInfo.CODE_MO) : csOracleSet.CS_LIST;
        }
        private async Task<CS_LIST> getCsList(int CS_LIST_ID)
        {
            return !User.IsInRole("Admin") ? await csOracleSet.CS_LIST.FirstOrDefaultAsync(x => x.CODE_MO == userInfo.CODE_MO && x.CS_LIST_ID == CS_LIST_ID) : await csOracleSet.CS_LIST.FirstOrDefaultAsync(x => x.CS_LIST_ID == CS_LIST_ID);
        }
        private async Task<CS_LIST_IN> getCsItem(int CS_LIST_IN_ID)
        {

            return !User.IsInRole("Admin") ? await csOracleSet.CS_LIST_IN.FirstOrDefaultAsync(x => x.CS_LIST.CODE_MO == userInfo.CODE_MO && x.CS_LIST_IN_ID == CS_LIST_IN_ID) : await csOracleSet.CS_LIST_IN.FirstOrDefaultAsync(x => x.CS_LIST_IN_ID == CS_LIST_IN_ID);
        }
        private IQueryable<CS_LIST> getCsList(List<int> CS_LIST_ID)
        {
            return !User.IsInRole("Admin") ? csOracleSet.CS_LIST.Where(x => x.CODE_MO == userInfo.CODE_MO && CS_LIST_ID.Contains(x.CS_LIST_ID)) : csOracleSet.CS_LIST.Where(x => CS_LIST_ID.Contains(x.CS_LIST_ID));
        }
        #endregion


    }

    public class ModelException : Exception
    {
        public string Key { get; }

        public ModelException()
        {
        }

        public ModelException(string key, string message) : base(message)
        {
            Key = key;
        }

        public ModelException(string key, string message, Exception inner) : base(message, inner)
        {
            Key = key;
        }
    }


}
