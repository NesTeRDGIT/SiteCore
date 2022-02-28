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
    public interface ITMKExcelCreator
    {
         byte[] GetTMKReestrXLSX(IEnumerable<TMKListModel> items, Dictionary<string, CODE_MO> moSPR, Dictionary<int, TMIS> tmisSPR, Dictionary<int, NMIC> nmisSPR, Dictionary<int, List<F014>> f014SPR);
         byte[] GetReportXLS(List<ReportTMKRow> items, List<Report2TMKRow> items2);
    }
    public class TMKExcelCreator: ITMKExcelCreator
    {
        private string TemplateReestr { get; }
        private string TemplateReport { get; }

        public TMKExcelCreator(string TemplateReestr,string TemplateReport)
        {
            this.TemplateReestr = TemplateReestr;
            this.TemplateReport = TemplateReport;
        }

        public byte[] GetTMKReestrXLSX(IEnumerable<TMKListModel> items, Dictionary<string,CODE_MO> moSPR, Dictionary<int, TMIS> tmisSPR, Dictionary<int, NMIC> nmisSPR, Dictionary<int,List<F014>> f014SPR)
        {
            using var xls = new ExcelOpenXML();
            var template = File.ReadAllBytes(TemplateReestr);
            using var stream = new MemoryStream();
            stream.Write(template);
            xls.OpenFile(stream, 0);
            var dtformat = xls.CreateNumFormatCustom("dd.MM.yyyyy");
            var stylecenter = xls.CreateType(new FontOpenXML() { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterDT = xls.CreateType(new FontOpenXML() { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Center, Format = dtformat }, new BorderOpenXML(), null);
            var styleright = xls.CreateType(new FontOpenXML() { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            uint rowIndex = 2;
            var index = 0;
            foreach (var rep in items)
            {
                var r = xls.GetRow(rowIndex);
                index++;
                rowIndex++;

                xls.PrintCell(r, 1, index, styleright);
                xls.PrintCell(r, 2, rep.STATUS.ToString(), stylecenter);
                xls.PrintCell(r, 3, rep.ENP, stylecenter);
                xls.PrintCell(r, 4, rep.CODE_MO, stylecenter);
                var NAM_MOK = moSPR.ContainsKey(rep.CODE_MO) ? moSPR[rep.CODE_MO].NAM_MOK : "";
                xls.PrintCell(r, 5, NAM_MOK, styleright);
                
                xls.PrintCell(r, 6, rep.FIO, styleright);
                xls.PrintCell(r, 7, rep.DATE_B, stylecenterDT);
                xls.PrintCell(r, 8, rep.DATE_QUERY, stylecenterDT);
                xls.PrintCell(r, 9, rep.DATE_PROTOKOL, stylecenterDT);
                var TMIS_NAME = tmisSPR.ContainsKey(rep.TMIS) ? tmisSPR[rep.TMIS].TMS_NAME : "";
                var NMIC_NAME = nmisSPR.ContainsKey(rep.NMIC) ? nmisSPR[rep.NMIC].NMIC_NAME : "";

                xls.PrintCell(r, 10, TMIS_NAME, stylecenter);
                xls.PrintCell(r, 11, NMIC_NAME, styleright);
                xls.PrintCell(r, 12, rep.SMO, stylecenter);
                xls.PrintCell(r, 13, rep.VID_NHISTORY, stylecenter);
                xls.PrintCell(r, 14, rep.OPLATA, stylecenter);

                xls.PrintCell(r, 15, string.Join(",", rep.MEK.Select(x=>x.DATEACT.ToString("dd.MM.yyyy"))), stylecenter);


                xls.PrintCell(r, 16, string.Join(";", rep.MEK.Select(x=> FindF014(x, f014SPR))), stylecenter);
                xls.PrintCell(r, 17, string.Join(",", rep.MEE.Select(x => x.DATEACT.ToString("dd.MM.yyyy"))), stylecenter);
                xls.PrintCell(r, 18, string.Join(";", rep.MEE.Select(x => FindF014(x, f014SPR))), stylecenter);
                xls.PrintCell(r, 19, string.Join(",", rep.EKMP.Select(x => x.DATEACT.ToString("dd.MM.yyyy"))), stylecenter);
                xls.PrintCell(r, 20, string.Join(";", rep.EKMP.Select(x => FindF014(x, f014SPR))), stylecenter);
                xls.PrintCell(r, 21, rep.TMK_ID, stylecenter);
            }
            xls.Save();
            return stream.ToArray();
        }

        private string FindF014(TMKListExpModel exp, Dictionary<int, List<F014>> f014SPR)
        {
            var values = new List<string>();
            foreach(var osn in exp.OSN)
            {
                if(f014SPR.ContainsKey(osn))
                {
                    var list = f014SPR[osn];
                    var item = list.FirstOrDefault(x => x.DATEBEG <= exp.DATEACT && (x.DATEEND >= exp.DATEACT || !x.DATEEND.HasValue));
                    if(item!=null)
                        values.Add(item.FullName);
                    else
                        values.Add("");
                }
                else
                {
                    values.Add("");
                }
            }
            return string.Join(";", values);
        }

        public byte[] GetReportXLS(List<ReportTMKRow> items, List<Report2TMKRow> items2)
        {
            using var xls = new ExcelOpenXML();
            using var stream = new MemoryStream();
            var template = System.IO.File.ReadAllBytes(TemplateReport);
            stream.Write(template, 0, template.Length);
            xls.OpenFile(stream, 0);
            var stylecenter = xls.CreateType(new FontOpenXML() { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterNUM = xls.CreateType(new FontOpenXML() { fontname = "Calibri", Format = (uint)ExcelManager.DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var styleright = xls.CreateType(new FontOpenXML() { fontname = "Calibri", HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);
            var stylecenterBOLD = xls.CreateType(new FontOpenXML() { fontname = "Calibri", Bold = true, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylecenterNUMBOLD = xls.CreateType(new FontOpenXML() { fontname = "Calibri", Bold = true, Format = (uint)ExcelManager.DefaultNumFormat.F4, HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var stylerightBOLD = xls.CreateType(new FontOpenXML() { fontname = "Calibri", Bold = true, HorizontalAlignment = HorizontalAlignmentV.Right }, new BorderOpenXML(), null);

            uint rowIndex = 6;
            MRow r;
            foreach (var rep in items)
            {
                r = xls.GetRow(rowIndex);
                rowIndex++;
                xls.PrintCell(r, 1, rep.SUB, styleright);
                xls.PrintCell(r, 2, rep.SMO, stylecenter);
                xls.PrintCell(r, 3, rep.NAM_SMOK, styleright);
                xls.PrintCell(r, 4, rep.MO, stylecenter);
                xls.PrintCell(r, 5, rep.NAM_MOK, styleright);
                xls.PrintCell(r, 6, rep.NMIC, stylecenter);
                xls.PrintCell(r, 7, rep.C, stylecenter);
                xls.PrintCell(r, 8, rep.C_V, stylecenter);
                xls.PrintCell(r, 9, rep.C_P, stylecenter);
                xls.PrintCell(r, 10, rep.C_MEK_TFOMS, stylecenter);
                xls.PrintCell(r, 11, rep.C_MEE_TFOMS, stylecenter);
                xls.PrintCell(r, 12, rep.C_EKMP_TFOMS, stylecenter);
                xls.PrintCell(r, 13, rep.C_MEK_SMO, stylecenter);
                xls.PrintCell(r, 14, rep.C_MEE_SMO, stylecenter);
                xls.PrintCell(r, 15, rep.C_EKMP_SMO, stylecenter);
                xls.PrintCell(r, 16, rep.C_MEK_D_TFOMS, stylecenter);
                xls.PrintCell(r, 17, rep.C_MEE_D_TFOMS, stylecenter);
                xls.PrintCell(r, 18, rep.C_EKMP_D_TFOMS, stylecenter);
                xls.PrintCell(r, 19, rep.C_MEK_D_SMO, stylecenter);
                xls.PrintCell(r, 20, rep.C_MEE_D_SMO, stylecenter);
                xls.PrintCell(r, 21, rep.C_EKMP_D_SMO, stylecenter);
                xls.PrintCell(r, 22, rep.S_MEK_D_TFOMS, stylecenter);
                xls.PrintCell(r, 23, rep.S_MEE_D_TFOMS, stylecenter);
                xls.PrintCell(r, 24, rep.S_EKMP_D_TFOMS, stylecenter);
                xls.PrintCell(r, 25, rep.S_MEK_D_SMO, stylecenter);
                xls.PrintCell(r, 26, rep.S_MEE_D_SMO, stylecenter);
                xls.PrintCell(r, 27, rep.S_EKMP_D_SMO, stylecenter);
                xls.PrintCell(r, 28, Convert.ToDouble(rep.S_SUM_TFOMS), stylecenterNUM);
                xls.PrintCell(r, 29, Convert.ToDouble(rep.S_FINE_TFOMS), stylecenterNUM);
                xls.PrintCell(r, 30, Convert.ToDouble(rep.S_SUM_SMO), stylecenterNUM);
                xls.PrintCell(r, 31, Convert.ToDouble(rep.S_FINE_SMO), stylecenterNUM);

                xls.PrintCell(r, 32, rep.S_ALL, stylecenter);
                xls.PrintCell(r, 33, rep.S_1_1_3, stylecenter);
                xls.PrintCell(r, 34, rep.S_1_2_2, stylecenter);
                xls.PrintCell(r, 35, rep.S_1_3_2, stylecenter);
                xls.PrintCell(r, 36, rep.S_1_4, stylecenter);
                xls.PrintCell(r, 37, rep.S_3_1, stylecenter);
                xls.PrintCell(r, 38, rep.S_3_2_2, stylecenter);
                xls.PrintCell(r, 39, rep.S_3_2_3, stylecenter);
                xls.PrintCell(r, 40, rep.S_3_2_4, stylecenter);
                xls.PrintCell(r, 41, rep.S_3_2_5, stylecenter);
                xls.PrintCell(r, 42, rep.S_3_2_6, stylecenter);
                xls.PrintCell(r, 43, rep.S_3_3_1, stylecenter);
                xls.PrintCell(r, 44, rep.S_3_4, stylecenter);
                xls.PrintCell(r, 45, rep.S_3_5, stylecenter);
                xls.PrintCell(r, 46, rep.S_3_6, stylecenter);
                xls.PrintCell(r, 47, rep.S_3_7, stylecenter);
                xls.PrintCell(r, 48, rep.S_3_8, stylecenter);
                xls.PrintCell(r, 49, rep.S_3_10, stylecenter);
                xls.PrintCell(r, 50, rep.S_4_2, stylecenter);
                xls.PrintCell(r, 51, rep.S_5_1_3, stylecenter);
                xls.PrintCell(r, 52, rep.S_5_3_1, stylecenter);
                xls.PrintCell(r, 53, rep.S_5_4, stylecenter);
                xls.PrintCell(r, 54, rep.S_5_5, stylecenter);
                xls.PrintCell(r, 55, rep.S_5_6, stylecenter);
                xls.PrintCell(r, 56, rep.S_5_7, stylecenter);
                xls.PrintCell(r, 57, rep.S_5_8, stylecenter);
            }
            if (items.Count > 1)
            {
                r = xls.GetRow(rowIndex);
                xls.PrintCell(r, 1, "Всего", stylerightBOLD);
                xls.PrintCell(r, 2, "", stylecenterBOLD);
                xls.PrintCell(r, 3, "", stylerightBOLD);
                xls.PrintCell(r, 4, "", stylecenterBOLD);
                xls.PrintCell(r, 5, "", stylerightBOLD);
                xls.PrintCell(r, 6, "", stylerightBOLD);
                xls.PrintCell(r, 7, items.Sum(x => x.C), stylecenterBOLD);
                xls.PrintCell(r, 8, items.Sum(x => x.C_V), stylecenterBOLD);
                xls.PrintCell(r, 9, items.Sum(x => x.C_P), stylecenterBOLD);
                xls.PrintCell(r, 10, items.Sum(x => x.C_MEK_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 11, items.Sum(x => x.C_MEE_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 12, items.Sum(x => x.C_EKMP_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 13, items.Sum(x => x.C_MEK_SMO), stylecenterBOLD);
                xls.PrintCell(r, 14, items.Sum(x => x.C_MEE_SMO), stylecenterBOLD);
                xls.PrintCell(r, 15, items.Sum(x => x.C_EKMP_SMO), stylecenterBOLD);
                xls.PrintCell(r, 16, items.Sum(x => x.C_MEK_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 17, items.Sum(x => x.C_MEE_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 18, items.Sum(x => x.C_EKMP_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 19, items.Sum(x => x.C_MEK_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 20, items.Sum(x => x.C_MEE_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 21, items.Sum(x => x.C_EKMP_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 22, items.Sum(x => x.S_MEK_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 23, items.Sum(x => x.S_MEE_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 24, items.Sum(x => x.S_EKMP_D_TFOMS), stylecenterBOLD);
                xls.PrintCell(r, 25, items.Sum(x => x.S_MEK_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 26, items.Sum(x => x.S_MEE_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 27, items.Sum(x => x.S_EKMP_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 28, Convert.ToDouble(items.Sum(x => x.S_SUM_TFOMS)), stylecenterNUMBOLD);
                xls.PrintCell(r, 29, Convert.ToDouble(items.Sum(x => x.S_FINE_TFOMS)), stylecenterNUMBOLD);
                xls.PrintCell(r, 30, Convert.ToDouble(items.Sum(x => x.S_SUM_SMO)), stylecenterNUMBOLD);
                xls.PrintCell(r, 31, Convert.ToDouble(items.Sum(x => x.S_FINE_SMO)), stylecenterNUMBOLD);

                xls.PrintCell(r, 32, Convert.ToDouble(items.Sum(x => x.S_ALL)), stylecenter);
                xls.PrintCell(r, 33, Convert.ToDouble(items.Sum(x => x.S_1_1_3)), stylecenter);
                xls.PrintCell(r, 34, Convert.ToDouble(items.Sum(x => x.S_1_2_2)), stylecenter);
                xls.PrintCell(r, 35, Convert.ToDouble(items.Sum(x => x.S_1_3_2)), stylecenter);
                xls.PrintCell(r, 36, Convert.ToDouble(items.Sum(x => x.S_1_4)), stylecenter);
                xls.PrintCell(r, 37, Convert.ToDouble(items.Sum(x => x.S_3_1)), stylecenter);
                xls.PrintCell(r, 38, Convert.ToDouble(items.Sum(x => x.S_3_2_2)), stylecenter);
                xls.PrintCell(r, 39, Convert.ToDouble(items.Sum(x => x.S_3_2_3)), stylecenter);
                xls.PrintCell(r, 40, Convert.ToDouble(items.Sum(x => x.S_3_2_4)), stylecenter);
                xls.PrintCell(r, 41, Convert.ToDouble(items.Sum(x => x.S_3_2_5)), stylecenter);
                xls.PrintCell(r, 42, Convert.ToDouble(items.Sum(x => x.S_3_2_6)), stylecenter);
                xls.PrintCell(r, 43, Convert.ToDouble(items.Sum(x => x.S_3_3_1)), stylecenter);
                xls.PrintCell(r, 44, Convert.ToDouble(items.Sum(x => x.S_3_4)), stylecenter);
                xls.PrintCell(r, 45, Convert.ToDouble(items.Sum(x => x.S_3_5)), stylecenter);
                xls.PrintCell(r, 46, Convert.ToDouble(items.Sum(x => x.S_3_6)), stylecenter);
                xls.PrintCell(r, 47, Convert.ToDouble(items.Sum(x => x.S_3_7)), stylecenter);
                xls.PrintCell(r, 48, Convert.ToDouble(items.Sum(x => x.S_3_8)), stylecenter);
                xls.PrintCell(r, 49, Convert.ToDouble(items.Sum(x => x.S_3_10)), stylecenter);
                xls.PrintCell(r, 50, Convert.ToDouble(items.Sum(x => x.S_4_2)), stylecenter);
                xls.PrintCell(r, 51, Convert.ToDouble(items.Sum(x => x.S_5_1_3)), stylecenter);
                xls.PrintCell(r, 52, Convert.ToDouble(items.Sum(x => x.S_5_3_1)), stylecenter);
                xls.PrintCell(r, 53, Convert.ToDouble(items.Sum(x => x.S_5_4)), stylecenter);
                xls.PrintCell(r, 54, Convert.ToDouble(items.Sum(x => x.S_5_5)), stylecenter);
                xls.PrintCell(r, 55, Convert.ToDouble(items.Sum(x => x.S_5_6)), stylecenter);
                xls.PrintCell(r, 56, Convert.ToDouble(items.Sum(x => x.S_5_7)), stylecenter);
                xls.PrintCell(r, 57, Convert.ToDouble(items.Sum(x => x.S_5_8)), stylecenter);
            }
            if (items.All(x => string.IsNullOrEmpty(x.SMO)))
            {
                xls.SetColumnVisible("B", false);
                xls.SetColumnVisible("C", false);
            }
            if (items.All(x => string.IsNullOrEmpty(x.MO)))
            {
                xls.SetColumnVisible("D", false);
                xls.SetColumnVisible("E", false);
            }
            if (items.All(x => string.IsNullOrEmpty(x.NMIC)))
            {
                xls.SetColumnVisible("F", false);
            }





            xls.SetCurrentSchet(1);
            rowIndex = 5;

            foreach (var rep in items2)
            {
                r = xls.GetRow(rowIndex);
                rowIndex++;
                xls.PrintCell(r, 1, rep.SUB, styleright);
                xls.PrintCell(r, 2, rep.SMO, stylecenter);
                xls.PrintCell(r, 3, rep.NAM_SMOK, styleright);
                xls.PrintCell(r, 4, rep.MO, stylecenter);
                xls.PrintCell(r, 5, rep.NAM_MOK, styleright);
                xls.PrintCell(r, 6, rep.NMIC, stylecenter);
                xls.PrintCell(r, 7, rep.C, stylecenter);
                xls.PrintCell(r, 8, rep.C_V, stylecenter);
                xls.PrintCell(r, 9, rep.C_P, stylecenter);
               
                xls.PrintCell(r, 10, rep.C_MEE_SMO, stylecenter);
                xls.PrintCell(r, 11, rep.C_EKMP_SMO, stylecenter);
                xls.PrintCell(r, 12, rep.C_EKMP_SMO_PROC, stylecenter);
              
                xls.PrintCell(r, 13, rep.C_MEE_D_SMO, stylecenter);
                xls.PrintCell(r, 14, rep.C_EKMP_D_SMO, stylecenter);
              
                xls.PrintCell(r, 15, rep.S_MEE_D_SMO, stylecenter);
                xls.PrintCell(r, 16, rep.S_EKMP_D_SMO, stylecenter);
           
                xls.PrintCell(r, 17, Convert.ToDouble(rep.S_SUM_SMO), stylecenterNUM);
                xls.PrintCell(r, 18, Convert.ToDouble(rep.S_FINE_SMO), stylecenterNUM);

                xls.PrintCell(r, 19, rep.S_ALL, stylecenter);
                xls.PrintCell(r, 20, rep.S_1_4_3, stylecenter);
                xls.PrintCell(r, 21, rep.S_1_6_1, stylecenter);
                xls.PrintCell(r, 22, rep.S_1_9, stylecenter);
                xls.PrintCell(r, 23, rep.S_1_10, stylecenter);
                xls.PrintCell(r, 24, rep.S_2_1, stylecenter);
                xls.PrintCell(r, 25, rep.S_2_17, stylecenter);
                xls.PrintCell(r, 26, rep.S_3_1_2, stylecenter);
                xls.PrintCell(r, 27, rep.S_3_1_3, stylecenter);
                xls.PrintCell(r, 28, rep.S_3_1_4, stylecenter);
                xls.PrintCell(r, 29, rep.S_3_1_5, stylecenter);
                xls.PrintCell(r, 30, rep.S_3_2_2, stylecenter);
                xls.PrintCell(r, 31, rep.S_3_2_3, stylecenter);
                xls.PrintCell(r, 32, rep.S_3_2_4, stylecenter);
                xls.PrintCell(r, 33, rep.S_3_2_5, stylecenter);
                xls.PrintCell(r, 34, rep.S_3_3, stylecenter);
                xls.PrintCell(r, 35, rep.S_3_4, stylecenter);
                xls.PrintCell(r, 36, rep.S_3_5, stylecenter);
                xls.PrintCell(r, 37, rep.S_3_6, stylecenter);
                xls.PrintCell(r, 38, rep.S_3_8, stylecenter);
                xls.PrintCell(r, 39, rep.S_3_7, stylecenter);
                xls.PrintCell(r, 40, rep.S_3_10, stylecenter);
                xls.PrintCell(r, 41, rep.S_3_11, stylecenter);
                xls.PrintCell(r, 42, rep.S_3_13, stylecenter);
            }
            if (items2.Count > 1)
            {
                r = xls.GetRow(rowIndex);
                xls.PrintCell(r, 1, "Всего", stylerightBOLD);
                xls.PrintCell(r, 2, "", stylecenterBOLD);
                xls.PrintCell(r, 3, "", stylerightBOLD);
                xls.PrintCell(r, 4, "", stylecenterBOLD);
                xls.PrintCell(r, 5, "", stylerightBOLD);
                xls.PrintCell(r, 6, "", stylerightBOLD);
                xls.PrintCell(r, 7, items2.Sum(x => x.C), stylecenterBOLD);
                xls.PrintCell(r, 8, items2.Sum(x => x.C_V), stylecenterBOLD);
                xls.PrintCell(r, 9, items2.Sum(x => x.C_P), stylecenterBOLD);
               
                xls.PrintCell(r, 10, items2.Sum(x => x.C_MEE_SMO), stylecenterBOLD);
                xls.PrintCell(r, 11, items2.Sum(x => x.C_EKMP_SMO), stylecenterBOLD);
                xls.PrintCell(r, 12, "", stylecenterBOLD);

                xls.PrintCell(r, 13, items2.Sum(x => x.C_MEE_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 14, items2.Sum(x => x.C_EKMP_D_SMO), stylecenterBOLD);
                
                xls.PrintCell(r, 15, items2.Sum(x => x.S_MEE_D_SMO), stylecenterBOLD);
                xls.PrintCell(r, 16, items2.Sum(x => x.S_EKMP_D_SMO), stylecenterBOLD);
 
                xls.PrintCell(r, 17, Convert.ToDouble(items2.Sum(x => x.S_SUM_SMO)), stylecenterNUMBOLD);
                xls.PrintCell(r, 18, Convert.ToDouble(items2.Sum(x => x.S_FINE_SMO)), stylecenterNUMBOLD);

                xls.PrintCell(r, 19,items2.Sum(x => x.S_ALL), stylecenter);
                xls.PrintCell(r, 20,items2.Sum(x => x.S_1_4_3), stylecenter);
                xls.PrintCell(r, 21,items2.Sum(x => x.S_1_6_1), stylecenter);
                xls.PrintCell(r, 22,items2.Sum(x => x.S_1_9), stylecenter);
                xls.PrintCell(r, 23,items2.Sum(x => x.S_1_10), stylecenter);
                xls.PrintCell(r, 24,items2.Sum(x => x.S_2_1), stylecenter);
                xls.PrintCell(r, 25,items2.Sum(x => x.S_2_17), stylecenter);
                xls.PrintCell(r, 26,items2.Sum(x => x.S_3_1_2), stylecenter);
                xls.PrintCell(r, 27,items2.Sum(x => x.S_3_1_3), stylecenter);
                xls.PrintCell(r, 28,items2.Sum(x => x.S_3_1_4), stylecenter);
                xls.PrintCell(r, 29,items2.Sum(x => x.S_3_1_5), stylecenter);
                xls.PrintCell(r, 30,items2.Sum(x => x.S_3_2_2), stylecenter);
                xls.PrintCell(r, 31,items2.Sum(x => x.S_3_2_3), stylecenter);
                xls.PrintCell(r, 32,items2.Sum(x => x.S_3_2_4), stylecenter);
                xls.PrintCell(r, 33,items2.Sum(x => x.S_3_2_5), stylecenter);
                xls.PrintCell(r, 34,items2.Sum(x => x.S_3_3), stylecenter);
                xls.PrintCell(r, 35,items2.Sum(x => x.S_3_4), stylecenter);
                xls.PrintCell(r, 36,items2.Sum(x => x.S_3_5), stylecenter);
                xls.PrintCell(r, 37,items2.Sum(x => x.S_3_6), stylecenter);
                xls.PrintCell(r, 38,items2.Sum(x => x.S_3_8), stylecenter);
                xls.PrintCell(r, 39,items2.Sum(x => x.S_3_7), stylecenter);
                xls.PrintCell(r, 40,items2.Sum(x => x.S_3_10), stylecenter);
                xls.PrintCell(r, 41,items2.Sum(x => x.S_3_11), stylecenter);
                xls.PrintCell(r, 42,items2.Sum(x => x.S_3_13), stylecenter);
            }
            if (items2.All(x => string.IsNullOrEmpty(x.SMO)))
            {
                xls.SetColumnVisible("B", false);
                xls.SetColumnVisible("C", false);
            }
            if (items2.All(x => string.IsNullOrEmpty(x.MO)))
            {
                xls.SetColumnVisible("D", false);
                xls.SetColumnVisible("E", false);
            }
            if (items2.All(x => string.IsNullOrEmpty(x.NMIC)))
            {
                xls.SetColumnVisible("F", false);
            }
            xls.Save();
            return stream.ToArray();
        }
    }
}
