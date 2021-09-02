using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelManager;


namespace SiteCore.Class
{
    public interface ISTAC_PLANExcelCreator
    {
        byte[] CreateXLS_TABLE_1(DataTable tbl);
        byte[] CreateXLS_TABLE_2(DataTable tbl);
        byte[] CreateXLS_TABLE_3(DataTable tbl);
        byte[] CreateXLS_TABLE_4(DataTable tbl);
        DataTable PIVOT_TABLE_2(DataTable tbl);
    }

    public class STAC_PLANExcelCreator: ISTAC_PLANExcelCreator
    {
        private string TemplateTable1;
        private string TemplateTable3;
        private string TemplateTable4;

        public STAC_PLANExcelCreator(string TemplateTable1, string TemplateTable3, string TemplateTable4)
        {
            this.TemplateTable1 = TemplateTable1;
            this.TemplateTable3 = TemplateTable3;
            this.TemplateTable4 = TemplateTable4;
        }

        public byte[] CreateXLS_TABLE_1(DataTable tbl)
        {
            using var ms = new MemoryStream();
            var templateBytes = File.ReadAllBytes(TemplateTable1);
            ms.Write(templateBytes, 0, templateBytes.Length);


            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);

            uint rowindex = 6;
            var Text = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", }, new BorderOpenXML(), null);

            foreach (DataRow row in tbl.Rows)
            {

                var r = ex.GetRow(rowindex);
                ex.PrintCell(r, 1, row["NAM_MOK"].ToString(), Text);
                ex.PrintCell(r, 2, Convert.ToDouble(row["st1"]), Text);
                ex.PrintCell(r, 3, Convert.ToDouble(row["st2"]), Text);
                ex.PrintCell(r, 4, Convert.ToDouble(row["st3"]), Text);
                ex.PrintCell(r, 5, Convert.ToDouble(row["st4"]), Text);
                ex.PrintCell(r, 6, Convert.ToDouble(row["st5"]), Text);
                ex.PrintCell(r, 7, Convert.ToDouble(row["st6"]), Text);
                ex.PrintCell(r, 8, Convert.ToDouble(row["st7"]), Text);
                ex.PrintCell(r, 9, Convert.ToDouble(row["st8"]), Text);
                ex.PrintCell(r, 10, Convert.ToDouble(row["st9"]), Text);
                ex.PrintCell(r, 11, Convert.ToDouble(row["st10"]), Text);
                ex.PrintCell(r, 12, Convert.ToDouble(row["st11"]), Text);
                ex.PrintCell(r, 13, Convert.ToDouble(row["st12"]), Text);
                ex.PrintCell(r, 14, Convert.ToDouble(row["st13"]), Text);
                ex.PrintCell(r, 15, Convert.ToDouble(row["st14"]), Text);
                ex.PrintCell(r, 16, Convert.ToDouble(row["st15"]), Text);
                ex.PrintCell(r, 17, Convert.ToDouble(row["st16"]), Text);
                rowindex++;
            }

            ex.Save();
            return ms.ToArray();
        }
        public byte[] CreateXLS_TABLE_2(DataTable tbl)
        {
            using var ms = new MemoryStream();
            using var ex = new ExcelOpenXML(ms, "Лист1");
            uint rowindex = 1;
            var bold = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", Bold = true }, new BorderOpenXML(), null);
            var Text = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", }, new BorderOpenXML(), null);

            uint colindex = 1;
            var r = ex.GetRow(rowindex, false);
            foreach (DataColumn col in tbl.Columns)
            {
                ex.PrintCell(r, colindex, col.Caption, bold);
                colindex++;
            }

            rowindex++;
            foreach (DataRow row in tbl.Rows)
            {
                r = ex.GetRow(rowindex, false);
                colindex = 1;
                foreach (DataColumn col in tbl.Columns)
                {
                    if (col.DataType == typeof(string))
                    {
                        ex.PrintCell(r, colindex, row[col].ToString(), Text);
                    }
                    else
                    {
                        if (row[col] == DBNull.Value)
                        {
                            ex.PrintCell(r, colindex, row[col].ToString(), Text);
                        }
                        else
                        {
                            ex.PrintCell(r, colindex, Convert.ToDouble(row[col]), Text);
                        }
                    }

                    colindex++;
                }

                rowindex++;
            }

            ex.Save();
            return ms.ToArray();
        }
        public DataTable PIVOT_TABLE_2(DataTable tbl)
        {
            var comm = "";
            if (tbl.Columns.Contains("COMM") && tbl.Rows.Count > 0)
            {
                comm = tbl.Rows[0]["COMM"].ToString();
            }
            var newtbl = new DataTable();
            var distinctProfil = tbl.DefaultView.ToTable(true, "NAME_PROFIL", "ORDER_PROFIL");
            var distinctMO = tbl.DefaultView.ToTable(true, "NAME_MO", "CODE_MO");
            var OrderMO = distinctMO.Select().OrderBy(x => x["CODE_MO"]);
            var OrderProfil = distinctProfil.Select().OrderBy(x => x["ORDER_PROFIL"]);
            newtbl.Columns.Add(new DataColumn("NAME_PROFIL") { DataType = typeof(string), Caption = "Профиль" });
            newtbl.Columns.Add(new DataColumn("COMM") { DataType = typeof(string), Caption = "Профиль", ColumnMapping = MappingType.Hidden });
            foreach (var row in OrderMO)
            {
                newtbl.Columns.Add(new DataColumn(row["code_mo"].ToString()) { DataType = typeof(decimal), Caption = row["NAME_MO"].ToString() });
            }

            foreach (var row in OrderProfil)
            {
                var PROFIL = row["NAME_PROFIL"].ToString();
                var r = tbl.Select("NAME_PROFIL = '" + PROFIL + "'");
                var newrow = newtbl.NewRow();
                newrow["NAME_PROFIL"] = PROFIL;
                foreach (var rr in r)
                {
                    newrow[rr["code_mo"].ToString()] = rr["count"];
                    newrow["COMM"] = comm;
                }

                foreach (DataColumn c in newtbl.Columns)
                {
                    if (newrow[c] == DBNull.Value)
                    {
                        newrow[c] = 0;
                    }
                }

                newtbl.Rows.Add(newrow);
            }

            return newtbl;
        }
        public byte[] CreateXLS_TABLE_3(DataTable tbl)
        {
            using var ms = new MemoryStream();
            var templateBytes = File.ReadAllBytes(TemplateTable3);
            ms.Write(templateBytes, 0, templateBytes.Length);

            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);

            uint rowindex = 6;
            var Text = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", }, new BorderOpenXML(), null);

            foreach (DataRow row in tbl.Rows)
            {
                var r = ex.GetRow(rowindex);
                ex.PrintCell(r, 1, row["PROFIL"].ToString(), Text);
                ex.PrintCell(r, 2, Convert.ToDouble(row["st1"]), Text);
                ex.PrintCell(r, 3, Convert.ToDouble(row["st2"]), Text);
                ex.PrintCell(r, 4, Convert.ToDouble(row["st3"]), Text);
                ex.PrintCell(r, 5, Convert.ToDouble(row["st4"]), Text);
                ex.PrintCell(r, 6, Convert.ToDouble(row["st5"]), Text);
                ex.PrintCell(r, 7, Convert.ToDouble(row["st6"]), Text);
                ex.PrintCell(r, 8, Convert.ToDouble(row["st7"]), Text);
                ex.PrintCell(r, 9, Convert.ToDouble(row["st8"]), Text);
                ex.PrintCell(r, 10, Convert.ToDouble(row["st9"]), Text);
                ex.PrintCell(r, 11, Convert.ToDouble(row["st10"]), Text);
                ex.PrintCell(r, 12, Convert.ToDouble(row["st11"]), Text);
                ex.PrintCell(r, 13, Convert.ToDouble(row["st12"]), Text);
                ex.PrintCell(r, 14, Convert.ToDouble(row["st13"]), Text);
                ex.PrintCell(r, 15, Convert.ToDouble(row["st14"]), Text);
                ex.PrintCell(r, 16, Convert.ToDouble(row["st15"]), Text);
                ex.PrintCell(r, 17, Convert.ToDouble(row["st16"]), Text);
                ex.PrintCell(r, 18, Convert.ToDouble(row["st17"]), Text);
                ex.PrintCell(r, 19, Convert.ToDouble(row["st18"]), Text);
                ex.PrintCell(r, 20, Convert.ToDouble(row["st19"]), Text);
                ex.PrintCell(r, 21, Convert.ToDouble(row["st20"]), Text);
                rowindex++;
            }
            ex.Save();
            return ms.ToArray();
        }
        public byte[] CreateXLS_TABLE_4(DataTable tbl)
        {
            using var ms = new MemoryStream();
            var templateBytes = File.ReadAllBytes(TemplateTable4);
            ms.Write(templateBytes, 0, templateBytes.Length);

            using var ex = new ExcelOpenXML();
            ex.OpenFile(ms, 0);
            uint rowindex = 6;
            var Text = ex.CreateType(new FontOpenXML { fontname = "Times New Roman", }, new BorderOpenXML(), null);

            foreach (DataRow row in tbl.Rows)
            {
                var r = ex.GetRow(rowindex);
                ex.PrintCell(r, 1, row["NAME"].ToString(), Text);
                ex.PrintCell(r, 2, Convert.ToDouble(row["st1"]), Text);
                ex.PrintCell(r, 3, Convert.ToDouble(row["st2"]), Text);
                ex.PrintCell(r, 4, Convert.ToDouble(row["st3"]), Text);
                ex.PrintCell(r, 5, Convert.ToDouble(row["st4"]), Text);
                ex.PrintCell(r, 6, Convert.ToDouble(row["st5"]), Text);
                ex.PrintCell(r, 7, Convert.ToDouble(row["st6"]), Text);
                rowindex++;
            }
            ex.Save();
            return ms.ToArray();
        }
    }
}
