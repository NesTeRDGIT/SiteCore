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
        public ReportsController(MyOracleSet myOracleSet,IZPZExcelCreator zpzExcelCreator)
        {
            this.myOracleSet = myOracleSet;
            this.zpzExcelCreator = zpzExcelCreator;
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
                TempData["GET_VMP_VIEW"] = items.ObjectToXml();
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
                var list = TempData["GET_VMP_VIEW"].ToString().XmlToObject<List<CURRENT_VMP_OOMS2>>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_VMP_VIEW"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(CreateXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Текущее ВМП от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }

        }
        private byte[] CreateXLS(IEnumerable<CURRENT_VMP_OOMS2> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;
            var bold = ex.CreateType(new FontOpenXML { Bold = true }, new BorderOpenXML(), null);
            var Text = ex.CreateType(new FontOpenXML(), new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4 }, new BorderOpenXML(), null);
            var Date = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);
            var r = ex.GetRow(rowindex, true);
            ex.PrintCell(r, "A", "Код МО", bold);
            ex.PrintCell(r, "B", "Код СМО", bold);
            ex.PrintCell(r, "C", "ФИО", bold);
            ex.PrintCell(r, "D", "Вид ВМП", bold);
            ex.PrintCell(r, "E", "Метод ВМП", bold);
            ex.PrintCell(r, "F", "Группа ВМП", bold);
            ex.PrintCell(r, "G", "Диагноз", bold);
            ex.PrintCell(r, "H", "Дата начала", bold);
            ex.PrintCell(r, "I", "Дата окончания", bold);
            ex.PrintCell(r, "J", "Дата талона", bold);
            ex.PrintCell(r, "K", "Дата планируемой", bold);
            ex.PrintCell(r, "L", "Особый случай", bold);
            ex.PrintCell(r, "M", "Сумма", bold);
            rowindex++;
            foreach (var row in item)
            {
                r = ex.GetRow(rowindex, true);
                ex.PrintCell(r, "A", row.CODE_MO, Text);
                ex.PrintCell(r, "B", row.SMO, Text);
                ex.PrintCell(r, "C", row.FIO, Text);
                ex.PrintCell(r, "D", row.VID_HMP, Text);
                ex.PrintCell(r, "E", row.METOD_HMP, Text);
                ex.PrintCell(r, "F", row.GRP, Text);
                ex.PrintCell(r, "G", row.DS1, Text);
                ex.PrintCell(r, "H", row.DATE_1, Date);
                ex.PrintCell(r, "I", row.DATE_2, Date);
                ex.PrintCell(r, "J", row.TAL_D, Date);
                ex.PrintCell(r, "K", row.TAL_P, Date);
                ex.PrintCell(r, "L", row.OS_SLUCH, Text);
                if (row.SUMM.HasValue)
                    ex.PrintCell(r, "M", Convert.ToDouble(row.SUMM.Value), Number);

                rowindex++;
            }
            ex.Save();
            return ms.ToArray();
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
                TempData["HMPContainer"] = container.ObjectToXml();
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
                var list = TempData["HMPContainer"].ToString().XmlToObject<HMPContainer>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["HMPContainer"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(CreateVMPPeriodXLS(list.DATA), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ВМП за период {(list.DT1 == list.DT2? $"{list.DT1:MM_yyyy}":$"c {list.DT1:MM_yyyy} по {list.DT2:MM_yyyy}")}.xlsx"));
            }
            catch (Exception ex)
            {
                return CustomJsonResult.Create(ex.Message, false);
            }

        }
        private byte[] CreateVMPPeriodXLS(IEnumerable<VMP_OOMS> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            
            var boldHeadStyle = ex.CreateType(new FontOpenXML { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center, VerticalAlignment = VerticalAlignmentV.Center}, new BorderOpenXML(), null);
            var TextCenterStyle = ex.CreateType(new FontOpenXML{HorizontalAlignment = HorizontalAlignmentV.Center}, new BorderOpenXML(), null);
            var TextRightStyle = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var FloatStyle = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right}, new BorderOpenXML(), null);
          

            uint rowindex = 1;
            ex.PrintCell(rowindex, 1, "SLUCH_ID", boldHeadStyle);
            ex.PrintCell(rowindex, 2, "Код СМО", boldHeadStyle);
            ex.PrintCell(rowindex, 3, "Код МО", boldHeadStyle);
            ex.PrintCell(rowindex, 4, "Наименование МО", boldHeadStyle);
            ex.PrintCell(rowindex, 5, "Год", boldHeadStyle);
            ex.PrintCell(rowindex, 6, "Месяц", boldHeadStyle);
            ex.PrintCell(rowindex, 7, "ФИО", boldHeadStyle);
            ex.PrintCell(rowindex, 8, "Пол(1-мужской, 2-женский)", boldHeadStyle);
            ex.PrintCell(rowindex, 9, "Тип ДПФС", boldHeadStyle);
            ex.PrintCell(rowindex, 10, "Серия ДПФС", boldHeadStyle);
            ex.PrintCell(rowindex, 11, "№ ДПФС", boldHeadStyle);
            ex.PrintCell(rowindex, 12, "Возраст", boldHeadStyle);
            ex.PrintCell(rowindex, 13, "Вид ВМП", boldHeadStyle);
            ex.PrintCell(rowindex, 14, "Метод ВМП", boldHeadStyle);
            ex.PrintCell(rowindex, 15, "Группа ВМП", boldHeadStyle);
            ex.PrintCell(rowindex, 16, "Длительность", boldHeadStyle);
            ex.PrintCell(rowindex, 17, "Диагноз", boldHeadStyle);
            ex.PrintCell(rowindex, 18, "Сумма выставленная", boldHeadStyle);
            ex.PrintCell(rowindex, 19, "Сумма принятая", boldHeadStyle);
            rowindex++;
            foreach (var row in item)
            {
                ex.PrintCell(rowindex, 1, row.SLUCH_ID, TextCenterStyle);
                ex.PrintCell(rowindex, 2, row.SMO, TextCenterStyle);
                ex.PrintCell(rowindex, 3, row.CODE_MO, TextCenterStyle);
                ex.PrintCell(rowindex, 4, row.NAME_MO, TextRightStyle);
                ex.PrintCell(rowindex, 5, row.YEAR, TextCenterStyle);
                ex.PrintCell(rowindex, 6, row.MONTH, TextCenterStyle);
                ex.PrintCell(rowindex, 7, row.FIO, TextCenterStyle);
                ex.PrintCell(rowindex, 8, row.W, TextCenterStyle);
                ex.PrintCell(rowindex, 9, row.VPOLIS, TextCenterStyle);
                ex.PrintCell(rowindex, 10, row.NPOLIS, TextRightStyle);
                ex.PrintCell(rowindex, 11, row.SPOLIS, TextRightStyle);
                ex.PrintCell(rowindex, 12, row.AGE, TextCenterStyle);
                ex.PrintCell(rowindex, 13, row.VID_HMP, TextRightStyle);
                ex.PrintCell(rowindex, 14, row.METOD_HMP, TextRightStyle);
                ex.PrintCell(rowindex, 15, row.GRP_HMP, TextRightStyle);
                ex.PrintCell(rowindex, 16, row.DAYS, TextCenterStyle);
                ex.PrintCell(rowindex, 17, row.MKB, TextCenterStyle);
                ex.PrintCell(rowindex, 18, row.SUMV, FloatStyle);
                ex.PrintCell(rowindex, 19, row.SUMP, FloatStyle);
                rowindex++;
            }
            ex.AutoSizeColumns(1,19);
            ex.Save();
            return ms.ToArray();
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
                TempData["GET_ABORT_VIEW"] = t.ObjectToXml();
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
                var list = TempData["GET_ABORT_VIEW"].ToString().XmlToObject<List<Abort_Row>>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_ABORT_VIEW"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(CreateAbortXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Аборты от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
               return CustomJsonResult.Create(e.Message,false);
            }
         
        }

        private byte[] CreateAbortXLS(IEnumerable<Abort_Row> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            var TextLeftBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenterBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var NumberBOLD = ex.CreateType(new FontOpenXML { Bold = true, Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            var r = ex.GetRow(rowindex, true);
            ex.PrintCell(r, 1, "Диагноз", boldHead);
            ex.PrintCell(r, 2, "МКБ", boldHead);
            ex.PrintCell(r, 3, "Условие оказания", boldHead);
            ex.PrintCell(r, 4, "Кол-во", boldHead);
            ex.PrintCell(r, 5, "Сумма", boldHead);
            rowindex++;
            foreach (var row in item)
            {
                r = ex.GetRow(rowindex, true);
                var tl = TextLeft;
                var tc = TextCenter;
                var n = Number;
                if (!row.USL.HasValue)
                {
                    tl = TextLeftBOLD;
                    tc = TextCenterBOLD;
                    n = NumberBOLD;
                }

                ex.PrintCell(r, 1, row.Text, tl);
                ex.PrintCell(r, 2, row.DS, tc);
                ex.PrintCell(r, 3, row.USL, tc);
                ex.PrintCell(r, 4, row.C, tc);
                ex.PrintCell(r, 5, row.SUMV, n);

                rowindex++;
            }

            ex.AutoSizeColumns(1, 6);
            ex.Save();
            return ms.ToArray();
        }




        #endregion
        #region Отчет по ЭКО
        public class ECO_RECORD
        {
            public List<ECO_MTR_Row> ECO_MTR { get; set; }
            public List<ECO_MP_Row> ECO_MP { get; set; }
            public List<ECO_MP_Row> SVOD_ECO_MO
            {
                get
                {
                    var groupBy = ECO_MP.GroupBy(x => new { x.SMO, x.KSLP_NAME, x.KSLP });
                    return groupBy.Select(x => new ECO_MP_Row { SMO = x.Key.SMO, KSLP = x.Key.KSLP, KSLP_NAME = x.Key.KSLP_NAME, SUMV = x.Sum(y => y.SUMV), SUMP = x.Sum(y => y.SUMP ?? 0), SLUCH_ID = x.Count() }).ToList();
                }

            }
        }
        [Authorize(Roles = "OOMS, Admin")]
        [HttpGet]
        public CustomJsonResult GetEcoReport(int year, int month)
        {
            try
            {
                var taskECO_MTR = myOracleSet.GetEKO_MTRAsync(year, month);
                var taskECO_MP = myOracleSet.GetEKO_MPAsync(year, month);
                var result = new ECO_RECORD { ECO_MTR = taskECO_MTR.GetAwaiter().GetResult(), ECO_MP = taskECO_MP.GetAwaiter().GetResult() };

                TempData["GET_EKO_VIEW"] = result.ObjectToXml();

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
                var list = TempData["GET_EKO_VIEW"].ToString().XmlToObject<ECO_RECORD>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_EKO_VIEW"] = list.ObjectToXml();

                return CustomJsonResult.Create(File(CreateECOXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ЭКО от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        private byte[] CreateECOXLS(ECO_RECORD item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "На территории Заб.края");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var TextCenterDate = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center, Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);



            var r = ex.GetRow(rowindex, true);

            ex.PrintCell(r, 1, "Случай", boldHead);
            ex.PrintCell(r, 2, "СМО", boldHead);
            ex.PrintCell(r, 3, "Год", boldHead);
            ex.PrintCell(r, 4, "Месяц", boldHead);
            ex.PrintCell(r, 5, "Код МО", boldHead);
            ex.PrintCell(r, 6, "Наименование МО", boldHead);
            ex.PrintCell(r, 7, "Фамилия", boldHead);
            ex.PrintCell(r, 8, "Имя", boldHead);
            ex.PrintCell(r, 9, "Отчество", boldHead);
            ex.PrintCell(r, 10, "Дата рождения", boldHead);
            ex.PrintCell(r, 11, "Дата начала", boldHead);
            ex.PrintCell(r, 12, "Дата окончания", boldHead);
            ex.PrintCell(r, 13, "КСГ", boldHead);
            ex.PrintCell(r, 14, "Наименование", boldHead);
            ex.PrintCell(r, 15, "Сумма", boldHead);
            ex.PrintCell(r, 16, "Принято", boldHead);
            ex.PrintCell(r, 17, "КСЛП", boldHead);
            ex.PrintCell(r, 18, "Наименование", boldHead);

            rowindex++;
            foreach (var row in item.ECO_MP)
            {
                r = ex.GetRow(rowindex, true);

                ex.PrintCell(r, 1, row.SLUCH_ID, TextCenter);
                ex.PrintCell(r, 2, row.SMO, TextLeft);
                ex.PrintCell(r, 3, row.YEAR, TextCenter);
                ex.PrintCell(r, 4, row.MONTH, TextCenter);
                ex.PrintCell(r, 5, row.CODE_MO, TextCenter);
                ex.PrintCell(r, 6, row.NAM_MOK, TextLeft);
                ex.PrintCell(r, 7, row.FAM, TextLeft);
                ex.PrintCell(r, 8, row.IM, TextLeft);
                ex.PrintCell(r, 9, row.OT, TextLeft);
                ex.PrintCell(r, 10, row.DR, TextCenterDate);
                ex.PrintCell(r, 11, row.DATE_1, TextCenterDate);
                ex.PrintCell(r, 12, row.DATE_2, TextCenterDate);
                ex.PrintCell(r, 13, row.N_KSG, TextCenter);
                ex.PrintCell(r, 14, row.NAME_KSG, TextLeft);
                ex.PrintCell(r, 15, row.SUMV, Number);
                ex.PrintCell(r, 16, row.SUMP, Number);
                ex.PrintCell(r, 17, row.KSLP, TextCenter);
                ex.PrintCell(r, 18, row.KSLP_NAME, TextLeft);

                rowindex++;
            }

            ex.AutoSizeColumns(1, 18);

            ex.AddSheet("На территории Заб.края(СВОД)");
            rowindex = 1;
            r = ex.GetRow(rowindex, true);


            ex.PrintCell(r, 1, "СМО", boldHead);
            ex.PrintCell(r, 2, "КСЛП", boldHead);
            ex.PrintCell(r, 3, "Наименование", boldHead);
            ex.PrintCell(r, 4, "Кол-во", boldHead);
            ex.PrintCell(r, 5, "Сумма", boldHead);
            ex.PrintCell(r, 6, "Принято", boldHead);
            rowindex++;
            foreach (var row in item.SVOD_ECO_MO)
            {
                r = ex.GetRow(rowindex, true);
                ex.PrintCell(r, 1, row.SMO, TextLeft);
                ex.PrintCell(r, 2, row.KSLP, TextCenter);
                ex.PrintCell(r, 3, row.KSLP_NAME, TextLeft);
                ex.PrintCell(r, 4, row.SLUCH_ID, TextCenter);
                ex.PrintCell(r, 5, row.SUMV, Number);
                ex.PrintCell(r, 6, row.SUMP, Number);
                rowindex++;
            }
            ex.AutoSizeColumns(1, 5);


            ex.AddSheet("Вне Заб.края");
            rowindex = 1;
            r = ex.GetRow(rowindex, true);

            ex.PrintCell(r, 1, "Случай", boldHead);
            ex.PrintCell(r, 2, "Фамилия", boldHead);
            ex.PrintCell(r, 3, "Имя", boldHead);
            ex.PrintCell(r, 4, "Отчество", boldHead);
            ex.PrintCell(r, 5, "Дата рождения", boldHead);
            ex.PrintCell(r, 6, "Дата начала", boldHead);
            ex.PrintCell(r, 7, "Дата окончания", boldHead);
            ex.PrintCell(r, 8, "Сумма", boldHead);
            ex.PrintCell(r, 9, "Принято", boldHead);
            ex.PrintCell(r, 10, "МКБ", boldHead);
            ex.PrintCell(r, 11, "Диагноз", boldHead);
            ex.PrintCell(r, 12, "Дата платежа", boldHead);
            ex.PrintCell(r, 13, "	№ платежа", boldHead);
            ex.PrintCell(r, 14, "Код МО", boldHead);
            ex.PrintCell(r, 15, "Наименование МО", boldHead);
            ex.PrintCell(r, 16, "OKATO", boldHead);
            ex.PrintCell(r, 17, "Территория", boldHead);
            ex.PrintCell(r, 18, "Комментарий", boldHead);
            ex.PrintCell(r, 19, "Услуги", boldHead);
            rowindex++;
            foreach (var row in item.ECO_MTR)
            {
                r = ex.GetRow(rowindex, true);
                ex.PrintCell(r, 1, row.SLUCH_ID, TextCenter);
                ex.PrintCell(r, 2, row.FAM, TextLeft);
                ex.PrintCell(r, 3, row.IM, TextLeft);
                ex.PrintCell(r, 4, row.OT, TextLeft);
                ex.PrintCell(r, 5, row.DR, TextCenterDate);
                ex.PrintCell(r, 6, row.DATE_1, TextCenterDate);
                ex.PrintCell(r, 7, row.DATE_2, TextCenterDate);
                ex.PrintCell(r, 8, row.SUMV, Number);
                ex.PrintCell(r, 9, row.SUMP, Number);
                ex.PrintCell(r, 10, row.DS1, TextLeft);
                ex.PrintCell(r, 11, row.DS_NAME, TextLeft);
                ex.PrintCell(r, 12, row.DPLAT, TextCenterDate);
                ex.PrintCell(r, 13, row.NPLAT, TextLeft);
                ex.PrintCell(r, 14, row.LPU, TextLeft);
                ex.PrintCell(r, 15, row.NAM_MOK, TextLeft);
                ex.PrintCell(r, 16, row.C_OKATO, TextCenter);
                ex.PrintCell(r, 17, row.NAME_TFK, TextLeft);
                ex.PrintCell(r, 18, row.COMENTSL, TextLeft);
                ex.PrintCell(r, 19, row.USL, TextLeft);
                rowindex++;
            }
            ex.AutoSizeColumns(1, 19);

            ex.Save();
            return ms.ToArray();
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
                TempData["GET_KOHL_VIEW"] = t.ObjectToXml();
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
                var list = TempData["GET_KOHL_VIEW"].ToString().XmlToObject<List<KOHL_Row>>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_KOHL_VIEW"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(CreateKOHLXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Кохлеарная имплантация от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
            
        }

        private byte[] CreateKOHLXLS(List<KOHL_Row> items)
        {
            var ms = new MemoryStream();
            var ex = new ExcelOpenXML(ms, "Кохл");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextRight = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var TextCenterDate = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center, Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);

            var TextLeftBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenterBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var NumberBOLD = ex.CreateType(new FontOpenXML { Bold = true, Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            var r = ex.GetRow(rowindex, true);

            ex.PrintCell(r, 1, "ОКАТО", boldHead);
            ex.PrintCell(r, 2, "Наименование", boldHead);
            ex.PrintCell(r, 3, "Код МО", boldHead);
            ex.PrintCell(r, 4, "Наименование МО", boldHead);
            ex.PrintCell(r, 5, "Фамилия", boldHead);
            ex.PrintCell(r, 6, "Имя", boldHead);
            ex.PrintCell(r, 7, "Отчество", boldHead);
            ex.PrintCell(r, 8, "Дата рождения", boldHead);
            ex.PrintCell(r, 9, "Дата начала", boldHead);
            ex.PrintCell(r, 10, "Дата окончания", boldHead);
            ex.PrintCell(r, 11, "МКБ", boldHead);
            ex.PrintCell(r, 12, "Диагноз", boldHead);
            ex.PrintCell(r, 13, "Сумма", boldHead);
            ex.PrintCell(r, 14, "Принято", boldHead);
            ex.PrintCell(r, 15, "Комментарий", boldHead);
            ex.PrintCell(r, 16, "Услуги", boldHead);
            ex.PrintCell(r, 17, "SLUCH_ID", boldHead);
            rowindex++;
            foreach (var row in items)
            {
                r = ex.GetRow(rowindex, true);


                ex.PrintCell(r, 1, row.C_OKATO, TextCenter);
                ex.PrintCell(r, 2, row.NAME_TFK, TextLeft);
                ex.PrintCell(r, 3, row.LPU, TextCenter);
                ex.PrintCell(r, 4, row.NAM_MOK, TextLeft);

                ex.PrintCell(r, 5, row.FAM, TextLeft);
                ex.PrintCell(r, 6, row.IM, TextLeft);
                ex.PrintCell(r, 7, row.OT, TextLeft);
                ex.PrintCell(r, 8, row.DR, TextCenterDate);
                ex.PrintCell(r, 9, row.DATE_1, TextCenterDate);
                ex.PrintCell(r, 10, row.DATE_2, TextCenterDate);
                ex.PrintCell(r, 11, row.DS1, TextCenter);
                ex.PrintCell(r, 12, row.DS1_NAME, TextLeft);
                ex.PrintCell(r, 13, row.SUMV, Number);
                ex.PrintCell(r, 14, row.SUMP, Number);
                ex.PrintCell(r, 15, row.COMENTSL, TextCenter);
                ex.PrintCell(r, 16, row.USL, TextLeft);
                ex.PrintCell(r, 17, row.SLUCH_ID, TextLeft);

                rowindex++;
            }

            ex.AutoSizeColumns(1, 16);
            ex.Save();
            return ms.ToArray();
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
                TempData["GET_OKS_ONMK_VIEW"] = t.ObjectToXml();
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
                var list = TempData["GET_OKS_ONMK_VIEW"].ToString().XmlToObject<List<OKS_ONMK_Row>>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_OKS_ONMK_VIEW"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(CreateOKS_ONMKXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ОКС ОНМК от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }

        private byte[] CreateOKS_ONMKXLS(List<OKS_ONMK_Row> items)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Кохл");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextRight = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var TextCenterDate = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center, Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);

        

            var r = ex.GetRow(rowindex, true);



            ex.PrintCell(r, 1, "SLUCH_ID", boldHead);
            ex.PrintCell(r, 2, "СМО", boldHead);
            ex.PrintCell(r, 3, "Код МО", boldHead);
            ex.PrintCell(r, 4, "Наименование МО", boldHead);
            ex.PrintCell(r, 5, "Уровень", boldHead);
            ex.PrintCell(r, 6, "Профиль", boldHead);
            ex.PrintCell(r, 7, "Форма", boldHead);
            ex.PrintCell(r, 8, "Фамилия", boldHead);
            ex.PrintCell(r, 9, "Имя", boldHead);
            ex.PrintCell(r, 10, "Отчество", boldHead);
            ex.PrintCell(r, 11, "Дата рождения", boldHead);
            ex.PrintCell(r, 12, "Серия полиса", boldHead);
            ex.PrintCell(r, 13, "№ полиса", boldHead);
            ex.PrintCell(r, 14, "Дата начала", boldHead);
            ex.PrintCell(r, 15, "Дата окончания", boldHead);
            ex.PrintCell(r, 16, "МКБ", boldHead);
            ex.PrintCell(r, 17, "Диагноз", boldHead);
            ex.PrintCell(r, 18, "КСГ", boldHead);
            ex.PrintCell(r, 19, "Наименование КСГ", boldHead);
            ex.PrintCell(r, 20, "Результат", boldHead);
            ex.PrintCell(r, 21, "Исход", boldHead);
            ex.PrintCell(r, 22, "Сумма", boldHead);
            ex.PrintCell(r, 23, "Принято", boldHead);

            rowindex++;
            foreach (var row in items)
            {
                r = ex.GetRow(rowindex, true);


                ex.PrintCell(r, 1, row.SLUCH_ID, TextCenter);
                ex.PrintCell(r, 2, row.SMO, TextCenter);
                ex.PrintCell(r, 3, row.CODE_MO, TextCenter);
                ex.PrintCell(r, 4, row.NAM_MOK, TextLeft);
                ex.PrintCell(r, 5, row.LEV, TextCenter);
                ex.PrintCell(r, 6, row.PROFIL, TextLeft);
                ex.PrintCell(r, 7, row.FORMA, TextLeft);

                ex.PrintCell(r, 8, row.FAM, TextCenter);
                ex.PrintCell(r, 9, row.IM, TextCenter);
                ex.PrintCell(r, 10, row.OT, TextCenter);
                ex.PrintCell(r, 11, row.DR, TextCenterDate);
                ex.PrintCell(r, 12, row.SPOLIS, TextCenter);
                ex.PrintCell(r, 13, row.NPOLIS, TextCenter);
                ex.PrintCell(r, 14, row.DATE_1, TextCenterDate);
                ex.PrintCell(r, 15, row.DATE_2, TextCenterDate);
                ex.PrintCell(r, 16, row.DS1, TextCenter);
                ex.PrintCell(r, 17, row.DS1_NAME, TextLeft);
                ex.PrintCell(r, 18, row.N_KSG, TextCenter);
                ex.PrintCell(r, 19, row.NAME_KSG, TextRight);
                ex.PrintCell(r, 20, row.RSLT, TextLeft);
                ex.PrintCell(r, 21, row.ISHOD, TextLeft);
                ex.PrintCell(r, 22, row.SUMV, Number);
                ex.PrintCell(r, 23, row.SUMP, Number);


                rowindex++;
            }

            ex.AutoSizeColumns(1, 24);
            ex.Save();
            return ms.ToArray();
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
                TempData["GET_EFFECTIVENESS_VIEW"] = val.ObjectToXml();
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
                var list = TempData["GET_EFFECTIVENESS_VIEW"].ToString().XmlToObject<EffectivenessContainer>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["GET_EFFECTIVENESS_VIEW"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(zpzExcelCreator.CreateXLSEFFECTIVENESS(list.DATA), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Результативность с {list.DT1:yyyy-MM} по {list.DT2:yyyy-MM} от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx"));
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
                TempData["ResultControl"] = val.ObjectToXml();
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
                var list = TempData["ResultControl"].ToString().XmlToObject<ResultControl>();
                if (list == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["ResultControl"] = list.ObjectToXml();
                return CustomJsonResult.Create(File(zpzExcelCreator.CreateXLSResultControl(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Результаты контрольно-экспертных мероприятий.xlsx"));
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
                TempData["SMPContainer"] = new SMPContainer(dt1,dt2, list).ObjectToXml();
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
                var container = TempData["SMPContainer"].ToString().XmlToObject<SMPContainer>();
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["SMPContainer"] = container.ObjectToXml();
                return CustomJsonResult.Create(File(CreateSmpXLS(container.DATA), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет по СМП за {(container.DT1==container.DT2? $"{container.DT1:MM_yyyy}": $"{container.DT1:MM_yyyy}-{container.DT2:MM_yyyy}")}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }


        private byte[] CreateSmpXLS(List<SMPRow> data)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");

            var boldHeadNotBorder = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, null, null);
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

            var FloatStyle = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var IntStyle = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F1, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            

            uint rowindex = 1;
   

            ex.PrintCell(rowindex, 1, "Сведения об объемах и стоимости скорой медицинской помощи", boldHeadNotBorder); ex.AddMergedRegion(new CellRangeAddress(rowindex,1, rowindex,6));
            rowindex+=2;


            ex.PrintCell(rowindex, 1, "Показатель", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex+1, 1));
            ex.PrintCell(rowindex, 2, "№ строки", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 2, rowindex + 1, 2));
            ex.PrintCell(rowindex, 3, "Выполнено выездов ", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 3, rowindex, 4));
            ex.PrintCell(rowindex, 5, "из них: к детям в возрасте 0-17 лет включительно", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 5, rowindex, 6));
            rowindex++;
            ex.PrintCell(rowindex, 3, "Объемы, кол-во случаев", boldHead);
            ex.PrintCell(rowindex, 4, "Стоимость, тыс.руб", boldHead);
            ex.PrintCell(rowindex, 5, "Объемы, кол-во случаев", boldHead);
            ex.PrintCell(rowindex, 6, "Стоимость, тыс.руб", boldHead);
            rowindex++;
            ex.PrintCell(rowindex, 1, "1", boldHead);
            ex.PrintCell(rowindex, 2, "2", boldHead);
            ex.PrintCell(rowindex, 3, "3", boldHead);
            ex.PrintCell(rowindex, 4, "4", boldHead);
            ex.PrintCell(rowindex, 5, "5", boldHead);
            ex.PrintCell(rowindex, 6, "6", boldHead);
            rowindex++;
            foreach (var row in data)
            {
                ex.PrintCell(rowindex, 1, row.POK, TextLeft);
                ex.PrintCell(rowindex, 2, row.NN, TextCenter);
                ex.PrintCell(rowindex, 3, row.KOL, IntStyle);
                ex.PrintCell(rowindex, 4, row.SUM, FloatStyle);
                ex.PrintCell(rowindex, 5, row.KOL_DET, IntStyle);
                ex.PrintCell(rowindex, 6, row.SUM_DET, FloatStyle);
                rowindex++;
            }
            ex.SetColumnWidth(1,76);
            ex.SetColumnWidth(2, 14);
            ex.SetColumnWidth(3, 14);
            ex.SetColumnWidth(4, 14);
            ex.SetColumnWidth(5, 14);
            ex.SetColumnWidth(6, 14);
            ex.SetRowHeigth(3, 52);
            ex.SetRowHeigth(4, 52);
            
            ex.Save();
            return ms.ToArray();
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
                TempData["PENSContainer"] = new PENSContainer(year, list).ObjectToXml();
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
                var container = TempData["PENSContainer"].ToString().XmlToObject<PENSContainer>();
                if (container == null)
                {
                    throw new Exception("Не удалось найти отчет");
                }
                TempData["PENSContainer"] = container.ObjectToXml();
                return CustomJsonResult.Create(File(CreatePensXLS(container.DATA), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет пожилые за {container.YEAR}.xlsx"));
            }
            catch (Exception e)
            {
                return CustomJsonResult.Create(e.Message, false);
            }
        }


        private byte[] CreatePensXLS(List<PENSRow> data)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");

            var boldHeadNotBorder = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, null, null);
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

            var FloatStyle = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var IntStyle = ex.CreateType(new FontOpenXML { Format = (uint)DefaultNumFormat.F1, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);


            uint rowindex = 1;

            ex.PrintCell(rowindex, 1, "Код МО", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex+2, 1));
            ex.PrintCell(rowindex, 2, "Наименование МО", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 2, rowindex + 2, 2));
            ex.PrintCell(rowindex, 3, "55+ женщины, 60+ мужчины", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 3, rowindex, 6));
            ex.PrintCell(rowindex, 7, "в том числе 65+", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 7, rowindex, 10));
            rowindex++;
            ex.PrintCell(rowindex, 3, "Диспансеризация", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 3, rowindex, 4));
            ex.PrintCell(rowindex, 5, "Проф.осмотры", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 5, rowindex, 6));
            ex.PrintCell(rowindex, 7, "Диспансеризация", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 7, rowindex, 8));
            ex.PrintCell(rowindex, 9, "Проф.осмотры", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 9, rowindex, 10));
            rowindex++;
            ex.PrintCell(rowindex, 3, "муж.", boldHead);
            ex.PrintCell(rowindex, 4, "жен.", boldHead);
            ex.PrintCell(rowindex, 5, "муж.", boldHead);
            ex.PrintCell(rowindex, 6, "жен.", boldHead);
            ex.PrintCell(rowindex, 7, "муж.", boldHead);
            ex.PrintCell(rowindex, 8, "жен.", boldHead);
            ex.PrintCell(rowindex, 9, "муж.", boldHead);
            ex.PrintCell(rowindex, 10, "жен.", boldHead);

            rowindex++;

            ex.PrintCell(rowindex, 1, "1", boldHead);
            ex.PrintCell(rowindex, 2, "2", boldHead);
            ex.PrintCell(rowindex, 3, "3", boldHead);
            ex.PrintCell(rowindex, 4, "4", boldHead);
            ex.PrintCell(rowindex, 5, "5", boldHead);
            ex.PrintCell(rowindex, 6, "6", boldHead);
            ex.PrintCell(rowindex, 7, "7", boldHead);
            ex.PrintCell(rowindex, 8, "8", boldHead);
            ex.PrintCell(rowindex, 9, "9", boldHead);
            ex.PrintCell(rowindex, 10, "10", boldHead);
            rowindex++;
            foreach (var row in data)
            {
                ex.PrintCell(rowindex, 1, row.CODE_MO, TextCenter);
                ex.PrintCell(rowindex, 2, row.NAME, TextLeft);
                
                ex.PrintCell(rowindex, 3, row.DISP_M_GR1, IntStyle);
                ex.PrintCell(rowindex, 4, row.DISP_G_GR1, IntStyle);
                ex.PrintCell(rowindex, 5, row.PROF_M_GR1, IntStyle);
                ex.PrintCell(rowindex, 6, row.PROF_G_GR1, IntStyle);

                ex.PrintCell(rowindex, 7, row.DISP_M_GR2, IntStyle);
                ex.PrintCell(rowindex, 8, row.DISP_G_GR2, IntStyle);
                ex.PrintCell(rowindex, 9, row.PROF_M_GR2, IntStyle);
                ex.PrintCell(rowindex, 10, row.PROF_G_GR2, IntStyle);


                rowindex++;
            }
            ex.SetColumnWidth(1, 10);
            ex.SetColumnWidth(2, 40);
            ex.SetColumnWidth(3, 10);
            ex.SetColumnWidth(4, 10);
            ex.SetColumnWidth(5, 10);
            ex.SetColumnWidth(6, 10);
            ex.SetColumnWidth(6, 10);
            ex.SetColumnWidth(7, 10);
            ex.SetColumnWidth(8, 10);
            ex.SetColumnWidth(9, 10);
            ex.SetColumnWidth(10, 10);



            ex.Save();
            return ms.ToArray();
        }
        #endregion




    }
}
