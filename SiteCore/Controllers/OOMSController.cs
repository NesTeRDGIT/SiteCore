using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteCore.Class;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Controllers
{
    [Authorize(Roles = "OOMS, Admin")]
    public class OOMSController : Controller
    {
        MyOracleSet myOracleSet;
        public OOMSController(MyOracleSet myOracleSet)
        {
            this.myOracleSet = myOracleSet;
        }
        [HttpGet]
        public IActionResult CURRENT_VMP_VIEW()
        {
            return View(new List<CURRENT_VMP_OOMS>());
        }
        [HttpGet]
        public IActionResult GET_VMP_VIEW()
        {          
            var t = myOracleSet.CURRENT_VMP_OOMS.ToList();
            TempData["GET_VMP_VIEW"] = t.ObjectToXml();
            return PartialView("_CURRENT_VMP_VIEWPartical", t);
        }
        [HttpGet]
        public IActionResult GET_VMP_VIEWXLS()
        {
            var list = TempData["GET_VMP_VIEW"].ToString().XmlToObject<List<CURRENT_VMP_OOMS>>();
            TempData["GET_VMP_VIEW"] = list.ObjectToXml();
            return list == null ? null : File(CreateXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Текущее ВМП от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
        }
        private byte[] CreateXLS(IEnumerable<CURRENT_VMP_OOMS> item)
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

        [HttpGet]
        public IActionResult AbortView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GET_ABORT_VIEW(int YEAR)
        {          
            var t = myOracleSet.GetReportAbort(YEAR).ToList();
            TempData["GET_ABORT_VIEW"] = t.ObjectToXml();
            return PartialView("_AbortViewPartical", t);
        }
        [HttpGet]
        public IActionResult GET_ABORT_VIEWXLS()
        {
            var list = TempData["GET_ABORT_VIEW"].ToString().XmlToObject<List<Abort_Row>>();
            TempData["GET_ABORT_VIEW"] = list.ObjectToXml();
            return list == null ? null : File(CreateAbortXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Аборты от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
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




        [HttpGet]
        public IActionResult ECOView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GET_EKO_VIEW(DateTime DT)
        {
            var model = new ECOViewModel
            {
                ECO_MTR = myOracleSet.GetEKO_MTR(DT.Year, DT.Month),
                ECO_MP = myOracleSet.GetEKO_MP(DT.Year, DT.Month)
            };
            TempData["GET_EKO_VIEW"] = model.ObjectToXml();
            return PartialView("_ECOViewPartical", model);
        }
        [HttpGet]
        public IActionResult GET_EKO_VIEWXLS()
        {
            var list = TempData["GET_EKO_VIEW"].ToString().XmlToObject<ECOViewModel>();
            TempData["GET_EKO_VIEW"] = list.ObjectToXml();
            return list == null ? null : File(CreateEKOXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ЭКО от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
        }

        private byte[] CreateEKOXLS(ECOViewModel item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "На территории Заб.края");
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




        [HttpGet]
        public IActionResult KOHLView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GET_KOHL_VIEW(DateTime DT1, DateTime DT2)
        {
            var t = myOracleSet.GetKOHL(DT1, DT2);
            TempData["GET_KOHL_VIEW"] = t.ObjectToXml();
            return PartialView("_KOHLViewPatrical", t);
        }
        [HttpGet]
        public IActionResult GET_KOHL_VIEWXLS()
        {
            var list = TempData["GET_KOHL_VIEW"].ToString().XmlToObject<List<KOHL_Row>>();
            TempData["GET_KOHL_VIEW"] = list.ObjectToXml();
            return list == null ? null : File(CreateKOHLXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Кохлеарная имплантация от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
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




        [HttpGet]
        public IActionResult OKS_ONMKView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GET_OKS_ONMK_VIEW(int YEAR)
        {
            var t = myOracleSet.GetOKS_ONMK(YEAR);
            TempData["GET_OKS_ONMK_VIEW"] = t.ObjectToXml();
            return PartialView("_OKS_ONMKViewPartical", t);
        }
        [HttpGet]
        public IActionResult GET_OKS_ONMK_VIEWXLS()
        {
            var list = TempData["GET_OKS_ONMK_VIEW"].ToString().XmlToObject<List<OKS_ONMK_Row>>();
            TempData["GET_OKS_ONMK_VIEW"] = list.ObjectToXml();
            return list == null ? null : File(CreateOKS_ONMKXLS(list), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ОКС ОНМК от {DateTime.Now:dd-MM-yyyy HH_mm}.xlsx");
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

            var TextLeftBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenterBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var NumberBOLD = ex.CreateType(new FontOpenXML { Bold = true, Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

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
    }
}
