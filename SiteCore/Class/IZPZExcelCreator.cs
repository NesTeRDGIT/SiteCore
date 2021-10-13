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
    public interface IZPZExcelCreator
    {
        byte[] CreateXLSEFFECTIVENESS(List<ZPZ_EFFECTIVENESS> items);
        byte[] CreateXLSResultControl(ResultControl item);
        
    }

    public class ZPZExcelCreator : IZPZExcelCreator
    {
        private string TemplateEFFECTIVENESS { get; }
        private string TemplateResultControl { get; }
        public ZPZExcelCreator(string TemplateEFFECTIVENESS,string TemplateResultControl)
        {
            this.TemplateEFFECTIVENESS = TemplateEFFECTIVENESS;
            this.TemplateResultControl = TemplateResultControl;
        }

        public byte[] CreateXLSEFFECTIVENESS(List<ZPZ_EFFECTIVENESS> item)
        {
            using var ms = new MemoryStream();
            var templateBytes = System.IO.File.ReadAllBytes(TemplateEFFECTIVENESS);
            ms.Write(templateBytes, 0, templateBytes.Length);
            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);
            ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true }, new BorderOpenXML(), null);
            var Text = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", }, new BorderOpenXML(), null);
            var Number = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F4 }, new BorderOpenXML(), null);
            ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);


            uint rowindex = 10;
            var i = 0;
            foreach (var row in item)
            {
                i++;
                var r = ex.GetRow(rowindex, false);
                ex.PrintCell(r, "A", i, Text);
                ex.PrintCell(r, "B", row.NAM_MOK, Text);
                ex.PrintCell(r, "C", "+", Text);
                ex.PrintCell(r, "D", Convert.ToDouble(row.COUNT), Text);
                ex.PrintCell(r, "E", Convert.ToDouble(row.COUNT_OSN), Text);
                ex.PrintCell(r, "F", Convert.ToDouble(row.DOL_OSN), Number);
                ex.PrintCell(r, "G", Convert.ToDouble(row.BAL_OSN), Text);
                ex.PrintCell(r, "H", Convert.ToDouble(row.C_MEE), Text);
                ex.PrintCell(r, "I", Convert.ToDouble(row.C_MEE_ERR), Text);
                ex.PrintCell(r, "J", Convert.ToDouble(row.DOL_MEE), Number);
                ex.PrintCell(r, "K", Convert.ToDouble(row.BAL_MEE), Text);
                ex.PrintCell(r, "L", Convert.ToDouble(row.C_EKMP), Text);
                ex.PrintCell(r, "M", Convert.ToDouble(row.C_EKMP_ERR), Text);
                ex.PrintCell(r, "N", Convert.ToDouble(row.DOL_EKMP), Number);
                ex.PrintCell(r, "O", Convert.ToDouble(row.BAL_EKMP), Text);
                ex.PrintCell(r, "P", Convert.ToDouble(row.SUM_BAL), Text);
                rowindex++;
            }
            ex.Save();
            return ms.ToArray();
        }

        public byte[] CreateXLSResultControl(ResultControl item)
        {
            using var ms = new MemoryStream();
            var templateBytes = System.IO.File.ReadAllBytes(TemplateResultControl);
            ms.Write(templateBytes, 0, templateBytes.Length);
            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);
            ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true }, new BorderOpenXML(), null);
            var TextCenter = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", HorizontalAlignment = HorizontalAlignmentV.Center }, new BorderOpenXML(), null);
            var TextLeft = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", HorizontalAlignment = HorizontalAlignmentV.Left}, new BorderOpenXML(), null);
            var Number2 = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F4 }, new BorderOpenXML(), null);
            var Number0 = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F1 }, new BorderOpenXML(), null);
            ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Format = (uint)DefaultNumFormat.F14 }, new BorderOpenXML(), null);


            uint rowindex = 7;
          
            foreach (var row in item.VZR)
            {
                var r = ex.GetRow(rowindex, false);
                ex.PrintCell(r, 1,  row.NN, TextCenter);
                ex.PrintCell(r, 2, row.DIAG, TextLeft);
                ex.PrintCell(r, 3, row.MKB, TextLeft);
                ex.PrintCell(r, 4, row.ST4, Number0);
                ex.PrintCell(r, 5, row.ST5, Number0);
                ex.PrintCell(r, 6, row.ST6, Number0);
                ex.PrintCell(r, 7, row.ST7, Number0);
                ex.PrintCell(r, 8, row.ST8, Number0);
                ex.PrintCell(r, 9, row.ST9, Number0);
                ex.PrintCell(r, 10, row.ST10, Number0);
                ex.PrintCell(r, 11, row.ST11, Number2);
                ex.PrintCell(r, 12, row.ST12, Number2);
                ex.PrintCell(r, 13, row.ST14, Number0);
                ex.PrintCell(r, 14, row.ST14, Number0);
                ex.PrintCell(r, 15, row.ST15, Number0);
                ex.PrintCell(r, 16, row.ST16, Number0);
                ex.PrintCell(r, 17, row.ST17, Number0);
                ex.PrintCell(r, 18, row.ST18, Number0);
                ex.PrintCell(r, 19, row.ST19, Number0);
                ex.PrintCell(r, 20, row.ST20, Number0);
                ex.PrintCell(r, 21, row.ST21, Number0);
                ex.PrintCell(r, 22, row.ST22, Number0);
                ex.PrintCell(r, 23, row.ST23, Number0);
                ex.PrintCell(r, 24, row.ST24, Number0);
                ex.PrintCell(r, 25, row.ST25, Number0);
                ex.PrintCell(r, 26, row.ST26, Number0);
                ex.PrintCell(r, 27, row.ST27, Number0);
                ex.PrintCell(r, 28, row.ST28, Number0);
                ex.PrintCell(r, 29, row.ST29, Number0);
                ex.PrintCell(r, 30, row.ST30, Number0);
                ex.PrintCell(r, 31, row.ST31, Number0);
                ex.PrintCell(r, 32, row.ST32, Number0);
                ex.PrintCell(r, 33, row.ST33, Number0);
                ex.PrintCell(r, 34, row.ST34, Number0);
                ex.PrintCell(r, 35, row.ST35, Number0);
                ex.PrintCell(r, 36, row.ST36, Number0);
                ex.PrintCell(r, 37, row.ST37, Number0);
                ex.PrintCell(r, 38, row.ST38, Number0);
                ex.PrintCell(r, 39, row.ST39, Number0);
                ex.PrintCell(r, 40, row.ST40, Number0);
                ex.PrintCell(r, 41, row.ST41, Number0);
                ex.PrintCell(r, 42, row.ST42, Number0);
                rowindex++;
            }


            ex.SetCurrentSchet(1);


            rowindex = 7;

            foreach (var row in item.DET)
            {
                var r = ex.GetRow(rowindex, false);
                ex.PrintCell(r, 1, row.NN, TextCenter);
                ex.PrintCell(r, 2, row.DIAG, TextLeft);
                ex.PrintCell(r, 3, row.MKB, TextLeft);
                ex.PrintCell(r, 4, row.ST4, Number0);
                ex.PrintCell(r, 5, row.ST5, Number0);
                ex.PrintCell(r, 6, row.ST6, Number0);
                ex.PrintCell(r, 7, row.ST7, Number0);
                ex.PrintCell(r, 8, row.ST8, Number0);
                ex.PrintCell(r, 9, row.ST9, Number0);
                ex.PrintCell(r, 10, row.ST10, Number2);
                ex.PrintCell(r, 11, row.ST11, Number2);
                ex.PrintCell(r, 12, row.ST12, Number0);
                ex.PrintCell(r, 13, row.ST14, Number0);
                ex.PrintCell(r, 14, row.ST14, Number0);
                ex.PrintCell(r, 15, row.ST15, Number0);
                ex.PrintCell(r, 16, row.ST16, Number0);
                ex.PrintCell(r, 17, row.ST17, Number0);
                ex.PrintCell(r, 18, row.ST18, Number0);
                ex.PrintCell(r, 19, row.ST19, Number0);
                ex.PrintCell(r, 20, row.ST20, Number0);
                ex.PrintCell(r, 21, row.ST21, Number0);
                ex.PrintCell(r, 22, row.ST22, Number0);
                ex.PrintCell(r, 23, row.ST23, Number0);
                ex.PrintCell(r, 24, row.ST24, Number0);
                ex.PrintCell(r, 25, row.ST25, Number0);
                ex.PrintCell(r, 26, row.ST26, Number0);
                ex.PrintCell(r, 27, row.ST27, Number0);
                ex.PrintCell(r, 28, row.ST28, Number0);
                ex.PrintCell(r, 29, row.ST29, Number0);
                ex.PrintCell(r, 30, row.ST30, Number0);
                ex.PrintCell(r, 31, row.ST31, Number0);
                ex.PrintCell(r, 32, row.ST32, Number0);
                ex.PrintCell(r, 33, row.ST33, Number0);
                ex.PrintCell(r, 34, row.ST34, Number0);
                ex.PrintCell(r, 35, row.ST35, Number0);
                ex.PrintCell(r, 36, row.ST36, Number0);
                ex.PrintCell(r, 37, row.ST37, Number0);
                ex.PrintCell(r, 38, row.ST38, Number0);
                ex.PrintCell(r, 39, row.ST39, Number0);
                rowindex++;
            }

            ex.Save();
            return ms.ToArray();
        }
    }
}
