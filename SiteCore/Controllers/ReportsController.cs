using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "OZPZ, OOMS, Admin")]
    public class ReportsController : Controller
    {

        MyOracleSet myOracleSet;
        private IZPZExcelCreator zpzExcelCreator;
        private IOMSXLSCreator omsxlsCreator;
        public ReportsController(MyOracleSet myOracleSet,IZPZExcelCreator zpzExcelCreator, IOMSXLSCreator omsxlsCreator)
        {
            this.myOracleSet = myOracleSet;
            this.zpzExcelCreator = zpzExcelCreator;
            this.omsxlsCreator = omsxlsCreator;
        }


        private void AddTempData(string key, object val)
        {
            TempData[key] = val.ObjectToXml();
        }

        private T GetTempData<T>(string key)
        {
            if (TempData.ContainsKey(key))
            {
                var item = TempData[key];
                TempData[key] = item;
                return item.ToString().XmlToObject<T>();
            }
            return default;
        }
        public IActionResult Reports()
        {
            return View();
        }

        #region  Отчет по ВМП-текущее
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetVmpReport()
        {
            try
            {
                var items = (await myOracleSet.GetVMPReportAsync()).ToList();
                AddTempData("GET_VMP_VIEW", items);
                return CustomJsonResult.Create(items);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }


        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetVmpReportXls()
        {
            try
            {
                var list = GetTempData<List<CURRENT_VMP_OOMS2>>("GET_VMP_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateCURRENT_VMP_OOMSXLS(list), IOMSXLSCreator.ContentType, $"Текущее ВМП от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }

        }
   
        #endregion
        #region  Отчет по ВМП-период
        public class HMPContainer
        {
            public HMPContainer()
            {

            }

            public HMPContainer(DateTime dt1, DateTime dt2, List<VMP_OOMS> data)
            {
                this.DATA = data;
                this.DT1 = dt1.Date;
                this.DT2 = dt2.Date;
            }
            public DateTime DT1 { get; set; }
            public DateTime DT2 { get; set; }
            public List<VMP_OOMS> DATA { get; set; }
        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetVMPPeriodReport(DateTime dt1,DateTime dt2)
        {
            try
            {
                var container = new HMPContainer(dt1,dt2,await myOracleSet.GetVMP_PERIODAsync(dt1,dt2));
                AddTempData("HMPContainer", container);
                return CustomJsonResult.Create(container.DATA);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }


        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetVMPPeriodXls()
        {
            try
            {
                var list = GetTempData< HMPContainer> ("HMPContainer");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateVMPPeriodXLS(list.DATA), IOMSXLSCreator.ContentType, $"ВМП за период {(list.DT1 == list.DT2? $"{list.DT1:MM_yyyy}":$"c {list.DT1:MM_yyyy} по {list.DT2:MM_yyyy}")}.xlsx"));
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }

        }
        
        #endregion
        #region отчет по Абортам
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]

        public async Task<CustomJsonResult> GetAbortReport(int year)
        {
            try
            {
                var t = (await myOracleSet.GetReportAbortAsync(year)).ToList();
                AddTempData("GET_ABORT_VIEW", t);
               
                return CustomJsonResult.Create(t);
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }
        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetAbortXls()
        {
            try
            {
                var list = GetTempData<List<Abort_Row>>("GET_ABORT_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateAbortXLS(list), IOMSXLSCreator.ContentType, $"Аборты от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
               return CustomJsonResult.Create(e.Message,false);
            }
         
        }


        #endregion
        #region Отчет по ЭКО
      
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetEcoReport(int year, int month)
        {
            try
            {
                var result = await myOracleSet.GetECO_RECORDAsync(year, month);
                AddTempData("GET_EKO_VIEW", result);
              
                return CustomJsonResult.Create(result);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetEcoXls()
        {
            try
            {
                var list = GetTempData<ECO_RECORD>("GET_EKO_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateECOXLS(list), IOMSXLSCreator.ContentType, $"ЭКО от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
       
        #endregion
        #region Отчет по Кохлеарке
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetKohlReport([FromQuery] DateTime dt1, [FromQuery] DateTime dt2)
        {
            try
            {
                var t = await myOracleSet.GetKOHLAsync(dt1, dt2);
                AddTempData("GET_KOHL_VIEW", t);
                return CustomJsonResult.Create(t);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetKohlXls()
        {
            try
            {
                var list = GetTempData<List<KOHL_Row>>("GET_KOHL_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateKOHLXLS(list), IOMSXLSCreator.ContentType, $"Кохлеарная имплантация от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
            
        }
        #endregion
        #region Отчет ОКС ОНМК
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetOksOnmkReport(int YEAR)
        {
            try
            {
                var t = await myOracleSet.GetOKS_ONMKAsync(YEAR);
                AddTempData("GET_OKS_ONMK_VIEW", t);
                return CustomJsonResult.Create(t);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
           
        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public IActionResult GetOksOnmkXls()
        {
            try
            {
                var list = GetTempData<List<OKS_ONMK_Row>>("GET_OKS_ONMK_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateOKS_ONMKXLS(list), IOMSXLSCreator.ContentType, $"ОКС ОНМК от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }

   
        #endregion
        #region  Отчет Результативность

        public class EffectivenessContainer
        {
            public EffectivenessContainer()
            {

            }

            public EffectivenessContainer(DateTime dt1, DateTime dt2, List<ZPZ_EFFECTIVENESS> data)
            {
                this.DATA = data;
                this.DT1 = dt1;
                this.DT2 = dt2;
            }

            public DateTime DT1 { get; set; }
            public DateTime DT2 { get; set; }

            public List<ZPZ_EFFECTIVENESS> DATA { get; set; }
        }

        [Authorize(Roles = "OZPZ, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetEffectivenessReport(DateTime dt1, DateTime dt2)
        {
            try
            {
                var val = new EffectivenessContainer(dt1, dt2, await myOracleSet.Get_ZPZ_EFFECTIVENESSAsync(dt1, dt2));
                AddTempData("GET_EFFECTIVENESS_VIEW", val);
                return CustomJsonResult.Create(val.DATA);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OZPZ, Admin")]
        [HttpGet]
        public IActionResult GetEffectivenessXls()
        {
            try
            {
                var list = GetTempData<EffectivenessContainer>("GET_EFFECTIVENESS_VIEW");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(zpzExcelCreator.CreateXLSEFFECTIVENESS(list.DATA), IOMSXLSCreator.ContentType, $"Результативность с {list.DT1:yyyy-MM} по {list.DT2:yyyy-MM} от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }

        #endregion
        #region  Отчет резульатаы контрольно-экспертных мироприятий
        [Authorize(Roles = "OZPZ, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetResultControlReport(DateTime dt1, DateTime dt2)
        {
            try
            {
                var resultControlVZR =  myOracleSet.GetResultControlVZRAsync(dt1, dt2);
                var resultControlDET = myOracleSet.GetResultControlDETAsync(dt1, dt2);
                var resultVZR = await resultControlVZR;
                var resultDET = await resultControlDET;
                var val = new ResultControl
                {
                    DET = resultDET,
                    VZR = resultVZR
                };
                AddTempData("ResultControl", val);
                return CustomJsonResult.Create(val);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OZPZ, Admin")]
        [HttpGet]
        public CustomJsonResult GetResultControlXls()
        {
            try
            {
                var list = GetTempData<ResultControl>("ResultControl");
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(zpzExcelCreator.CreateXLSResultControl(list), IOMSXLSCreator.ContentType, $"Результаты контрольно-экспертных мероприятий.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }

        #endregion
        #region Отчет СМП


        public class  SMPContainer
        {
            public SMPContainer()
            {

            }

                public SMPContainer(DateTime dt1, DateTime dt2, List<SMPRow> data)
            {
                this.DATA = data;
                this.DT1 = dt1.Date;
                this.DT2 = dt2.Date;
            }
            public DateTime DT1 { get; set; }
            public DateTime DT2 { get; set; }
            public List<SMPRow> DATA { get; set; }
        }
            
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetSmpReport(DateTime dt1, DateTime dt2)
        {
            try
            {
                var list = await myOracleSet.GetSMPAsync(dt1, dt2);
                AddTempData("SMPContainer", new SMPContainer(dt1, dt2, list));
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetSmpXls()
        {
            try
            {
                var container = GetTempData<SMPContainer>("SMPContainer");
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateSmpXLS(container.DATA), IOMSXLSCreator.ContentType, $"Отчет по СМП за {(container.DT1==container.DT2? $"{container.DT1:MM_yyyy}": $"{container.DT1:MM_yyyy}-{container.DT2:MM_yyyy}")}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }


    
        #endregion
        #region Соятояние БД

        [HttpGet]
        public async Task<CustomJsonResult> GetDbState()
        {
            try
            {
                var list = await myOracleSet.GetDataBaseStateAsync();
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }

        #endregion
        #region Отчет Пожилые


        public class PENSContainer
        {
            public PENSContainer()
            {

            }

            public PENSContainer(int year, List<PENSRow> data)
            {
                this.DATA = data;
                this.YEAR = year;
            }
            public int YEAR { get; set; }
       
            public List<PENSRow> DATA { get; set; }
        }

        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetPensReport(int year)
        {
            try
            {
                var list = await myOracleSet.GetPENSAsync(year);
                AddTempData("PENSContainer", new PENSContainer(year, list));
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetPensXls()
        {
            try
            {
                var container = GetTempData<PENSContainer>("PENSContainer");
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                
                return CustomJsonResult.Create(File(omsxlsCreator.CreatePensXLS(container.DATA), IOMSXLSCreator.ContentType, $"Отчет пожилые за {container.YEAR}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }


    
        #endregion
        #region Отчет по диспансеризации


        public class DISPContainer
        {
            public DISPContainer()
            {

            }

            public DISPContainer(DateTime dt, DispRecord data)
            {
                this.DATA = data;
                this.DT = dt;
            }
            public DateTime DT { get; set; }

            public DispRecord DATA { get; set; }
        }

        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetDispReport(int year, int month)
        {
            try
            {
                var dt = new DateTime(year, month, 1);
                var list = await myOracleSet.GetDispReportAsync(dt);
                AddTempData("DISPContainer", new DISPContainer(dt, list));
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetDispXls()
        {
            try
            {
                var container = GetTempData<DISPContainer>("DISPContainer");
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }

                return CustomJsonResult.Create(File(omsxlsCreator.CreateDispXLS(container.DATA), IOMSXLSCreator.ContentType, $"Отчет по диспансеризации за {container.DT:MMMMMMMMMMMMM yyyy}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }



        #endregion


        #region Отчет КВ2 МТР


        public class KV2_MTRContainer
        {
            public KV2_MTRContainer()
            {

            }

            public KV2_MTRContainer(DateTime dt, List<Kv2MtrRow> data)
            {
                this.DATA = data;
                this.DT = dt;
            }
            public DateTime DT { get; set; }

            public List<Kv2MtrRow> DATA { get; set; }
        }

        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetKv2MtrReport(int year, int month)
        {
            try
            {
                var dt = new DateTime(year, month, 1);
                var list = await myOracleSet.GetKV2_MTRAsync(dt);
                AddTempData("KV2_MTRContainer", new KV2_MTRContainer(dt, list));
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetKv2MtrXls()
        {
            try
            {
                var container = GetTempData<KV2_MTRContainer>("KV2_MTRContainer");
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateKv2MtrXLS(container.DATA), IOMSXLSCreator.ContentType, $"Отчет КВ2 МТР за {container.DT:MMMMMMMMMMMMM yyyy}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }



        #endregion

        #region Отчет КВ2 МТР


        public class DLIContainer
        {
            public DLIContainer()
            {

            }

            public DLIContainer(int YEAR, DliRecord data)
            {
                this.DATA = data;
                this.YEAR = YEAR;
            }
            public int YEAR { get; set; }

            public DliRecord DATA { get; set; }
        }

        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public async Task<CustomJsonResult> GetDliReport(int year)
        {
            try
            {
                var list = await myOracleSet.GetDliAsync(year);
                AddTempData("DLIContainer", new DLIContainer(year, list));
             
                return CustomJsonResult.Create(list);
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }

        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetDliXls()
        {
            try
            {
                var container = GetTempData<DLIContainer>("DLIContainer");
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                return CustomJsonResult.Create(File(omsxlsCreator.CreateDLIXLS(container.DATA), IOMSXLSCreator.ContentType, $"Отчет ДЛИ за {container.YEAR} год от {DateTime.Now:dd.MM.yyyy}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }



        #endregion

    }
}
