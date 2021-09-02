using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;
using SiteCore.Data;
using SiteCore.Models;

namespace SiteCore.Class
{
    public interface IMSEExcelCreator
    {
        byte[] GetReportXLS(List<ReportMSERow> items);
        byte[] GetMSEReestrXLSX(List<MSE_ITEMModel> items);
    }

    public class MSEExcelCreator: IMSEExcelCreator
    {
        private string TemplateReport { get; }
        private string TemplateMSEReestr { get; }
        public MSEExcelCreator(string TemplateReport, string TemplateMSEReestr)
        {
            this.TemplateReport = TemplateReport;
            this.TemplateMSEReestr = TemplateMSEReestr;
        }
      
        public byte[] GetReportXLS(List<ReportMSERow> items)
        {
            using var xls = new ExcelOpenXML();
            using var stream = new MemoryStream();
            var template = File.ReadAllBytes(TemplateReport);
            stream.Write(template, 0, template.Length);

            xls.OpenFile(stream, 0);
            var stylecenter = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterNUM = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var styleright = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var stylecenterBOLD = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterNUMBOLD = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true, Format = (uint)DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylerightBOLD = xls.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            uint rowIndex = 8;
            MRow r;
            foreach (var rep in items)
            {
                r = xls.GetRow(rowIndex);
                rowIndex++;
                xls.PrintCell(r, 1, rep.SUB, styleright);
                xls.PrintCell(r, 2, rep.SMO, stylecenter);
                xls.PrintCell(r, 3, rep.NAM_SMOK, styleright);
                xls.PrintCell(r, 4, "", stylecenter);
                xls.PrintCell(r, 5, "", stylecenter);
                xls.PrintCell(r, 6, "", stylecenter);
                xls.PrintCell(r, 7, "", stylecenter);
                xls.PrintCell(r, 8, "", stylecenter);
                xls.PrintCell(r, 9, "", stylecenter);
                xls.PrintCell(r, 10, rep.ST8, stylecenter);
                xls.PrintCell(r, 11, rep.ST9, stylecenter);
                xls.PrintCell(r, 12, rep.ST10, stylecenter);
                xls.PrintCell(r, 13, rep.ST11, stylecenterNUM);
                xls.PrintCell(r, 14, rep.ST12, stylecenterNUM);
                xls.PrintCell(r, 15, "", stylecenter);
                xls.PrintCell(r, 16, "", stylecenter);
                xls.PrintCell(r, 17, "", stylecenter);
                xls.PrintCell(r, 18, rep.ST16, stylecenter);
                xls.PrintCell(r, 19, rep.ST17, stylecenter);
                xls.PrintCell(r, 20, rep.ST18, stylecenter);
                xls.PrintCell(r, 21, rep.ST19, stylecenterNUM);
                xls.PrintCell(r, 22, rep.ST20, stylecenterNUM);
                xls.PrintCell(r, 23, rep.S_ALL, stylecenter);
                xls.PrintCell(r, 24, rep.S_1_1_3, stylecenter);
                xls.PrintCell(r, 25, rep.S_1_1_4, stylecenter);
                xls.PrintCell(r, 26, rep.S_1_2_2, stylecenter);
                xls.PrintCell(r, 27, rep.S_1_3_2, stylecenter);
                xls.PrintCell(r, 28, rep.S_1_4, stylecenter);
                xls.PrintCell(r, 29, rep.S_2_2_1, stylecenter);
                xls.PrintCell(r, 30, rep.S_2_2_2, stylecenter);
                xls.PrintCell(r, 31, rep.S_2_4_1, stylecenter);
                xls.PrintCell(r, 32, rep.S_2_4_2, stylecenter);
                xls.PrintCell(r, 33, rep.S_3_1, stylecenter);
                xls.PrintCell(r, 34, rep.S_3_2_2, stylecenter);
                xls.PrintCell(r, 35, rep.S_3_2_3, stylecenter);
                xls.PrintCell(r, 36, rep.S_3_2_4, stylecenter);
                xls.PrintCell(r, 37, rep.S_3_2_5, stylecenter);
                xls.PrintCell(r, 38, rep.S_3_3_1, stylecenter);
                xls.PrintCell(r, 39, rep.S_3_5, stylecenter);
                xls.PrintCell(r, 40, rep.S_3_6, stylecenter);
                xls.PrintCell(r, 41, rep.S_3_9, stylecenter);
                xls.PrintCell(r, 42, rep.S_4_1, stylecenter);
                xls.PrintCell(r, 43, rep.S_4_2, stylecenter);
                xls.PrintCell(r, 44, rep.S_4_4, stylecenter);
                xls.PrintCell(r, 45, rep.S_4_5, stylecenter);
                xls.PrintCell(r, 46, rep.S_4_6_1, stylecenter);
                xls.PrintCell(r, 47, rep.S_4_6_2, stylecenter);
                xls.PrintCell(r, 48, rep.S_5_3_1, stylecenter);
                xls.PrintCell(r, 49, rep.S_5_4_1, stylecenter);
                xls.PrintCell(r, 50, rep.S_5_4_2, stylecenter);
                xls.PrintCell(r, 51, rep.S_5_5, stylecenter);
                xls.PrintCell(r, 52, rep.S_5_6, stylecenter);
                xls.PrintCell(r, 53, rep.S_5_7, stylecenter);

            }

            if (items.Count > 1)
            {
                r = xls.GetRow(rowIndex);
                xls.PrintCell(r, 1, "Всего", stylerightBOLD);
                xls.PrintCell(r, 2, "", stylecenterBOLD);
                xls.PrintCell(r, 3, "", stylerightBOLD);
                xls.PrintCell(r, 4, "", stylecenterBOLD);
                xls.PrintCell(r, 5, "", stylecenterBOLD);
                xls.PrintCell(r, 6, "", stylecenterBOLD);
                xls.PrintCell(r, 7, "", stylecenterBOLD);
                xls.PrintCell(r, 8, "", stylecenterBOLD);
                xls.PrintCell(r, 9, "", stylecenterBOLD);
                xls.PrintCell(r, 10, items.Sum(x => x.ST8), stylecenterBOLD);
                xls.PrintCell(r, 11, items.Sum(x => x.ST9), stylecenterBOLD);
                xls.PrintCell(r, 12, items.Sum(x => x.ST10), stylecenterBOLD);
                xls.PrintCell(r, 13, items.Sum(x => x.ST11), stylecenterNUMBOLD);
                xls.PrintCell(r, 14, items.Sum(x => x.ST12), stylecenterNUMBOLD);
                xls.PrintCell(r, 15, "", stylecenterBOLD);
                xls.PrintCell(r, 16, "", stylecenterBOLD);
                xls.PrintCell(r, 17, "", stylecenterBOLD);
                xls.PrintCell(r, 18, items.Sum(x => x.ST16), stylecenterBOLD);
                xls.PrintCell(r, 19, items.Sum(x => x.ST17), stylecenterBOLD);
                xls.PrintCell(r, 20, items.Sum(x => x.ST18), stylecenterBOLD);
                xls.PrintCell(r, 21, items.Sum(x => x.ST19), stylecenterNUMBOLD);
                xls.PrintCell(r, 22, items.Sum(x => x.ST20), stylecenterNUMBOLD);
                xls.PrintCell(r, 23, items.Sum(x => x.S_ALL), stylecenterBOLD);
                xls.PrintCell(r, 24, items.Sum(x => x.S_1_1_3), stylecenterBOLD);
                xls.PrintCell(r, 25, items.Sum(x => x.S_1_1_4), stylecenterBOLD);
                xls.PrintCell(r, 26, items.Sum(x => x.S_1_2_2), stylecenterBOLD);
                xls.PrintCell(r, 27, items.Sum(x => x.S_1_3_2), stylecenterBOLD);
                xls.PrintCell(r, 28, items.Sum(x => x.S_1_4), stylecenterBOLD);
                xls.PrintCell(r, 29, items.Sum(x => x.S_2_2_1), stylecenterBOLD);
                xls.PrintCell(r, 30, items.Sum(x => x.S_2_2_2), stylecenterBOLD);
                xls.PrintCell(r, 31, items.Sum(x => x.S_2_4_1), stylecenterBOLD);
                xls.PrintCell(r, 32, items.Sum(x => x.S_2_4_2), stylecenterBOLD);
                xls.PrintCell(r, 33, items.Sum(x => x.S_3_1), stylecenterBOLD);
                xls.PrintCell(r, 34, items.Sum(x => x.S_3_2_2), stylecenterBOLD);
                xls.PrintCell(r, 35, items.Sum(x => x.S_3_2_3), stylecenterBOLD);
                xls.PrintCell(r, 36, items.Sum(x => x.S_3_2_4), stylecenterBOLD);
                xls.PrintCell(r, 37, items.Sum(x => x.S_3_2_5), stylecenterBOLD);
                xls.PrintCell(r, 38, items.Sum(x => x.S_3_3_1), stylecenterBOLD);
                xls.PrintCell(r, 39, items.Sum(x => x.S_3_5), stylecenterBOLD);
                xls.PrintCell(r, 40, items.Sum(x => x.S_3_6), stylecenterBOLD);
                xls.PrintCell(r, 41, items.Sum(x => x.S_3_9), stylecenterBOLD);
                xls.PrintCell(r, 42, items.Sum(x => x.S_4_1), stylecenterBOLD);
                xls.PrintCell(r, 43, items.Sum(x => x.S_4_2), stylecenterBOLD);
                xls.PrintCell(r, 44, items.Sum(x => x.S_4_4), stylecenterBOLD);
                xls.PrintCell(r, 45, items.Sum(x => x.S_4_5), stylecenterBOLD);
                xls.PrintCell(r, 46, items.Sum(x => x.S_4_6_1), stylecenterBOLD);
                xls.PrintCell(r, 47, items.Sum(x => x.S_4_6_2), stylecenterBOLD);
                xls.PrintCell(r, 48, items.Sum(x => x.S_5_3_1), stylecenterBOLD);
                xls.PrintCell(r, 49, items.Sum(x => x.S_5_4_1), stylecenterBOLD);
                xls.PrintCell(r, 50, items.Sum(x => x.S_5_4_2), stylecenterBOLD);
                xls.PrintCell(r, 51, items.Sum(x => x.S_5_5), stylecenterBOLD);
                xls.PrintCell(r, 52, items.Sum(x => x.S_5_6), stylecenterBOLD);
                xls.PrintCell(r, 53, items.Sum(x => x.S_5_7), stylecenterBOLD);
            }

            if (items.All(x => string.IsNullOrEmpty(x.SMO)))
            {
                xls.SetColumnVisible("B", false);
                xls.SetColumnVisible("C", false);
            }
            xls.Save();
            return stream.ToArray();
        }
        public byte[] GetMSEReestrXLSX(List<MSE_ITEMModel> items)
        {
            using var xls = new ExcelOpenXML();
            using var stream = new MemoryStream();
            var template = File.ReadAllBytes(TemplateMSEReestr);
            stream.Write(template, 0, template.Length);


            xls.OpenFile(stream, 0);
            var dtformat = xls.CreateNumFormatCustom("dd.MM.yyyy");
            var stylecenter = xls.CreateType(new FontOpenXML { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterDT = xls.CreateType(new FontOpenXML { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Center, Format = dtformat }, new BorderOpenXML(), null);
            var styleright = xls.CreateType(new FontOpenXML { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            uint rowIndex = 2;
            var index = 0;
            MRow r;
            foreach (var rep in items)
            {
                r = xls.GetRow(rowIndex);
                index++;
                rowIndex++;
                uint c = 1;
                xls.PrintCell(r, c, index, styleright); c++;
                xls.PrintCell(r, c, rep.ENP, stylecenter); c++;
                xls.PrintCell(r, c, rep.NAM_MOK, styleright); c++;
                xls.PrintCell(r, c, rep.FIO, styleright); c++;
                xls.PrintCell(r, c, rep.SNILS, stylecenter); c++;
                xls.PrintCell(r, c, rep.N_PROT, stylecenter); c++;
                xls.PrintCell(r, c, rep.D_PROT, stylecenterDT); c++;
                xls.PrintCell(r, c, rep.D_FORM, stylecenterDT); c++;

                xls.PrintCell(r, c, rep.REASON_R, stylecenter); c++;
                xls.PrintCell(r, c, rep.PRNAME, stylecenter); c++;
                xls.PrintCell(r, c, rep.DS_NAME, stylecenter); c++;

                xls.PrintCell(r, c, rep.SMO, styleright); c++;
                xls.PrintCell(r, c, rep.DATE_MEK, stylecenter); c++;
                xls.PrintCell(r, c, rep.DEF_MEK, stylecenter); c++;
                xls.PrintCell(r, c, rep.DATE_MEE, stylecenter); c++;
                xls.PrintCell(r, c, rep.DEF_MEE, stylecenter); c++;
                xls.PrintCell(r, c, rep.DATE_EKMP, stylecenter); c++;
                xls.PrintCell(r, c, rep.DEF_EKMP , stylecenter); c++;
                xls.PrintCell(r, c, rep.MSE_ID, stylecenter); c++;
            }
            xls.Save();
            return stream.ToArray();
        }
    }
}
