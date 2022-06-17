using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using ExcelManager;
using SiteCore.Data;
using Color = System.Drawing.Color;

namespace SiteCore.Class
{
    public interface IOMSXLSCreator
    {
        public static string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public byte[] CreateCURRENT_VMP_OOMSXLS(IEnumerable<CURRENT_VMP_OOMS2> item);
        public byte[] CreateVMPPeriodXLS(IEnumerable<VMP_OOMS> item);
        public byte[] CreateAbortXLS(IEnumerable<Abort_Row> item);
        public byte[] CreateECOXLS(ECO_RECORD item);
        public byte[] CreateKOHLXLS(List<KOHL_Row> items);
        public byte[] CreateOKS_ONMKXLS(List<OKS_ONMK_Row> items);
        public byte[] CreateSmpXLS(List<SMPRow> data);
        public byte[] CreatePensXLS(List<PENSRow> data);
        public byte[] CreateDispXLS(DispRecord data);
        public byte[] CreateKv2MtrXLS(IEnumerable<Kv2MtrRow> data);
        public byte[] CreateDLIXLS(DliRecord data);
        public byte[] CreateKSGXLS(IEnumerable<KSG_Row> item);

    }
    public class OMSXLSCreator: IOMSXLSCreator
    {
        private readonly string DispReportTemplate;
        private readonly string Kv2MtrTemplate;
        private readonly string DliReportTemplate;


        public OMSXLSCreator(string dispReportTemplate,string Kv2MtrTemplate,string DliReportTemplate)
        {
            DispReportTemplate = dispReportTemplate;
            this.Kv2MtrTemplate = Kv2MtrTemplate;
            this.DliReportTemplate = DliReportTemplate;
        }
        public byte[] CreateCURRENT_VMP_OOMSXLS(IEnumerable<CURRENT_VMP_OOMS2> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;
            var bold = ex.CreateType(new FontOpenXML { Bold = true }, new BorderOpenXML(), null);
            var Text = ex.CreateType(new FontOpenXML(), new BorderOpenXML(), null);
     
            

            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace }, new BorderOpenXML(), null);
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
                ex.PrintCell(r, "M", row.SUMM, Number);
                rowindex++;
            }
            ex.AutoSizeColumns(1,13);
            ex.Save();
            return ms.ToArray();
        }
        public byte[] CreateVMPPeriodXLS(IEnumerable<VMP_OOMS> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");

            var boldHeadStyle = ex.CreateType(new FontOpenXML { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center, VerticalAlignment = VerticalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextCenterStyle = ex.CreateType(new FontOpenXML { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextRightStyle = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

          
            
            var FloatStyle = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);


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
                ex.PrintCell(rowindex, 10, row.SPOLIS, TextRightStyle);
                ex.PrintCell(rowindex, 11, row.NPOLIS, TextRightStyle);
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
            ex.AutoSizeColumns(1, 19);
            ex.Save();
            return ms.ToArray();
        }
        public byte[] CreateAbortXLS(IEnumerable<Abort_Row> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;

        
      

            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            var TextLeftBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenterBOLD = ex.CreateType(new FontOpenXML() { Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var NumberBOLD = ex.CreateType(new FontOpenXML { Bold = true, Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

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
        public byte[] CreateECOXLS(ECO_RECORD item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "На территории Заб.края");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

         

            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
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
        public byte[] CreateKOHLXLS(List<KOHL_Row> items)
        {
            var ms = new MemoryStream();
            var ex = new ExcelOpenXML(ms, "Кохл");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

          

            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var TextCenterDate = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center, Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);

       

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
        public byte[] CreateOKS_ONMKXLS(List<OKS_ONMK_Row> items)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Кохл");
            uint rowindex = 1;
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextRight = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

          


            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
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
        public byte[] CreateSmpXLS(List<SMPRow> data)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");

            var boldHeadNotBorder = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, null, null);
            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

            var floatFormat = ex.CreateNumFormatCustom("# ##0,00");
            var intFormat = ex.CreateNumFormatCustom("# ##0");

            var FloatStyle = ex.CreateType(new FontOpenXML { Format = floatFormat, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var IntStyle = ex.CreateType(new FontOpenXML { Format = intFormat, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);


            uint rowindex = 1;


            ex.PrintCell(rowindex, 1, "Сведения об объемах и стоимости скорой медицинской помощи", boldHeadNotBorder); ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex, 6));
            rowindex += 2;


            ex.PrintCell(rowindex, 1, "Показатель", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex + 1, 1));
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
            ex.SetColumnWidth(1, 76);
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
        public byte[] CreatePensXLS(List<PENSRow> data)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
         
            var intFormat = ex.CreateNumFormatCustom("# ##0");

            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);

           
            var IntStyle = ex.CreateType(new FontOpenXML { Format = intFormat, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);


            uint rowindex = 1;

            ex.PrintCell(rowindex, 1, "Код МО", boldHead); ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex + 2, 1));
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

        public byte[] CreateDispXLS(DispRecord data)
        {
            using var ms = new MemoryStream();
            var templateBytes = System.IO.File.ReadAllBytes(DispReportTemplate);
            ms.Write(templateBytes, 0, templateBytes.Length);
            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms,0);
            
            var textLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left, fontname = "Times New Roman", size = 10 }, new BorderOpenXML(), null);
            var floatStyle = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 10 }, new BorderOpenXML(), null);
            var intStyle = ex.CreateType(new FontOpenXML { Format = NumFormat.IntAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 10 }, new BorderOpenXML(), null);


            uint rowIndex = 8;

            foreach (var row in data.DispVzr)
            {
                ex.PrintCell(rowIndex, 1, row.SMO, textLeft);
                ex.PrintCell(rowIndex, 2, row.POK, textLeft);
                ex.PrintCell(rowIndex, 3, row.ST3, floatStyle);
                ex.PrintCell(rowIndex, 4, row.ST4, intStyle);
                ex.PrintCell(rowIndex, 5, row.ST5, intStyle);
                ex.PrintCell(rowIndex, 6, row.ST6, floatStyle);
                ex.PrintCell(rowIndex, 7, row.ST7, intStyle);
                ex.PrintCell(rowIndex, 8, row.ST8, intStyle);
                ex.PrintCell(rowIndex, 9, row.ST9, floatStyle);
                ex.PrintCell(rowIndex, 10, row.ST10, intStyle);
                ex.PrintCell(rowIndex, 11, row.ST11, floatStyle);
                ex.PrintCell(rowIndex, 12, row.ST12, floatStyle);
                ex.PrintCell(rowIndex, 13, row.ST13, intStyle);
                ex.PrintCell(rowIndex, 14, row.ST14, intStyle);
                ex.PrintCell(rowIndex, 15, row.ST15, intStyle);
                ex.PrintCell(rowIndex, 16, row.ST16, floatStyle);
                ex.PrintCell(rowIndex, 17, row.ST17, intStyle);
                ex.PrintCell(rowIndex, 18, row.ST18, intStyle);
                ex.PrintCell(rowIndex, 19, row.ST19, intStyle);
                ex.PrintCell(rowIndex, 20, row.ST20, floatStyle);
                ex.PrintCell(rowIndex, 21, row.ST21, intStyle);
                ex.PrintCell(rowIndex, 22, row.ST22, floatStyle);
                ex.PrintCell(rowIndex, 23, row.ST23, intStyle);
                ex.PrintCell(rowIndex, 24, row.ST24, intStyle);
                ex.PrintCell(rowIndex, 25, row.ST25, intStyle);
                ex.PrintCell(rowIndex, 26, row.ST26, intStyle);
                ex.PrintCell(rowIndex, 27, row.ST27, intStyle);
                ex.PrintCell(rowIndex, 28, row.ST28, intStyle);
                ex.PrintCell(rowIndex, 29, row.ST29, intStyle);
                ex.PrintCell(rowIndex, 30, row.ST30, intStyle);
                ex.PrintCell(rowIndex, 31, row.ST31, intStyle);
                ex.PrintCell(rowIndex, 32, row.ST32, intStyle);
                ex.PrintCell(rowIndex, 33, row.ST33, intStyle);
                ex.PrintCell(rowIndex, 34, row.ST34, intStyle);
                ex.PrintCell(rowIndex, 35, row.ST35, intStyle);
                ex.PrintCell(rowIndex, 36, row.ST36, intStyle);
                ex.PrintCell(rowIndex, 37, row.ST37, intStyle);
                ex.PrintCell(rowIndex, 38, row.ST38, intStyle);
                rowIndex++;
            }
            ex.SetCurrentSchet(1);
            rowIndex = 7;
            foreach (var row in data.DispDet)
            {
                ex.PrintCell(rowIndex, 1, row.SMO, textLeft);
                ex.PrintCell(rowIndex, 2, row.POK, textLeft);
                ex.PrintCell(rowIndex, 3, row.SUMV, floatStyle);
                ex.PrintCell(rowIndex, 4, row.KOL, intStyle);
                ex.PrintCell(rowIndex, 5, row.SUM, floatStyle);
                ex.PrintCell(rowIndex, 6, row.KOL_VBR, intStyle);
                ex.PrintCell(rowIndex, 7, row.SUM_VBR, floatStyle);
                ex.PrintCell(rowIndex, 8, row.SUMP, floatStyle);
                ex.PrintCell(rowIndex, 9, row.KOL_P, intStyle);
                ex.PrintCell(rowIndex, 10, row.SUM_P, floatStyle);
                ex.PrintCell(rowIndex, 11, row.KOL_VBR_P, intStyle);
                ex.PrintCell(rowIndex, 12, row.SUM_VBR_P, floatStyle);
                ex.PrintCell(rowIndex, 13, row.C_GRP_1, intStyle);
                ex.PrintCell(rowIndex, 14, row.C_GRP_2, intStyle);
                ex.PrintCell(rowIndex, 15, row.C_GRP_3, intStyle);
                ex.PrintCell(rowIndex, 16, row.C_GRP_4, intStyle);
                ex.PrintCell(rowIndex, 17, row.C_GRP_5, intStyle);

                rowIndex++;
            }
            ex.SetCurrentSchet(2);
            rowIndex = 6;
            foreach (var row in data.ProfVzr)
            {
                ex.PrintCell(rowIndex, 1, row.SMO, textLeft);
                ex.PrintCell(rowIndex, 2, row.NAM_SMO, textLeft);
                ex.PrintCell(rowIndex, 3, row.NN, textLeft);
                ex.PrintCell(rowIndex, 4, row.GRP, textLeft);
                ex.PrintCell(rowIndex, 5, row.KOL, intStyle);
                ex.PrintCell(rowIndex, 6, row.SUM, floatStyle);
                ex.PrintCell(rowIndex, 7, row.KOL_P, intStyle);
                ex.PrintCell(rowIndex, 8, row.SUM_P, floatStyle);
                rowIndex++;
            }
            ex.SetCurrentSchet(3);
            rowIndex = 7;

            foreach (var row in data.ProfDet)
            {
                ex.PrintCell(rowIndex, 1, row.SMO, textLeft);
                ex.PrintCell(rowIndex, 2, row.ST2, floatStyle);
                ex.PrintCell(rowIndex, 3, row.ST3, intStyle);
                ex.PrintCell(rowIndex, 4, row.ST4, intStyle);
                ex.PrintCell(rowIndex, 5, row.ST5, floatStyle);
                ex.PrintCell(rowIndex, 6, row.ST6, intStyle);
                ex.PrintCell(rowIndex, 7, row.ST7, floatStyle);
                ex.PrintCell(rowIndex, 8, row.ST8, intStyle);
                ex.PrintCell(rowIndex, 9, row.ST9, intStyle);
                ex.PrintCell(rowIndex, 10, row.ST10, floatStyle);
                ex.PrintCell(rowIndex, 11, row.ST11, intStyle);
                ex.PrintCell(rowIndex, 12, row.ST12, floatStyle);
                ex.PrintCell(rowIndex, 13, row.ST13, floatStyle);
                ex.PrintCell(rowIndex, 14, row.ST14, intStyle);
                ex.PrintCell(rowIndex, 15, row.ST15, intStyle);
                ex.PrintCell(rowIndex, 16, row.ST16, floatStyle);
                ex.PrintCell(rowIndex, 17, row.ST17, intStyle);
                ex.PrintCell(rowIndex, 18, row.ST18, floatStyle);
                ex.PrintCell(rowIndex, 19, row.ST19, intStyle);
                ex.PrintCell(rowIndex, 20, row.ST20, intStyle);
                ex.PrintCell(rowIndex, 21, row.ST21, floatStyle);
                ex.PrintCell(rowIndex, 22, row.ST22, intStyle);
                ex.PrintCell(rowIndex, 23, row.ST23, floatStyle);
                ex.PrintCell(rowIndex, 24, row.ST24, intStyle);
                ex.PrintCell(rowIndex, 25, row.ST25, intStyle);
                ex.PrintCell(rowIndex, 26, row.ST26, intStyle);
                ex.PrintCell(rowIndex, 27, row.ST27, intStyle);
                ex.PrintCell(rowIndex, 28, row.ST28, intStyle);
                rowIndex++;
          
                
            }
            ex.Save();
            return ms.ToArray();
        }

        public byte[] CreateKv2MtrXLS(IEnumerable<Kv2MtrRow> data)
        {
            using var ms = new MemoryStream();
            var templateBytes = System.IO.File.ReadAllBytes(Kv2MtrTemplate);
            ms.Write(templateBytes, 0, templateBytes.Length);
            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);

      
            uint rowIndex = 6;

            foreach (var row in data)
            {
                ex.PrintCell(rowIndex, 4, row.KOL, null);
                ex.PrintCell(rowIndex, 5, row.SUM, null);
                rowIndex++;
            }
            ex.Save();
            return ms.ToArray();
        }

        public byte[] CreateDLIXLS(DliRecord data)
        {
            using var ms = new MemoryStream();
            var templateBytes = System.IO.File.ReadAllBytes(DliReportTemplate);
            ms.Write(templateBytes, 0, templateBytes.Length);
            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);
           
            var textLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left, fontname = "Times New Roman", size = 9 }, new BorderOpenXML(), null);
            var floatStyle = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 9 }, new BorderOpenXML(), null);
            var intStyle = ex.CreateType(new FontOpenXML { Format = NumFormat.IntAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 9 }, new BorderOpenXML(), null);
            var floatStyleGrey = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 9 }, new BorderOpenXML(), new FillOpenXML { color = Color.Silver });
            var intStyleGrey = ex.CreateType(new FontOpenXML { Format = NumFormat.IntAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, fontname = "Times New Roman", size = 9 }, new BorderOpenXML(), new FillOpenXML{color = Color.Silver});

            uint rowIndex = 5;
            int i = 1;
            foreach (var row in data.tbl1)
            {
                ex.PrintCell(rowIndex, 1, row.SMO, textLeft);
                ex.PrintCell(rowIndex, 2, row.NAME, textLeft);
                ex.PrintCell(rowIndex, 3, i, textLeft);
                ex.PrintCell(rowIndex, 4, 0, intStyleGrey);
                ex.PrintCell(rowIndex, 5, 0, intStyleGrey);
                ex.PrintCell(rowIndex, 6, 0, floatStyleGrey);
                ex.PrintCell(rowIndex, 7, 0, floatStyleGrey);
                ex.PrintCell(rowIndex, 8, row.K, intStyle);
                ex.PrintCell(rowIndex, 9, row.ENP, intStyle);
                ex.PrintCell(rowIndex, 10, row.S, floatStyle);
                ex.PrintCell(rowIndex, 11, row.K_MTR, intStyle);
                ex.PrintCell(rowIndex, 12, row.ENP_MTR, intStyle);
                ex.PrintCell(rowIndex, 13, row.S_MTR, floatStyle);
                rowIndex++;
                i++;
            }
            ex.SetCurrentSchet(1);
            rowIndex = 4;
            foreach (var row in data.tbl2)
            {
                ex.PrintCell(rowIndex, 1, row.NAME_TFK, textLeft);
                ex.PrintCell(rowIndex, 2, row.OKRUG, textLeft);
                
                ex.PrintCell(rowIndex, 3, row.C_KT, intStyle);
                ex.PrintCell(rowIndex, 4, row.E_KT, intStyle);
                ex.PrintCell(rowIndex, 5, row.S_KT, floatStyle);

                ex.PrintCell(rowIndex, 6, row.C_MRT, intStyle);
                ex.PrintCell(rowIndex, 7, row.E_MRT, intStyle);
                ex.PrintCell(rowIndex, 8, row.S_MRT, floatStyle);

                ex.PrintCell(rowIndex, 9, row.C_USI, intStyle);
                ex.PrintCell(rowIndex, 10, row.E_USI, intStyle);
                ex.PrintCell(rowIndex, 11, row.S_USI, floatStyle);

                ex.PrintCell(rowIndex, 12, row.C_ENDO, intStyle);
                ex.PrintCell(rowIndex, 13, row.E_ENDO, intStyle);
                ex.PrintCell(rowIndex, 14, row.S_ENDO, floatStyle);

                ex.PrintCell(rowIndex, 15, row.C_MOL, intStyle);
                ex.PrintCell(rowIndex, 16, row.E_MOL, intStyle);
                ex.PrintCell(rowIndex, 17, row.S_MOL, floatStyle);

                ex.PrintCell(rowIndex, 18, row.C_GIST, intStyle);
                ex.PrintCell(rowIndex, 19, row.E_GIST, intStyle);
                ex.PrintCell(rowIndex, 20, row.S_GIST, floatStyle);

                rowIndex++;
            }
            
            ex.Save();
            return ms.ToArray();
        }

        public byte[] CreateKSGXLS(IEnumerable<KSG_Row> item)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;

            var boldHead = ex.CreateType(new FontOpenXML { Bold = true, wordwrap = true, HorizontalAlignment = HorizontalAlignmentV.Center, color = Color.DarkBlue }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Left, color = Color.DarkBlue }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML() { HorizontalAlignment = HorizontalAlignmentV.Center, color = Color.DarkBlue }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { Format = NumFormat.FloatAndSpace, HorizontalAlignment = HorizontalAlignmentV.Right, color = Color.DarkBlue }, new BorderOpenXML(), null);

            var r = ex.GetRow(rowindex, true);
            ex.PrintCell(r, 1, "Год", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 1, rowindex + 1, 1));
            ex.PrintCell(r, 2, "Месяц", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 2, rowindex + 1, 2));
            ex.PrintCell(r, 3, "Код МО", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 3, rowindex + 1, 3));
            ex.PrintCell(r, 4, "Наименование МО", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 4, rowindex + 1, 4));
            ex.PrintCell(r, 5, "Условия оказания", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 5, rowindex + 1, 5));
            ex.PrintCell(r, 6, "КСГ", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 6, rowindex + 1, 6));
            ex.PrintCell(r, 7, "Наименование", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 7, rowindex + 1, 7));
            ex.PrintCell(r, 8, "Профиль", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 8, rowindex + 1, 8));
            ex.PrintCell(r, 9, "Наименование", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 9, rowindex + 1, 9));
            ex.PrintCell(r, 10, "Профиль", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 10, rowindex, 11));
            ex.PrintCell(r, 12, "Принято", boldHead);
            ex.AddMergedRegion(new CellRangeAddress(rowindex, 12, rowindex, 13));
            r = ex.GetRow(++rowindex, true);
            ex.PrintCell(r, 10, "Кол-во", boldHead);
            ex.PrintCell(r, 11, "Сумма", boldHead);
            ex.PrintCell(r, 12, "Кол-во", boldHead);
            ex.PrintCell(r, 13, "Сумма", boldHead);

            rowindex++;
            foreach (var row in item)
            {
                r = ex.GetRow(rowindex, true);
                var tl = TextLeft;
                
                ex.PrintCell(r, 1, row.Year, TextCenter);
                ex.PrintCell(r, 2, row.Month, TextCenter);
                ex.PrintCell(r, 3, row.Code_MO, TextCenter);
                ex.PrintCell(r, 4, row.Nam_MOK, TextCenter);
                ex.PrintCell(r, 5, row.Usl_OK, TextCenter);
                ex.PrintCell(r, 6, row.N_KSG, TextCenter);
                ex.PrintCell(r, 7, row.Name_KSG, TextLeft);
                ex.PrintCell(r, 8, row.Id_Profil, TextCenter);
                ex.PrintCell(r, 9, row.Name, TextLeft);
                ex.PrintCell(r, 10, row.C, Number);
                ex.PrintCell(r, 11, row.S, Number);
                ex.PrintCell(r, 12, row.C_P, Number);
                ex.PrintCell(r, 13, row.S_P, Number);

                rowindex++;
            }

            ex.AutoSizeColumns(1, 6);
            ex.Save();
            return ms.ToArray();
        }
    }
}
