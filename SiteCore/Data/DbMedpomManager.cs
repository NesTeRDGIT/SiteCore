using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace SiteCore.Data
{
    public class DbMedpomManager
    {
        private string ConnectionString = "";
        public DbMedpomManager(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        private const string Z_SL_view = "v_z_sluch";
        private const string SL_view = "V_SLUCH";
        private const string DS2_view = "V_DS2";
        private const string DS3_view = "V_DS3";
        private const string DS2_N_view = "V_DS2_N";
        private const string NAZR_view = "V_NAZR";
        private const string NAPR_view = "V_NAPR";
        private const string CONS_view = "V_CONS";
        private const string CRIT_view = "V_CRIT";
        private const string SL_KOEF_view = "V_koef";
        private const string ONK_USL_view = "V_ONK_USL";
        private const string B_PROT_view = "V_B_PROT";
        private const string B_DIAG_view = "V_B_DIAG";
        private const string LEK_PR_view = "V_LEK_PR";
        private const string v_USL = "v_USL";

        public Z_SL FindZ_SL(int SLUCH_Z_ID)
        {
            using (var conn = new OracleConnection(ConnectionString))
            {
                using (var oda = new OracleDataAdapter($"select * from {Z_SL_view} where SLUCH_Z_ID = {SLUCH_Z_ID}", conn))
                {
                    var Z_SLtbl = new DataTable();
                    oda.Fill(Z_SLtbl);
                    if (Z_SLtbl.Rows.Count == 0) throw new Exception("Не удалось найти случай");
                    var item = Z_SL.Get(Z_SLtbl.Rows[0]);
                    var SLtbl = new DataTable();
                    oda.SelectCommand.CommandText = $"select * from {SL_view} where SLUCH_Z_ID = { item.SLUCH_Z_ID}";
                    oda.Fill(SLtbl);

                    foreach (DataRow SLrow in SLtbl.Rows)
                    {
                        var SLUCH_ID = Convert.ToInt32(SLrow["SLUCH_ID"]);
                        var DS2tbl = new DataTable();
                        var DS3tbl = new DataTable();
                        var DS2_Ntbl = new DataTable();
                        var NAZRtbl = new DataTable();
                        var NAPRtbl = new DataTable();
                        var CONStbl = new DataTable();
                        var CRITtbl = new DataTable();
                        var SL_KOEFtbl = new DataTable();
                        var ONK_USLtbl = new DataTable();
                        var B_DIAGtbl = new DataTable();
                        var B_PROTtbl = new DataTable();
                        var LEKPRtbl = new DataTable();
                        var USLTbl = new DataTable();
                        oda.SelectCommand.CommandText = $"select * from {DS2_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(DS2tbl);
                        oda.SelectCommand.CommandText = $"select * from {DS3_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(DS3tbl);
                        oda.SelectCommand.CommandText = $"select * from {DS2_N_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(DS2_Ntbl);
                        oda.SelectCommand.CommandText = $"select * from {NAZR_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(NAZRtbl);
                        oda.SelectCommand.CommandText = $"select * from {NAPR_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(NAPRtbl);
                        oda.SelectCommand.CommandText = $"select * from {CONS_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(CONStbl);
                        oda.SelectCommand.CommandText = $"select * from {CRIT_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(CRITtbl);
                        oda.SelectCommand.CommandText = $"select * from {SL_KOEF_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(SL_KOEFtbl);
                        oda.SelectCommand.CommandText = $"select * from {ONK_USL_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(ONK_USLtbl);
                        oda.SelectCommand.CommandText = $"select * from {B_DIAG_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(B_DIAGtbl);
                        oda.SelectCommand.CommandText = $"select * from {B_PROT_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(B_PROTtbl);
                        oda.SelectCommand.CommandText = $"select * from {LEK_PR_view} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(LEKPRtbl);
                        oda.SelectCommand.CommandText = $"select * from {v_USL} where SLUCH_ID = {SLUCH_ID}";
                        oda.Fill(USLTbl);


                        var sl = SL.Get(SLrow, DS2tbl.Select(), DS3tbl.Select(), DS2_Ntbl.Select(), NAZRtbl.Select(),
                            NAPRtbl.Select(), CONStbl.Select(), CRITtbl.Select(), SL_KOEFtbl.Select());
                        sl.USL.AddRange(USLTbl.Select().Select(USL.Get));

                        item.SL.Add(sl);
                        if (sl.ONK_SL != null)
                        {
                            sl.ONK_SL.B_DIAG.AddRange(B_DIAGtbl.Select().Select(B_DIAG.Get));
                            sl.ONK_SL.B_PROT.AddRange(B_PROTtbl.Select().Select(B_PROT.Get));
                            sl.ONK_SL.ONK_USL.AddRange(ONK_USLtbl.Select().Select(x => ONK_USL.Get(x, LEKPRtbl.Select($"ONK_USL_ID = {x["ONK_USL_ID"]}"))));
                        }
                    }

                    return item;
                }
            }


        }
    }
}
