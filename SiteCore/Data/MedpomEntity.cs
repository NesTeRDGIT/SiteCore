using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SiteCore.Data
{
    public class Z_SL
    {
        public static Z_SL Get(DataRow row)
        {
            try
            {
                var item = new Z_SL();
                item.PAC = PACIENT.Get(row);
                item.SC = SCHET.Get(row);
                item.SLUCH_Z_ID = Convert.ToInt32(row["SLUCH_Z_ID"]);
                item.ZAP_ID = Convert.ToInt32(row["ZAP_ID"]);
                if (row["USL_OK"] != DBNull.Value)
                    item.USL_OK = Convert.ToInt32(row["USL_OK"]);
                item.N_USL_OK = Convert.ToString(row["N_USL_OK"]);
                if (row["VIDPOM"] != DBNull.Value)
                    item.VIDPOM = Convert.ToInt32(row["VIDPOM"]);
                item.N_VIDPOM = Convert.ToString(row["N_VIDPOM"]);
                if (row["USL_OK"] != DBNull.Value)
                    item.FOR_POM = Convert.ToInt32(row["FOR_POM"]);
                item.N_FOR_POM = Convert.ToString(row["N_FOR_POM"]);

                item.NPR_MO = Convert.ToString(row["NPR_MO"]);
                item.N_NPR_MO = Convert.ToString(row["N_NPR_MO"]);

                if (row["DATE_Z_1"] != DBNull.Value)
                    item.DATE_Z_1 = Convert.ToDateTime(row["DATE_Z_1"]);
                if (row["DATE_Z_2"] != DBNull.Value)
                    item.DATE_Z_2 = Convert.ToDateTime(row["DATE_Z_2"]);

                if (row["RSLT_D"] != DBNull.Value)
                    item.RSLT_D = Convert.ToInt32(row["RSLT_D"]);
                item.N_RSLT_D = Convert.ToString(row["N_RSLT_D"]);

                if (row["RSLT"] != DBNull.Value)
                    item.RSLT = Convert.ToInt32(row["RSLT"]);
                item.N_RSLT = Convert.ToString(row["N_RSLT"]);


                if (row["ISHOD"] != DBNull.Value)
                    item.ISHOD = Convert.ToInt32(row["ISHOD"]);
                item.N_ISHOD = Convert.ToString(row["N_ISHOD"]);

                if (row["IDSP"] != DBNull.Value)
                    item.IDSP = Convert.ToInt32(row["IDSP"]);
                item.N_IDSP = Convert.ToString(row["N_IDSP"]);

                if (row["OPLATA"] != DBNull.Value)
                    item.OPLATA = Convert.ToInt32(row["OPLATA"]);
                item.N_OPLATA = Convert.ToString(row["N_OPLATA"]);
                item.SUMV = Convert.ToDecimal(row["SUMV"]);
                if (row["SUMP"] != DBNull.Value)
                    item.SUMP = Convert.ToDecimal(row["SUMP"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения Z_SL:" + ex.Message);
            }
        }

        public SCHET SC = new SCHET();
        public PACIENT PAC = new PACIENT();
        public int SLUCH_Z_ID { get; set; }
        public int ZAP_ID { get; set; }
        public int? USL_OK { get; set; }
        public string N_USL_OK { get; set; }
        public int? VIDPOM { get; set; }
        public string N_VIDPOM { get; set; }
        public int? FOR_POM { get; set; }
        public string N_FOR_POM { get; set; }
        public string NPR_MO { get; set; }
        public string N_NPR_MO { get; set; }
        public DateTime? NPR_DATE { get; set; }
        public DateTime DATE_Z_1 { get; set; }
        public DateTime DATE_Z_2 { get; set; }
        public int? RSLT_D { get; set; }
        public string N_RSLT_D { get; set; }
        public int? RSLT { get; set; }
        public string N_RSLT { get; set; }
        public int? ISHOD { get; set; }
        public string N_ISHOD { get; set; }
        public List<SL> SL { get; set; } = new List<SL>();
        public int IDSP { get; set; }
        public string N_IDSP { get; set; }
        public decimal SUMV { get; set; }
        public int? OPLATA { get; set; }
        public string N_OPLATA { get; set; }
        public decimal? SUMP { get; set; }
        public List<SANK> SANK { get; set; } = new List<SANK>();

    }
    public class SCHET
    {
        public static SCHET Get(DataRow row)
        {
            try
            {
                var item = new SCHET();
                item.SCHET_ID = Convert.ToInt32(row["SCHET_ID"]);

                item.DOP_FLAG = Convert.ToBoolean(row["DOP_FLAG"]);
                item.ZGLV_ID = Convert.ToInt32(row["ZGLV_ID"]);
                item.CODE_MO = row["CODE_MO"].ToString();
                item.N_CODE_MO = row["N_CODE_MO"].ToString();
                item.YEAR = Convert.ToInt32(row["YEAR"]);
                item.MONTH = Convert.ToInt32(row["MONTH"]);
                item.NSCHET = row["NSCHET"].ToString();
                item.DSCHET = Convert.ToDateTime(row["DSCHET"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения SCHET:" + ex.Message);
            }
        }
        public int SCHET_ID { get; set; }
        public bool DOP_FLAG { get; set; }
        public int ZGLV_ID { get; set; }
        public string CODE_MO { get; set; }
        public string N_CODE_MO { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public string NSCHET { get; set; }
        public DateTime DSCHET { get; set; }

    }
    public class PACIENT
    {
        public static PACIENT Get(DataRow row)
        {
            try
            {
                var item = new PACIENT();
                item.PACIENT_ID = Convert.ToInt32(row["PACIENT_ID"]);
                item.PERS_ID = Convert.ToInt32(row["PERS_ID"]);
                item.ZAP_ID = Convert.ToInt32(row["ZAP_ID"]);
                item.ENP = Convert.ToString(row["ENP"]);
                item.FAM = Convert.ToString(row["FAM"]);
                item.IM = Convert.ToString(row["IM"]);
                item.OT = Convert.ToString(row["OT"]);
                item.W = Convert.ToInt32(row["W"]);
                item.DR = Convert.ToDateTime(row["DR"]);

                item.TEL = Convert.ToString(row["TEL"]);
                item.FAM_P = Convert.ToString(row["FAM_P"]);
                item.IM_P = Convert.ToString(row["IM_P"]);
                item.OT_P = Convert.ToString(row["OT_P"]);
                if (row["W_P"] != DBNull.Value)
                    item.W_P = Convert.ToInt32(row["W_P"]);
                if (row["DR_P"] != DBNull.Value)
                    item.DR_P = Convert.ToDateTime(row["DR_P"]);
                if (row["DDEATH"] != DBNull.Value)
                    item.DDEATH = Convert.ToDateTime(row["DDEATH"]);
                item.SMO = Convert.ToString(row["SMO"]);
                item.N_SMO = Convert.ToString(row["N_SMO"]);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения PACIENT:" + ex.Message);
            }
        }
        public int PACIENT_ID { get; set; }
        public int PERS_ID { get; set; }
        public int ZAP_ID { get; set; }
        public string ENP { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public decimal W { get; set; }
        public DateTime DR { get; set; }
        public string TEL { get; set; }
        public string FAM_P { get; set; }
        public string IM_P { get; set; }
        public string OT_P { get; set; }
        public int? W_P { get; set; }
        public DateTime? DR_P { get; set; }
        public DateTime? DDEATH { get; set; }
        public string SMO { get; set; }
        public string N_SMO { get; set; }
    }
    public class DSX
    {
        public DSX(string MKB, string N_MKB)
        {
            this.MKB = MKB;
            this.N_MKB = N_MKB;
        }
        public string MKB { get; set; }
        public string N_MKB { get; set; }
    }
    public class SL
    {
        public static SL Get(DataRow row, IEnumerable<DataRow> DS2tbl, IEnumerable<DataRow> DS3tbl, IEnumerable<DataRow> DS2_Ntbl, IEnumerable<DataRow> NAZRtbl, IEnumerable<DataRow> NAPRtbl, IEnumerable<DataRow> CONStbl, IEnumerable<DataRow> CRITtbl, IEnumerable<DataRow> SL_KOEFtbl)
        {
            try
            {
                var item = new SL();

                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.SLUCH_Z_ID = Convert.ToInt32(row["SLUCH_Z_ID"]);
                item.VID_HMP = Convert.ToString(row["VID_HMP"]);
                item.N_VID_HMP = Convert.ToString(row["N_VID_HMP"]);

                if (row["METOD_HMP"] != DBNull.Value)
                    item.METOD_HMP = Convert.ToInt32(row["METOD_HMP"]);
                item.N_METOD_HMP = Convert.ToString(row["N_METOD_HMP"]);

                item.LPU_1 = Convert.ToString(row["LPU_1"]);
                item.N_LPU_1 = Convert.ToString(row["N_LPU_1"]);
                if (row["PODR"] != DBNull.Value)
                    item.PODR = Convert.ToInt32(row["PODR"]);
                if (row["PROFIL"] != DBNull.Value)
                    item.PROFIL = Convert.ToInt32(row["PROFIL"]);

                item.N_PROFIL = Convert.ToString(row["N_PROFIL"]);

                if (row["PROFIL_K"] != DBNull.Value)
                    item.PROFIL_K = Convert.ToInt32(row["PROFIL_K"]);

                item.N_PROFIL_K = Convert.ToString(row["N_PROFIL_K"]);

                item.P_CEL = Convert.ToString(row["P_CEL"]);
                item.N_P_CEL = Convert.ToString(row["N_P_CEL"]);
                if (row["TAL_D"] != DBNull.Value)
                    item.TAL_D = Convert.ToDateTime(row["TAL_D"]);
                item.TAL_NUM = Convert.ToString(row["TAL_NUM"]);
                if (row["TAL_P"] != DBNull.Value)
                    item.TAL_P = Convert.ToDateTime(row["TAL_P"]);

                item.NHISTORY = Convert.ToString(row["NHISTORY"]);
                item.DATE_1 = Convert.ToDateTime(row["DATE_1"]);
                item.DATE_2 = Convert.ToDateTime(row["DATE_2"]);

                if (row["DS0"] != DBNull.Value)
                    item.DS0 = new DSX(row["DS0"].ToString(), row["N_DS0"].ToString());
                if (row["DS1"] != DBNull.Value)
                    item.DS1 = new DSX(row["DS1"].ToString(), row["N_DS1"].ToString());

                item.DS2.AddRange(DS2tbl.Select(x => new DSX(x["DS2"].ToString(), x["N_DS2"].ToString())));
                item.DS3.AddRange(DS3tbl.Select(x => new DSX(x["DS3"].ToString(), x["N_DS3"].ToString())));

                if (row["C_ZAB"] != DBNull.Value)
                    item.C_ZAB = Convert.ToInt32(row["C_ZAB"]);
                item.N_C_ZAB = Convert.ToString(row["N_C_ZAB"]);

                item.DS1_PR = Convert.ToBoolean(row["DS1_PR"]);
                item.DS_ONK = Convert.ToBoolean(row["DS_ONK"]);

                item.DS2_N.AddRange(DS2_Ntbl.Select(Data.DS2_N.Get));
                item.NAZ.AddRange(NAZRtbl.Select(NAZR.Get));

                if (row["DN"] != DBNull.Value)
                    item.DN = Convert.ToInt32(row["DN"]);
                item.N_DN = Convert.ToString(row["N_DN"]);

                item.NAPR.AddRange(NAPRtbl.Select(Data.NAPR.Get));
                item.CONS.AddRange(CONStbl.Select(Data.CONS.Get));


                if (row["DS1_T"] != DBNull.Value)
                {
                    item.ONK_SL = ONK_SL.Get(row);
                }

                if (row["N_KSG"] != DBNull.Value)
                {
                    item.KSG_KPG = KSG_KPG.Get(row, CRITtbl, SL_KOEFtbl);
                }


                if (row["PRVS"] != DBNull.Value)
                    item.PRVS = Convert.ToInt32(row["PRVS"]);
                item.N_PRVS = Convert.ToString(row["N_PRVS"]);

                item.IDDOKT = Convert.ToString(row["IDDOKT"]);

                item.SUM_M = Convert.ToDecimal(row["SUM_M"]);

                if (row["SUM_MP"] != DBNull.Value)
                    item.SUM_MP = Convert.ToDecimal(row["SUM_MP"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения SL:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int SLUCH_Z_ID { get; set; }
        public string VID_HMP { get; set; }
        public string N_VID_HMP { get; set; }
        public int? METOD_HMP { get; set; }
        public string N_METOD_HMP { get; set; }
        public string LPU_1 { get; set; }
        public string N_LPU_1 { get; set; }
        public int? PODR { get; set; }
        public int? PROFIL { get; set; }
        public string N_PROFIL { get; set; }
        public int? PROFIL_K { get; set; }
        public string N_PROFIL_K { get; set; }
        public string P_CEL { get; set; }
        public string N_P_CEL { get; set; }
        public DateTime? TAL_D { get; set; }
        public string TAL_NUM { get; set; }
        public DateTime? TAL_P { get; set; }
        public string NHISTORY { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public DSX DS0 { get; set; }
        public DSX DS1 { get; set; }
        public List<DSX> DS2 { get; set; } = new List<DSX>();
        public List<DSX> DS3 { get; set; } = new List<DSX>();

        public string DS0str => DS0 != null ? $"{DS0.N_MKB}({DS0.MKB})" : "";
        public string DS1str => DS1 != null ? $"{DS1.N_MKB}({DS1.MKB})" : "";
        public string DS2str => string.Join(Environment.NewLine, DS2.Select(x => $"{x.N_MKB}({x.MKB})"));
        public string DS3str => string.Join(Environment.NewLine, DS3.Select(x => $"{x.N_MKB}({x.MKB})"));
        public int? C_ZAB { get; set; }
        public string N_C_ZAB { get; set; }
        public bool DS1_PR { get; set; }
        public bool DS_ONK { get; set; }
        public List<DS2_N> DS2_N { get; set; } = new List<DS2_N>();
        public List<NAZR> NAZ { get; set; } = new List<NAZR>();
        public int? DN { get; set; }
        public string N_DN { get; set; }
        public List<NAPR> NAPR { get; set; } = new List<NAPR>();
        public List<CONS> CONS { get; set; } = new List<CONS>();
        public ONK_SL ONK_SL { get; set; }
        public KSG_KPG KSG_KPG { get; set; }
        public int? PRVS { get; set; }
        public string N_PRVS { get; set; }
        public string IDDOKT { get; set; }
        public decimal SUM_M { get; set; }
        public decimal? SUM_MP { get; set; }
        public List<USL> USL { get; set; } = new List<USL>();
    }
    public class DS2_N
    {
        public static DS2_N Get(DataRow row)
        {
            try
            {
                var item = new DS2_N();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.DS2 = row["DS2"].ToString();
                item.N_DS2 = row["N_DS2"].ToString();
                item.DS2_PR = Convert.ToBoolean(row["DS2_PR"]);
                if (row["PR_DS2_N"] != DBNull.Value)
                    item.PR_DS2_N = Convert.ToInt32(row["PR_DS2_N"]);
                item.N_PR_DS2_N = row["N_PR_DS2_N"].ToString();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения DS2_N:" + ex.Message);
            }
        }
        public int SLUCH_ID { get; set; }
        public string DS2 { get; set; }
        public string N_DS2 { get; set; }
        public bool DS2_PR { get; set; }
        public int? PR_DS2_N { get; set; }
        public string N_PR_DS2_N { get; set; }


    }
    public class NAZR
    {
        public static NAZR Get(DataRow row)
        {
            try
            {
                var item = new NAZR();
                if (row["NAZ_N"] != DBNull.Value)
                    item.NAZ_N = Convert.ToInt32(row["NAZ_N"]);
                item.NAZ_R = Convert.ToInt32(row["NAZ_R"]);
                item.N_NAZ_R = Convert.ToString(row["N_NAZ_R"]);
                if (row["NAZ_SP"] != DBNull.Value)
                {
                    item.NAZ_SP = Convert.ToInt32(row["NAZ_SP"]);
                    item.N_NAZ_SP = Convert.ToString(row["N_NAZ_SP"]);
                }
                if (row["NAZ_V"] != DBNull.Value)
                {
                    item.NAZ_V = Convert.ToInt32(row["NAZ_V"]);
                    item.N_NAZ_V = Convert.ToString(row["N_NAZ_V"]);
                }
                if (row["NAZ_USL"] != DBNull.Value)
                {
                    item.NAZ_USL = Convert.ToString(row["NAZ_USL"]);
                    item.N_NAZ_USL = Convert.ToString(row["N_NAZ_USL"]);
                }
                if (row["NAPR_DATE"] != DBNull.Value)
                    item.NAPR_DATE = Convert.ToDateTime(row["NAPR_DATE"]);
                if (row["NAPR_MO"] != DBNull.Value)
                {
                    item.NAPR_MO = Convert.ToString(row["NAPR_MO"]);
                    item.N_NAPR_MO = Convert.ToString(row["N_NAPR_MO"]);
                }
                if (row["NAZ_PMP"] != DBNull.Value)
                {
                    item.NAZ_PMP = Convert.ToInt32(row["NAZ_PMP"]);
                    item.N_NAZ_PMP = Convert.ToString(row["N_NAZ_PMP"]);
                }
                if (row["NAZ_PK"] != DBNull.Value)
                {
                    item.NAZ_PK = Convert.ToInt32(row["NAZ_PK"]);
                    item.N_NAZ_PK = Convert.ToString(row["N_NAZ_PK"]);
                }
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения NAZR:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int NAZ_N { get; set; }
        public int NAZ_R { get; set; }
        public string N_NAZ_R { get; set; }
        public int? NAZ_SP { get; set; }
        public string N_NAZ_SP { get; set; }
        public int? NAZ_V { get; set; }
        public string N_NAZ_V { get; set; }
        public string NAZ_USL { get; set; }
        public string N_NAZ_USL { get; set; }
        public DateTime? NAPR_DATE { get; set; }
        public string NAPR_MO { get; set; }
        public string N_NAPR_MO { get; set; }
        public int? NAZ_PMP { get; set; }
        public string N_NAZ_PMP { get; set; }
        public int? NAZ_PK { get; set; }
        public string N_NAZ_PK { get; set; }
    }
    public class ONK_SL
    {
        public static ONK_SL Get(DataRow row)
        {
            try
            {
                var item = new ONK_SL();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                if (row["DS1_T"] != DBNull.Value)
                {
                    item.DS1_T = Convert.ToInt32(row["DS1_T"]);
                    item.N_DS1_T = Convert.ToString(row["N_DS1_T"]);
                }
                if (row["MTSTZ"] != DBNull.Value)
                    item.MTSTZ = Convert.ToBoolean(row["MTSTZ"]);
                if (row["ONK_M"] != DBNull.Value)
                {
                    item.ONK_M = Convert.ToInt32(row["ONK_M"]);
                    item.N_ONK_M = Convert.ToString(row["N_ONK_M"]);
                }
                if (row["ONK_N"] != DBNull.Value)
                {
                    item.ONK_N = Convert.ToInt32(row["ONK_N"]);
                    item.N_ONK_N = Convert.ToString(row["N_ONK_N"]);
                }

                if (row["ONK_T"] != DBNull.Value)
                {
                    item.ONK_T = Convert.ToInt32(row["ONK_T"]);
                    item.N_ONK_T = Convert.ToString(row["N_ONK_T"]);
                }



                if (row["SOD"] != DBNull.Value)
                    item.SOD = Convert.ToDecimal(row["SOD"]);
                if (row["STAD"] != DBNull.Value)
                {
                    item.STAD = Convert.ToInt32(row["STAD"]);
                    item.N_STAD = Convert.ToString(row["N_STAD"]);
                }
                if (row["K_FR"] != DBNull.Value)
                    item.K_FR = Convert.ToInt32(row["K_FR"]);
                if (row["WEI"] != DBNull.Value)
                    item.WEI = Convert.ToDecimal(row["WEI"]);
                if (row["HEI"] != DBNull.Value)
                    item.HEI = Convert.ToInt32(row["HEI"]);
                if (row["BSA"] != DBNull.Value)
                    item.BSA = Convert.ToDecimal(row["BSA"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения ONK_SL:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int DS1_T { get; set; }
        public string N_DS1_T { get; set; }
        public int? STAD { get; set; }
        public string N_STAD { get; set; }
        public int? ONK_T { get; set; }
        public string N_ONK_T { get; set; }
        public int? ONK_N { get; set; }
        public string N_ONK_N { get; set; }
        public int? ONK_M { get; set; }
        public string N_ONK_M { get; set; }
        public bool? MTSTZ { get; set; }
        public decimal? SOD { get; set; }
        public int? K_FR { get; set; }
        public decimal? WEI { get; set; }
        public int? HEI { get; set; }
        public decimal? BSA { get; set; }
        public List<B_DIAG> B_DIAG { get; set; } = new List<B_DIAG>();
        public List<B_PROT> B_PROT { get; set; } = new List<B_PROT>();
        public List<ONK_USL> ONK_USL { get; set; } = new List<ONK_USL>();

    }
    public class B_DIAG
    {
        public static B_DIAG Get(DataRow row)
        {
            try
            {
                var item = new B_DIAG();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.DIAG_DATE = Convert.ToDateTime(row["DIAG_DATE"]);

                item.DIAG_TIP = Convert.ToInt32(row["DIAG_TIP"]);
                item.N_DIAG_TIP = Convert.ToString(row["N_DIAG_TIP"]);

                item.DIAG_CODE = Convert.ToInt32(row["DIAG_CODE"]);
                item.N_DIAG_CODE = Convert.ToString(row["N_DIAG_CODE"]);

                if (row["DIAG_RSLT"] != DBNull.Value)
                {
                    item.DIAG_RSLT = Convert.ToInt32(row["DIAG_RSLT"]);
                    item.N_DIAG_RSLT = Convert.ToString(row["N_DIAG_RSLT"]);
                }
                if (row["REC_RSLT"] != DBNull.Value)
                    item.REC_RSLT = Convert.ToBoolean(row["REC_RSLT"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения B_DIAG:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public DateTime DIAG_DATE { get; set; }
        public int DIAG_TIP { get; set; }
        public string N_DIAG_TIP { get; set; }
        public int DIAG_CODE { get; set; }
        public string N_DIAG_CODE { get; set; }
        public int? DIAG_RSLT { get; set; }
        public string N_DIAG_RSLT { get; set; }
        public bool? REC_RSLT { get; set; }
    }
    public class B_PROT
    {
        public static B_PROT Get(DataRow row)
        {
            try
            {
                var item = new B_PROT();
                item.PROT = Convert.ToInt32(row["PROT"]);
                item.N_PROT = Convert.ToString(row["N_PROT"]);
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.D_PROT = Convert.ToDateTime(row["D_PROT"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения B_PROT:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int PROT { get; set; }
        public string N_PROT { get; set; }
        public DateTime D_PROT { get; set; }
    }
    public class KSG_KPG
    {
        public static KSG_KPG Get(DataRow row, IEnumerable<DataRow> CRITtbl, IEnumerable<DataRow> SL_KOEFtbl)
        {
            try
            {
                var item = new KSG_KPG();
                item.N_KSG = row["N_KSG"].ToString();
                item.N_N_KSG = row["N_N_KSG"].ToString();
                if (row["KOEF_Z"] != DBNull.Value)
                    item.KOEF_Z = Convert.ToDecimal(row["KOEF_Z"]);
                if (row["KOEF_UP"] != DBNull.Value)
                    item.KOEF_UP = Convert.ToDecimal(row["KOEF_UP"]);
                if (row["BZTSZ"] != DBNull.Value)
                    item.BZTSZ = Convert.ToDecimal(row["BZTSZ"]);
                if (row["KOEF_D"] != DBNull.Value)
                    item.KOEF_D = Convert.ToDecimal(row["KOEF_D"]);
                if (row["KOEF_U"] != DBNull.Value)
                    item.KOEF_U = Convert.ToDecimal(row["KOEF_U"]);
                if (row["SL_K"] != DBNull.Value)
                    item.SL_K = Convert.ToDecimal(row["SL_K"]);
                if (row["IT_SL"] != DBNull.Value)
                    item.IT_SL = Convert.ToDecimal(row["IT_SL"]);

                item.CRIT.AddRange(CRITtbl.Select(Data.CRIT.Get));
                item.SL_KOEF.AddRange(SL_KOEFtbl.Select(Data.SL_KOEF.Get));

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения KSG_KPG:" + ex.Message);
            }
        }



        public string N_KSG { get; set; }
        public string N_N_KSG { get; set; }
        public decimal? KOEF_Z { get; set; }
        public decimal? KOEF_UP { get; set; }
        public decimal? BZTSZ { get; set; }
        public decimal? KOEF_D { get; set; }
        public decimal? KOEF_U { get; set; }
        public List<CRIT> CRIT { get; set; } = new List<CRIT>();
        public decimal? SL_K { get; set; }
        public decimal? IT_SL { get; set; }
        public List<SL_KOEF> SL_KOEF { get; set; } = new List<SL_KOEF>();
    }
    public class SL_KOEF
    {
        public static SL_KOEF Get(DataRow row)
        {
            try
            {
                var item = new SL_KOEF();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.IDSL = Convert.ToInt32(row["IDSL"]);
                item.N_IDSL = Convert.ToString(row["N_IDSL"]);
                item.Z_SL = Convert.ToDecimal(row["Z_SL"]);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения SL_KOEF:" + ex.Message);
            }
        }
        public int SLUCH_ID { get; set; }
        public int IDSL { get; set; }
        public string N_IDSL { get; set; }
        public decimal Z_SL { get; set; }

    }
    public class CRIT
    {
        public static CRIT Get(DataRow row)
        {
            try
            {
                var item = new CRIT();
                item.CRIT_VALUE = Convert.ToString(row["CRIT_VALUE"]);
                item.N_CRIT_VALUE = Convert.ToString(row["N_CRIT_VALUE"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения CRIT:" + ex.Message);
            }
        }
        public string CRIT_VALUE { get; set; }
        public string N_CRIT_VALUE { get; set; }


    }
    public class SANK
    {
        public SANK()
        {
            CODE_EXP = new List<CODE_EXP>();
            SL_ID = new List<string>();
        }
        public static SANK Get(DataRow row)
        {
            try
            {
                var item = new SANK();
                if (row["SANK_ID"] != DBNull.Value)
                    item.SANK_ID = Convert.ToDecimal(row["SANK_ID"]);
                if (row["SLUCH_ID"] != DBNull.Value)
                    item.SLUCH_ID = Convert.ToDecimal(row["SLUCH_ID"]);
                if (row["S_CODE"] != DBNull.Value)
                    item.S_CODE = row["S_CODE"].ToString();
                if (row["S_COM"] != DBNull.Value)
                    item.S_COM = row["S_COM"].ToString();
                if (row["S_FINE"] != DBNull.Value)
                    item.S_FINE = Convert.ToDecimal(row["S_FINE"]);
                if (row["S_IDSERV"] != DBNull.Value)
                    item.S_IDSERV = row["S_IDSERV"].ToString();
                if (row["S_IST"] != DBNull.Value)
                    item.S_IST = Convert.ToDecimal(row["S_IST"]);
                if (row["S_MONTH"] != DBNull.Value)
                    item.S_MONTH = Convert.ToDecimal(row["S_MONTH"]);
                if (row["S_OSN"] != DBNull.Value)
                    item.S_OSN = Convert.ToDecimal(row["S_OSN"]);
                if (row["S_PLAN"] != DBNull.Value)
                    item.S_PLAN = Convert.ToDecimal(row["S_PLAN"]);
                if (row["S_SUM"] != DBNull.Value)
                    item.S_SUM = Convert.ToDecimal(row["S_SUM"]);
                if (row["S_TEM"] != DBNull.Value)
                    item.S_TEM = Convert.ToDecimal(row["S_TEM"]);
                if (row["S_TIP"] != DBNull.Value)
                    item.S_TIP = Convert.ToDecimal(row["S_TIP"]);
                if (row["S_YEAR"] != DBNull.Value)
                    item.S_YEAR = Convert.ToDecimal(row["S_YEAR"]);
                if (row["S_ZGLV_ID"] != DBNull.Value)
                    item.S_ZGLV_ID = Convert.ToDecimal(row["S_ZGLV_ID"]);
                if (row["DATE_ACT"] != DBNull.Value)
                    item.DATE_ACT = Convert.ToDateTime(row["DATE_ACT"]);
                if (row["NUM_ACT"] != DBNull.Value)
                    item.NUM_ACT = Convert.ToString(row["NUM_ACT"]);


                foreach (var exp in row["SL_ID"].ToString().Split(','))
                {
                    if (!string.IsNullOrEmpty(exp))
                    {
                        item.SL_ID.Add(exp);
                    }
                }

                foreach (var exp in row["CODE_EXP"].ToString().Split(','))
                {
                    if (!string.IsNullOrEmpty(exp))
                    {
                        item.CODE_EXP.Add(new CODE_EXP { SANK_ID = item.SANK_ID, VALUE = exp });
                    }
                }
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения SANK:" + ex.Message);
            }
        }
        [XmlIgnore]
        public decimal? S_ZGLV_ID { get; set; }
        [XmlIgnore]
        public decimal? SLUCH_ID { get; set; }
        [XmlIgnore]
        public decimal? SLUCH_Z_ID { get; set; }
        [XmlIgnore]
        public decimal? SANK_ID { get; set; }


        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string S_CODE { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public decimal S_SUM { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public decimal S_TIP { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<string> SL_ID { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public decimal S_OSN { get; set; }
        [XmlElement("DATE_ACT", Form = XmlSchemaForm.Unqualified, IsNullable = true, DataType = "date")]
        public DateTime? DATE_ACT { get; set; }
        public bool ShouldSerializeDATE_ACT()
        {
            return DATE_ACT.HasValue;
        }
        [XmlElement("NUM_ACT", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string NUM_ACT { get; set; }

        [XmlElement("CODE_EXP", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public List<CODE_EXP> CODE_EXP { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string S_COM { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public decimal S_IST { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public decimal? S_YEAR { get; set; }
        public bool ShouldSerializeS_YEAR()
        {
            return S_YEAR.HasValue;
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public decimal? S_MONTH { get; set; }
        public bool ShouldSerializeS_MONTH()
        {
            return S_MONTH.HasValue;
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public decimal? S_PLAN { get; set; }
        public bool ShouldSerializeS_PLAN()
        {
            return S_PLAN.HasValue;
        }
        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public decimal? S_TEM { get; set; }
        public bool ShouldSerializeS_TEM()
        {
            return S_TEM.HasValue;
        }
        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = true)]
        public decimal? S_FINE { get; set; }
        public bool ShouldSerializeS_FINE()
        {
            return S_FINE.HasValue;
        }

        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string S_IDSERV { get; set; }

        [XmlIgnore]
        public string GetSL_ID => string.Join(",", SL_ID);
    }
    public class CODE_EXP
    {
        [XmlIgnore]
        public decimal? SANK_ID { get; set; }
        [XmlText]
        public string VALUE { get; set; }
    }
    public class USL
    {
        public static USL Get(DataRow row)
        {
            try
            {
                var item = new USL();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.USL_ID = Convert.ToInt32(row["USL_ID"]);
                item.IDSERV = Convert.ToString(row["IDSERV"]);
                if (row["PROFIL"] != DBNull.Value)
                    item.PROFIL = Convert.ToInt32(row["PROFIL"].ToString());
                if (row["N_PROFIL"] != DBNull.Value)
                    item.N_PROFIL = row["N_PROFIL"].ToString();
                item.VID_VME = row["VID_VME"].ToString();
                item.N_VID_VME = row["N_VID_VME"].ToString();
                item.DATE_IN = Convert.ToDateTime(row["DATE_IN"].ToString());
                item.DATE_OUT = Convert.ToDateTime(row["DATE_OUT"].ToString());
                item.DS = row["DS"].ToString();
                item.N_DS = row["N_DS"].ToString();
                item.CODE_USL = row["CODE_USL"].ToString();
                item.N_CODE_USL = row["N_CODE_USL"].ToString();
                if (row["KOL_USL"] != DBNull.Value)
                    item.KOL_USL = Convert.ToDecimal(row["KOL_USL"].ToString());
                if (row["SUMV_USL"] != DBNull.Value)
                    item.SUMV_USL = Convert.ToDecimal(row["SUMV_USL"].ToString());
                if (row["SUMP_USL"] != DBNull.Value)
                    item.SUMP_USL = Convert.ToDecimal(row["SUMP_USL"].ToString());
                if (row["PRVS"] != DBNull.Value)
                    item.PRVS = Convert.ToInt32(row["PRVS"].ToString());
                item.N_PRVS = row["N_PRVS"].ToString();
                item.CODE_MD = row["CODE_MD"].ToString();
                if (row["NPL"] != DBNull.Value)
                    item.NPL = Convert.ToInt32(row["NPL"].ToString());
                if (row["N_NPL"] != DBNull.Value)
                    item.N_NPL = row["N_NPL"].ToString();
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения USL:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int USL_ID { get; set; }
        public string IDSERV { get; set; }
        public int? PROFIL { get; set; }
        public string N_PROFIL { get; set; }
        public string VID_VME { get; set; }
        public string N_VID_VME { get; set; }
        public DateTime DATE_IN { get; set; }
        public DateTime DATE_OUT { get; set; }
        public string DS { get; set; }
        public string N_DS { get; set; }
        public string CODE_USL { get; set; }
        public string N_CODE_USL { get; set; }
        public decimal? KOL_USL { get; set; }
        public decimal SUMV_USL { get; set; }
        public decimal? SUMP_USL { get; set; }
        public int PRVS { get; set; }
        public string N_PRVS { get; set; }
        public string CODE_MD { get; set; }
        public int? NPL { get; set; }
        public string N_NPL { get; set; }
    }
    public class NAPR
    {
        public static NAPR Get(DataRow row)
        {
            try
            {
                var item = new NAPR();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.NAPR_DATE = Convert.ToDateTime(row["NAPR_DATE"]);

                item.NAPR_MO = row["NAPR_MO"].ToString();
                item.N_NAPR_MO = row["N_NAPR_MO"].ToString();

                item.NAPR_V = Convert.ToInt32(row["NAPR_V"]);
                item.N_NAPR_V = Convert.ToString(row["N_NAPR_V"]);


                if (row["MET_ISSL"] != DBNull.Value)
                {
                    item.MET_ISSL = Convert.ToInt32(row["MET_ISSL"]);
                    item.N_MET_ISSL = Convert.ToString(row["N_MET_ISSL"]);
                }

                item.NAPR_USL = row["NAPR_USL"].ToString();
                item.N_NAPR_USL = row["N_NAPR_USL"].ToString();

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения NAPR:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public DateTime NAPR_DATE { get; set; }
        public string NAPR_MO { get; set; }
        public string N_NAPR_MO { get; set; }
        public int NAPR_V { get; set; }
        public string N_NAPR_V { get; set; }
        public int? MET_ISSL { get; set; }
        public string N_MET_ISSL { get; set; }
        public string NAPR_USL { get; set; }
        public string N_NAPR_USL { get; set; }
    }
    public class CONS
    {
        public static CONS Get(DataRow row)
        {
            try
            {
                var item = new CONS();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.PR_CONS = Convert.ToInt32(row["PR_CONS"]);
                item.N_PR_CONS = Convert.ToString(row["N_PR_CONS"]);
                if (row["DT_CONS"] != DBNull.Value)
                    item.DT_CONS = Convert.ToDateTime(row["DT_CONS"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения CONS:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int PR_CONS { get; set; }
        public string N_PR_CONS { get; set; }
        public DateTime? DT_CONS { get; set; }
    }
    public class ONK_USL
    {
        public static ONK_USL Get(DataRow row, IEnumerable<DataRow> LEK_PRtbl)
        {
            try
            {
                var item = new ONK_USL();
                item.SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]);
                item.ONK_USL_ID = Convert.ToInt32(row["ONK_USL_ID"]);
                item.USL_TIP = Convert.ToInt32(row["USL_TIP"]);
                item.N_USL_TIP = Convert.ToString(row["N_USL_TIP"]);

                if (row["HIR_TIP"] != DBNull.Value)
                {
                    item.HIR_TIP = Convert.ToInt32(row["HIR_TIP"]);
                    item.N_HIR_TIP = Convert.ToString(row["N_HIR_TIP"]);
                }

                if (row["LEK_TIP_L"] != DBNull.Value)
                {
                    item.LEK_TIP_L = Convert.ToInt32(row["LEK_TIP_L"]);
                    item.N_LEK_TIP_L = Convert.ToString(row["N_LEK_TIP_L"]);
                }

                if (row["LEK_TIP_V"] != DBNull.Value)
                {
                    item.LEK_TIP_V = Convert.ToInt32(row["LEK_TIP_V"]);
                    item.N_LEK_TIP_V = Convert.ToString(row["N_LEK_TIP_V"]);
                }

                if (row["LUCH_TIP"] != DBNull.Value)
                {
                    item.LUCH_TIP = Convert.ToInt32(row["LUCH_TIP"]);
                    item.N_LUCH_TIP = Convert.ToString(row["N_LUCH_TIP"]);
                }

                if (row["PPTR"] != DBNull.Value)
                    item.PPTR = Convert.ToBoolean(row["PPTR"]);
                item.LEK_PR.AddRange(LEK_PRtbl.Select(Data.LEK_PR.Get));
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения ONK_USL:" + ex.Message);
            }
        }

        public int SLUCH_ID { get; set; }
        public int ONK_USL_ID { get; set; }
        public int USL_TIP { get; set; }
        public string N_USL_TIP { get; set; }
        public int? HIR_TIP { get; set; }
        public string N_HIR_TIP { get; set; }
        public int? LEK_TIP_L { get; set; }
        public string N_LEK_TIP_L { get; set; }
        public int? LEK_TIP_V { get; set; }
        public string N_LEK_TIP_V { get; set; }
        public List<LEK_PR> LEK_PR { get; set; } = new List<LEK_PR>();
        public bool? PPTR { get; set; }
        public int? LUCH_TIP { get; set; }
        public string N_LUCH_TIP { get; set; }

    }
    public class LEK_PR
    {
        public static LEK_PR Get(DataRow row)
        {
            try
            {
                var item = new LEK_PR();
                item.ONK_USL_ID = Convert.ToInt32(row["ONK_USL_ID"]);

                if (row["REGNUM"] != DBNull.Value)
                {
                    item.REGNUM = row["REGNUM"].ToString();
                    item.N_REGNUM = row["N_REGNUM"].ToString();
                }

                if (row["CODE_SH"] != DBNull.Value)
                {
                    item.CODE_SH = row["CODE_SH"].ToString();
                    item.N_CODE_SH = row["N_CODE_SH"].ToString();
                }

                foreach (var t in row["DATE_INJ"].ToString().Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(Convert.ToDateTime).ToList())
                {
                    item.DATE_INJ.Add(new DATE_INJ { VALUE = t });
                }

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения LEK_PR:" + ex.Message);
            }
        }

        public int ONK_USL_ID { get; set; }
        public int LEK_PR_ID { get; set; }
        public string REGNUM { get; set; }
        public string N_REGNUM { get; set; }
        public string CODE_SH { get; set; }
        public string N_CODE_SH { get; set; }
        public List<DATE_INJ> DATE_INJ { get; set; } = new List<DATE_INJ>();

    }
    public class DATE_INJ
    {

        public int LEK_PR_ID { get; set; }
        public DateTime VALUE { get; set; }

    }
}
