using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using CRZ_IDENTITIFI;
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

        private IQueryable<CS_LIST_IN> getCsItemSingle(int CS_LIST_IN_ID)
        {
            return !User.IsInRole("Admin") ?  csOracleSet.CS_LIST_IN.Where(x => x.CODE_MO == userInfo.CODE_MO && x.CS_LIST_IN_ID == CS_LIST_IN_ID) :  csOracleSet.CS_LIST_IN.Where(x => x.CS_LIST_IN_ID == CS_LIST_IN_ID);
        }

        private IQueryable<CS_LIST_IN> getCsItemList()
        {

            return !User.IsInRole("Admin") ?  csOracleSet.CS_LIST_IN.Where(x => x.CODE_MO == userInfo.CODE_MO) :  csOracleSet.CS_LIST_IN;
        }

        private CustomJsonResult InternalError(Exception e, bool toList = false)
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
        [HttpGet]
        public async Task<CustomJsonResult> GetCSItemTitleByID(int[] CS_LIST_IN_ID)
        {
            try
            {
                var list = getCsItemList();
                var items = await list.Where(x=> CS_LIST_IN_ID.Contains(x.CS_LIST_IN_ID)).Select(x => new PersonItem
                {
                    CS_LIST_IN_ID = x.CS_LIST_IN_ID,
                    CODE_MO = x.CODE_MO,
                    CURRENT_SMO = (x.CurrentSMO != null ? $"{ csOracleSet.FindNameSMO(x.CurrentSMO.TYPE_SMO, x.CurrentSMO.SMO, x.CurrentSMO.SMO_OK)} c {x.CurrentSMO.DATE_B.ToStr("dd.MM.yyyy")} {(x.CurrentSMO.DATE_E.HasValue ? $" по {x.CurrentSMO.DATE_E.ToStr("dd.MM.yyyy")}" : "")}" : ""),
                    DOC = x.DOC,
                    DR = x.DR,
                    FAM = x.FAM,
                    IM = x.IM,
                    OT = x.OT,
                    POLIS = x.POLIS,
                    STATUS = x.STATUS,
                    STATUS_TEXT = $"{x.STATUS_RUS}{(!string.IsNullOrEmpty(x.COMM) && x.STATUS == false ? $"({x.COMM})" : "")}",
                    STATUS_SEND = x.STATUS_SEND,
                    STATUS_SEND_TEXT = x.STATUS_SEND.ToRusStr(),
                    DATE_CREATE = x.DATE_CREATE,

                }).ToListAsync();
                return CustomJsonResult.Create(items);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet]
        public async  Task<CustomJsonResult> GetCSItemTitle(int first = 0, int rows = 25)
        {
            try
            {
                var list = getCsItemList();
                var totalRecord = await list.CountAsync();
                var items = await list.OrderByDescending(x => x.CS_LIST_IN_ID).Skip(first).Take(rows).Select(x => new PersonItem
                {
                    CS_LIST_IN_ID = x.CS_LIST_IN_ID,
                    CODE_MO = x.CODE_MO,
                    CURRENT_SMO = (x.CurrentSMO != null ? $"{ csOracleSet.FindNameSMO(x.CurrentSMO.TYPE_SMO, x.CurrentSMO.SMO, x.CurrentSMO.SMO_OK)} c {x.CurrentSMO.DATE_B.ToStr("dd.MM.yyyy")} {(x.CurrentSMO.DATE_E.HasValue ? $" по {x.CurrentSMO.DATE_E.ToStr("dd.MM.yyyy")}" : "")}" : ""),
                    DOC = x.DOC,
                    DR = x.DR,
                    FAM = x.FAM,
                    IM = x.IM,
                    OT = x.OT,
                    POLIS = x.POLIS,
                    STATUS = x.STATUS,
                    STATUS_TEXT = $"{x.STATUS_RUS}{(!string.IsNullOrEmpty(x.COMM) && x.STATUS == false ? $"({x.COMM})" : "")}",
                    STATUS_SEND = x.STATUS_SEND,
                    STATUS_SEND_TEXT = x.STATUS_SEND.ToRusStr(),
                    DATE_CREATE = x.DATE_CREATE,

                }).ToListAsync();
                return CustomJsonResult.Create(new TitleResult(){ PersonItems = items, TotalRecord = totalRecord});
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }

        [HttpGet]
        public async Task<CustomJsonResult> GetSPR()
        {
            try
            {
                var SPRmodel = new SPRModel
                {
                    W = new List<SprWItemModel> { new() { ID = 1, NAME = "Мужской" }, new() { ID = 2, NAME = "Женский" } },
                    F011 = await csOracleSet.F011.OrderBy(x => x.IDDOC == "14" ? "-1" : x.IDDOC).Select(x=>new SprF011ItemModel{ID = x.IDDOC, NAME = x.DOCNAME}).ToListAsync(),
                    VPOLIS = ((VPOLIS_VALUES[])Enum.GetValues(typeof(VPOLIS_VALUES))).Select(x => new SprVPOLISItemModel { ID = Convert.ToInt32(x), NAME = x.ToRusStr() }).ToList()
                };
                return CustomJsonResult.Create(SPRmodel);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
        }



        [HttpPost]
        public async Task<CustomJsonResult> AddPerson(PersonItemModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var item = model.ToCS_LIST_IN();
                    item.CODE_MO = userInfo.CODE_MO;
                    item.DATE_CREATE = DateTime.Now;
                   
                    csOracleSet.CS_LIST_IN.Add(item);
                    await csOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);

            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }

        [HttpPost]
        public async Task<CustomJsonResult> UpdatePerson(PersonItemModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!model.CS_LIST_IN_ID.HasValue)
                        throw new ModelException("Не указан идентификатор основного случая");

                    var baseItem = await getCsItemSingle(model.CS_LIST_IN_ID.Value).FirstOrDefaultAsync();
                    if (baseItem == null)
                        throw new ModelException($"Не удалось найти запись №{model.CS_LIST_IN_ID.Value}");
                    if(baseItem.STATUS_SEND!=StatusCS_LIST.New)
                        throw new ModelException($"Не допустимо редактирование записей со статусом отличным от \"Новый\"");

                    model.CopyTo(baseItem);
                    baseItem.CODE_MO = userInfo.CODE_MO;
                    baseItem.DATE_CREATE = DateTime.Now;

                    await csOracleSet.SaveChangesAsync();
                    return CustomJsonResult.Create(true);
                }
                return CustomJsonResult.Create(ModelState.GetErrors(), false);

            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }
        [HttpPost]
        public async Task<CustomJsonResult> RemovePerson(int[] CS_LIST_IN_ID)
        {
            try
            {
                var items = await  getCsItemList().Where(x => CS_LIST_IN_ID.Contains(x.CS_LIST_IN_ID)).ToListAsync();
                csOracleSet.CS_LIST_IN.RemoveRange(items);
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> GetPerson(int CS_LIST_IN_ID)
        {
            try
            {
                var item = await getCsItemSingle(CS_LIST_IN_ID).FirstOrDefaultAsync();
                if (item == null)
                    throw new ModelException($"Не удалось найти запись №{CS_LIST_IN_ID}");
                return CustomJsonResult.Create(new PersonItemModel(item));
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }


        [HttpPost]
        public async Task<CustomJsonResult> SendPerson(int[] CS_LIST_IN_ID)
        {
            try
            {
                var items = await getCsItemList().Where(x=>CS_LIST_IN_ID.Contains(x.CS_LIST_IN_ID)).ToListAsync();
                foreach (var item in items)
                {
                    if (item.STATUS_SEND != StatusCS_LIST.New)
                        throw new ModelException("Не возможно отправить запись со статусом отличным от статуса \"Новый\"");
                    item.STATUS_SEND = StatusCS_LIST.OnSend;
                }
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }


        [HttpPost]
        public async Task<CustomJsonResult> SetNewStatus(int[] CS_LIST_IN_ID)
        {
            try
            {
                var items = await getCsItemList().Include(x=>x.CS_LIST_IN_RESULT).ThenInclude(x=>x.CS_LIST_IN_RESULT_SMO).Where(x => CS_LIST_IN_ID.Contains(x.CS_LIST_IN_ID)).ToListAsync();
                foreach (var item in items)
                {
                    item.STATUS_SEND = StatusCS_LIST.New;
                    item.STATUS = null;
                    csOracleSet.CS_LIST_IN_RESULT_SMO.RemoveRange(item.CS_LIST_IN_RESULT.SelectMany(x => x.CS_LIST_IN_RESULT_SMO));
                    csOracleSet.CS_LIST_IN_RESULT.RemoveRange(item.CS_LIST_IN_RESULT);

                }
                await csOracleSet.SaveChangesAsync();
                return CustomJsonResult.Create(true);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> GetPersonView(int CS_LIST_IN_ID)
        {
            try
            {
                var item =  await getCsItemSingle(CS_LIST_IN_ID).Include(x=>x.CS_LIST_IN_RESULT).ThenInclude(x=>x.CS_LIST_IN_RESULT_SMO).ThenInclude(x=>x.TFOMS).FirstOrDefaultAsync();
                if (item == null)
                    throw new ModelException($"Запись №{CS_LIST_IN_ID} не найдена!");
                var pers = new PersonView()
                {
                    STATUS = item.STATUS,
                    DOC = item.DOC,
                    DR = item.DR,
                    FIO = item.FIO,
                    POLIS = item.POLIS,
                    SNILS = item.SNILS,
                    W = item.W == 1 ? "Мужской" : "Женский",
                    RESULT = item.CS_LIST_IN_RESULT.Select(y => new PersonViewResult()
                    {
                        DR = y.DR,
                        DDEATH = y.DDEATH,
                        ENP = y.ENP,
                        LVL_D = y.LVL_D.LVL_D_TO_RUS(),
                        LVL_D_KOD = y.LVL_D_KOD.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(lvl => $"{lvl} - {lvl.LVL_D_kod_TO_RUS()}").ToArray(),
                        SMO = y.CS_LIST_IN_RESULT_SMO.Select(smo => new PersonViewResultSMO()
                        {
                            NAME_TFK = smo.TFOMS == null ? "" : smo.TFOMS.NAME_TFK,
                            DATE_B = smo.DATE_B,
                            DATE_E = smo.DATE_E,
                            ENP = smo.ENP,
                            SMO = smo.SMO,
                            SMO_NAME = csOracleSet.FindNameSMO(smo.TYPE_SMO, smo.SMO, smo.SMO_OK),
                            VPOLIS = smo.VPOLIS.ToRusStr(),
                            SPOLIS = smo.SPOLIS,
                            NPOLIS = smo.NPOLIS,
                            SMO_OK = smo.SMO_OK,
                            TF_OKATO = smo.TF_OKATO,
                            TYPE_SMO = smo.TypeSMO_RUS
                        }).ToArray()
                    }).ToArray()
                };
                return CustomJsonResult.Create(pers);
            }
            catch (Exception ex)
            {
                return InternalError(ex);
            }
        }


        [HttpGet]
        public async Task<CustomJsonResult> GetServiceState()
        {
            try
            {
                return CustomJsonResult.Create(await wcfIdentiScaner.IsEnabledAsync());
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
         
        }
      
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public CustomJsonResult GetServiceLog()
        {
            try
            {
                return CustomJsonResult.Create(wcfIdentiScaner.GetLog());
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
           
        }


    }

    public class ModelException : Exception
    {
        public string Key { get; }

        public ModelException()
        {
        }
        public ModelException( string message) : base(message)
        {
            Key = "";
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
