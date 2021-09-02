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
    }

    public class ZPZExcelCreator : IZPZExcelCreator
    {
        private string TemplateEFFECTIVENESS { get; }
        public ZPZExcelCreator(string TemplateEFFECTIVENESS)
        {
            this.TemplateEFFECTIVENESS = TemplateEFFECTIVENESS;
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
    }
}
