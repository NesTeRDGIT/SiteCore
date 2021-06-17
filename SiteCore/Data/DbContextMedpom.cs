using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;

namespace SiteCore.Data
{
    public class MyOracleSet : DbContext
    {
        public MyOracleSet(DbContextOptions<MyOracleSet> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SITE");
           // modelBuilder.Entity<FILES>().HasOne(x=>x.FILE_L).WithOne(x=>x.ID_FILEL).HasForeignKey(x=>x.ID_FILEL);
            modelBuilder.Entity<FILES>().HasOne(x => x.PARENT).WithOne(x => x.FILE_L).HasForeignKey<FILES>(x=>x.ID_FILEL);

            modelBuilder.Entity<FILES>().HasOne(x => x.FILEPACK).WithMany(x => x.FILES).HasForeignKey(x => x.ID_PACK);

            modelBuilder.Entity<NEWS>().HasMany(x => x.NEWS_ROLE).WithOne(x => x.NEWS).HasForeignKey(x => x.ID_NEW);
            modelBuilder.Entity<NEWS_ROLE>().HasOne(x => x.NEWS).WithMany(x => x.NEWS_ROLE).HasForeignKey(x => x.ID_NEW);
            modelBuilder.Entity<NEWS_ROLE>().HasKey(x => new {x.ID_NEW, x.ID_ROLE});

            modelBuilder.Entity<ErrorSPRSection>().HasMany(x => x.Error).WithOne(x => x.Section).HasForeignKey(x => x.ID_SECTION);

        }
        public virtual DbSet<FILEPACK> FILEPACK { get; set; }
        public virtual DbSet<FILES> FILES { get; set; }
        public virtual DbSet<MESSAGE> MESSAGE { get; set; }
        public virtual DbSet<CURRENT_VMP_OOMS> CURRENT_VMP_OOMS { get; set; }
        public virtual DbSet<SNILS_SIGN> SNILS_SIGN { get; set; }
        public virtual DbSet<NEWS> NEWS { get; set; }
        public virtual DbSet<ErrorSPRSection> ErrorSPRSection { get; set; }
        public virtual DbSet<ErrorSPR> ErrorSPR { get; set; }


        public List<Abort_Row> GetReportAbort(int YEAR)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ABORT(:YEAR))", this.Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return Abort_Row.Get(tbl.Select());
        }


        public List<ECO_MP_Row> GetEKO_MP(int YEAR, int MONTH)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ECO_MP(:YEAR,:MONTH))", this.Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            oda.SelectCommand.Parameters.Add("MONTH", MONTH);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ECO_MP_Row.Get(tbl.Select());
        }


        public List<ECO_MTR_Row> GetEKO_MTR(int YEAR, int MONTH)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ECO_MTR(:YEAR,:MONTH))", this.Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            oda.SelectCommand.Parameters.Add("MONTH", MONTH);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ECO_MTR_Row.Get(tbl.Select());
        }


        public List<KOHL_Row> GetKOHL(DateTime dt1, DateTime dt2)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(OOMS_REPORT.GET_KOHL(:dt1,:dt2))", this.Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("dt1", dt1);
            oda.SelectCommand.Parameters.Add("dt2", dt2);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return KOHL_Row.Get(tbl.Select());
        }

        public List<OKS_ONMK_Row> GetOKS_ONMK(int YEAR)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(OOMS_REPORT.GET_OKS_ONMK(:YEAR))", this.Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return OKS_ONMK_Row.Get(tbl.Select());
        }

    }
    [Table("MESSAGE")]
    public class MESSAGE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal ID { get; set; }
        public string TEXT { get; set; }
        public LEV_MESSAGE? LEV { get; set; }
    }
    [Table("CURRENT_VMP_OOMS")]
    public class CURRENT_VMP_OOMS
    {
        [Key]
        public decimal ID { get; set; }
        public string CODE_MO { get; set; }
        public string SMO { get; set; }
        public string FIO { get; set; }
        public string VID_HMP { get; set; }
        public string METOD_HMP { get; set; }
        public string GRP { get; set; }
        public string DS1 { get; set; }
        public DateTime? DATE_1 { get; set; }
        public DateTime? DATE_2 { get; set; }
        public DateTime? TAL_P { get; set; }
        public DateTime? TAL_D { get; set; }
        public string OS_SLUCH { get; set; }
        public decimal? SUMM { get; set; }



    }


    public class Abort_Row
    {
        public static List<Abort_Row> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static Abort_Row Get(DataRow row)
        {
            try
            {
                var item = new Abort_Row();
                item.DS = row["DS"].ToString();
                if (row["USL"] != DBNull.Value)
                    item.USL = Convert.ToInt32(row["USL"]);
                item.C = Convert.ToInt32(row["C"]);
                item.SUMV = Convert.ToDecimal(row["SUMV"]);
                item.Text = Convert.ToString(row["Text"]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения Abort_Row: {ex.Message}", ex);
            }

        }

        public string Text { get; set; }
        public string DS { get; set; }
        public int? USL { get; set; }
        public int C { get; set; }
        public decimal SUMV { get; set; }
    }
    public class ECO_MP_Row
    {
        public static List<ECO_MP_Row> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static ECO_MP_Row Get(DataRow row)
        {
            try
            {
                var item = new ECO_MP_Row
                {
                    SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]),
                    SMO = Convert.ToString(row["SMO"]),
                    YEAR = Convert.ToInt32(row["YEAR"]),
                    MONTH = Convert.ToInt32(row["MONTH"]),
                    CODE_MO = Convert.ToString(row["CODE_MO"]),
                    NAM_MOK = Convert.ToString(row["NAM_MOK"]),
                    FAM = Convert.ToString(row["FAM"]),
                    IM = Convert.ToString(row["IM"]),
                    OT = Convert.ToString(row["OT"]),
                    DR = Convert.ToDateTime(row["DR"]),
                    DATE_1 = Convert.ToDateTime(row["DATE_1"]),
                    DATE_2 = Convert.ToDateTime(row["DATE_2"]),
                    N_KSG = Convert.ToString(row["N_KSG"]),
                    NAME_KSG = Convert.ToString(row["NAME_KSG"]),
                    SUMV = Convert.ToDecimal(row["SUMV"]),
                    KSLP = Convert.ToString(row["KSLP"]),
                    KSLP_NAME = Convert.ToString(row["KSLP_NAME"])
                };

                if (row["SUMP"] != DBNull.Value)
                    item.SUMP = Convert.ToDecimal(row["SUMP"]);

                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения ECO_MP_Row: {ex.Message}", ex);
            }

        }

        public int SLUCH_ID { get; set; }
        public string SMO { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public string CODE_MO { get; set; }
        public string NAM_MOK { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public string N_KSG { get; set; }
        public string NAME_KSG { get; set; }
        public decimal SUMV { get; set; }
        public decimal? SUMP { get; set; }
        public string KSLP { get; set; }
        public string KSLP_NAME { get; set; }
    }

    public class ECO_MTR_Row
    {
        public static List<ECO_MTR_Row> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static ECO_MTR_Row Get(DataRow row)
        {
            try
            {
                var item = new ECO_MTR_Row
                {
                    SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]),
                    DS1 = Convert.ToString(row["DS1"]),
                    DS_NAME = Convert.ToString(row["DS_NAME"]),
                    NPLAT = Convert.ToString(row["NPLAT"]),
                    C_OKATO = Convert.ToString(row["C_OKATO"]),
                    NAM_MOK = Convert.ToString(row["NAM_MOK"]),
                    FAM = Convert.ToString(row["FAM"]),
                    IM = Convert.ToString(row["IM"]),
                    OT = Convert.ToString(row["OT"]),
                    DR = Convert.ToDateTime(row["DR"]),
                    DATE_1 = Convert.ToDateTime(row["DATE_1"]),
                    DATE_2 = Convert.ToDateTime(row["DATE_2"]),
                    NAME_TFK = Convert.ToString(row["NAME_TFK"]),
                    COMENTSL = Convert.ToString(row["COMENTSL"]),
                    SUMV = Convert.ToDecimal(row["SUMV"]),
                    USL = Convert.ToString(row["USL"]),
                    LPU = Convert.ToString(row["LPU"])
                };

                if (row["SUMP"] != DBNull.Value)
                    item.SUMP = Convert.ToDecimal(row["SUMP"]);
                if (row["DPLAT"] != DBNull.Value)
                    item.DPLAT = Convert.ToDateTime(row["DPLAT"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения ECO_MTR_Row: {ex.Message}", ex);
            }

        }

        public int SLUCH_ID { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public decimal SUMV { get; set; }
        public decimal? SUMP { get; set; }



        public string DS1 { get; set; }
        public string DS_NAME { get; set; }
        public DateTime? DPLAT { get; set; }
        public string NPLAT { get; set; }
        public string LPU { get; set; }
        public string NAM_MOK { get; set; }
        public string C_OKATO { get; set; }
        public string NAME_TFK { get; set; }
        public string COMENTSL { get; set; }
        public string USL { get; set; }
    }

    public class KOHL_Row
    {
        public static List<KOHL_Row> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static KOHL_Row Get(DataRow row)
        {
            try
            {
                var item = new KOHL_Row
                {

                    SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]),
                    C_OKATO = Convert.ToString(row["C_OKATO"]),
                    NAM_MOK = Convert.ToString(row["NAM_MOK"]),
                    FAM = Convert.ToString(row["FAM"]),
                    IM = Convert.ToString(row["IM"]),
                    OT = Convert.ToString(row["OT"]),
                    DR = Convert.ToDateTime(row["DR"]),
                    DATE_1 = Convert.ToDateTime(row["DATE_1"]),
                    DATE_2 = Convert.ToDateTime(row["DATE_2"]),
                    NAME_TFK = Convert.ToString(row["NAME_TFK"]),
                    COMENTSL = Convert.ToString(row["COMENTSL"]),
                    SUMV = Convert.ToInt32(row["SUMV"]),
                    USL = Convert.ToString(row["USL"]),
                    LPU = Convert.ToString(row["LPU"]),
                    DS1 = Convert.ToString(row["DS1"]),
                    DS1_NAME = Convert.ToString(row["DS1_NAME"])
                };

                if (row["SUMP"] != DBNull.Value)
                    item.SUMP = Convert.ToInt32(row["SUMP"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения KOHL_Row: {ex.Message}", ex);
            }

        }

        public int SLUCH_ID { get; set; }
        public string C_OKATO { get; set; }
        public string NAME_TFK { get; set; }
        public string LPU { get; set; }
        public string NAM_MOK { get; set; }


        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public string DS1 { get; set; }
        public string DS1_NAME { get; set; }
        public decimal SUMV { get; set; }
        public string COMENTSL { get; set; }
        public decimal? SUMP { get; set; }
        public string USL { get; set; }
    }

    public class OKS_ONMK_Row
    {
        public static List<OKS_ONMK_Row> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static OKS_ONMK_Row Get(DataRow row)
        {
            try
            {
                var item = new OKS_ONMK_Row
                {

                    SLUCH_ID = Convert.ToInt32(row["SLUCH_ID"]),
                    SMO = Convert.ToString(row["SMO"]),
                    CODE_MO = Convert.ToString(row["CODE_MO"]),
                    NAM_MOK = Convert.ToString(row["NAM_MOK"]),
                    LEV = Convert.ToString(row["LEV"]),
                    PROFIL = Convert.ToString(row["PROFIL"]),
                    FORMA = Convert.ToString(row["FORMA"]),
                    FAM = Convert.ToString(row["FAM"]),
                    IM = Convert.ToString(row["IM"]),
                    OT = Convert.ToString(row["OT"]),
                    DR = Convert.ToDateTime(row["DR"]),
                    SPOLIS = Convert.ToString(row["SPOLIS"]),
                    NPOLIS = Convert.ToString(row["NPOLIS"]),
                    DATE_1 = Convert.ToDateTime(row["DATE_1"]),
                    DATE_2 = Convert.ToDateTime(row["DATE_2"]),
                    DS1 = Convert.ToString(row["DS1"]),
                    DS1_NAME = Convert.ToString(row["DS1_NAME"]),
                    N_KSG = Convert.ToString(row["N_KSG"]),
                    NAME_KSG = Convert.ToString(row["NAME_KSG"]),
                    RSLT = Convert.ToString(row["RSLT"]),
                    ISHOD = Convert.ToString(row["ISHOD"]),
                    SUMV = Convert.ToDecimal(row["SUMV"])
                };
                if (row["SUMP"] != DBNull.Value)
                    item.SUMP = Convert.ToDecimal(row["SUMP"]);


                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения OKS_ONMK_Row: {ex.Message}", ex);
            }

        }
        public int SLUCH_ID { get; set; }
        public string SMO { get; set; }
        public string CODE_MO { get; set; }
        public string NAM_MOK { get; set; }
        public string LEV { get; set; }
        public string PROFIL { get; set; }
        public string FORMA { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public string SPOLIS { get; set; }
        public string NPOLIS { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public string DS1 { get; set; }
        public string DS1_NAME { get; set; }
        public string N_KSG { get; set; }
        public string NAME_KSG { get; set; }
        public string RSLT { get; set; }
        public string ISHOD { get; set; }
        public decimal SUMV { get; set; }
        public decimal? SUMP { get; set; }
    }



    [Table("FILEPACK")]
    public class FILEPACK
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public decimal ID { get; set; }
        public string CODE_MO { get; set; }
        public STATUS_FILEPACK? STATUS { get; set; }
        public DateTime? DOWNPROT_LAST { get; set; }
        public DateTime? DOWNPROT_FIRST { get; set; }
        public virtual ICollection<FILES> FILES { get; set; } = new List<FILES>();
    }

    [Table("FILES")]
    public  class FILES
    {
        public FILES()
        {
        }
        public FILES(decimal? iD_FILEL, decimal? iD_PACK, STATUS_FILE? sTATUS)
        {
            ID_FILEL = iD_FILEL;
            ID_PACK = iD_PACK;
            STATUS = sTATUS;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID { get;  set; }
        public string FILENAME { get; set; }
        public DateTime? DATECREATE { get; set; }
        public decimal? ID_FILEL { get; set; }
        public decimal? ID_PACK { get; set; }
        public STATUS_FILE? STATUS { get; set; }
        public string COMENT { get; set; }
        public string HASH { get; set; } = "";
        public byte[] SIGN_DIR { get; set; }
        [NotMapped]
        public string SIGN_DIR_STR
        {
            get => SIGN_DIR == null ? "" : ByteArrayToString(SIGN_DIR);
            set => SIGN_DIR = StringToByteArray(value);
        }
        public bool SIGN_DIR_VALID { get; set; }
        public byte[] SIGN_BUH { get; set; }
        [NotMapped]
        public string SIGN_BUH_STR
        {
            get => SIGN_BUH == null ? "" : ByteArrayToString(SIGN_BUH);
            set => SIGN_BUH = StringToByteArray(value);
        }
        public bool SIGN_BUH_VALID { get; set; }
        public byte[] SIGN_ISP { get; set; }
        [NotMapped]
        public string SIGN_ISP_STR
        {
            get => SIGN_ISP == null ? "" : ByteArrayToString(SIGN_ISP);
            set => SIGN_ISP = StringToByteArray(value);
        }
        public bool SIGN_ISP_VALID { get; set; }
        private static byte[] StringToByteArray(string hex)
        {
            return Convert.FromBase64String(hex);
        }
        private static string ByteArrayToString(byte[] hashValue)
        {
            return Convert.ToBase64String(hashValue);
        }
        public TYPEFILE? TYPE_FILE { get; set; }
        public virtual FILEPACK FILEPACK { get; set; }
        public virtual FILES FILE_L { get; set; }
        public virtual FILES PARENT { get; set; }
    }


    public static class Ext
    {
        public static bool Contains(this TYPEFILE? TYPE_FILE, params TYPEFILE[] list)
        {
            return TYPE_FILE.HasValue && list.Contains(TYPE_FILE.Value);
        }
    }

    public enum STATUS_FILE 
    {
        NOT_INVITE = 0,
        INVITE = 1,
        XML_VALID = 2,
        XML_NOT_VALID = 3
    }

    public enum TYPEFILE 
    {
        H = 0,
        T = 1,
        DP = 2,
        DV = 3,
        DO = 4,
        DS = 5,
        DU = 6,
        DF = 7,
        DD = 8,
        DR = 9,
        LH = 10,
        LT = 11,
        LP = 12,
        LV = 13,
        LO = 14,
        LS = 15,
        LU = 16,
        LF = 17,
        LD = 18,
        LR = 19,
        C = 20,
        LC = 21
    }
    public enum LEV_MESSAGE 
    {
        EMPTY = 0,
        ERROR = 1,
        WARNING = 2
    }

    public enum STATUS_FILEPACK
    {
        CURRENT = 0,
        SEND = 1

    }
    [Flags]
    public enum SIGN_OWNER
    {
        NONE = 0,
        DIR = 2,
        BUH = 4,
        ISP = 8
    }

    [Table("SNILS_SIGN")]

    public class SNILS_SIGN
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; private set; }
        public string CODE_MO { get; set; } = "";
        public string SN { get; set; }
        public SIGN_OWNER OWNER { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
        public string PUBLICKEY { get; set; }

        public byte[] FILE_CERT { get; set; }

        [ForeignKey("CODE_MO")]
        public virtual CODE_MO CODE_MO_NAME { get; set; }

        public string DATE_E_STR => DATE_E.HasValue ? DATE_E.ToString("yyyy-MM-dd") : "";

        public string DATE_B_STR => DATE_B.ToString("yyyy-MM-dd");
    }

    [Table("NEWS")]
    public class NEWS
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int? ID_NEW { get; set; }
        /// <summary>
        /// Дата новости
        /// </summary>
        public DateTime DT { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        public string HEADER { get; set; }
        /// <summary>
        /// Текст новости
        /// </summary>
        public byte[] TEXT { get; set; }

        [NotMapped]
        public string TEXT_STR
        {
            get => TEXT == null ? "" : ByteArrayToString(TEXT);
            set => TEXT = StringToByteArray(value);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Encoding.GetEncoding(1251).GetBytes(hex);
        }
        private static string ByteArrayToString(byte[] hashValue)
        {
            return Encoding.GetEncoding(1251).GetString(hashValue);
        }
        public virtual ICollection<NEWS_ROLE> NEWS_ROLE { get; set; } = new List<NEWS_ROLE>();
    }

    [Table("NEWS_ROLE")]
    public class NEWS_ROLE
    {
        [Key]
        [Column(Order = 1)]
        public int ID_NEW { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ID_ROLE { get; set; }

        public virtual NEWS NEWS { get; set; }

    }
    [Table("ERRORSPRSECTION")]
    public class ErrorSPRSection
    {
        [Key]
        public int ID_SECTION { get; set; }
        [Required]
        public int ORD { get; set; }
        [Required]
        public string SECTION_NAME { get; set; }
        public virtual ICollection<ErrorSPR> Error { get; set; } = new List<ErrorSPR>();
    }
    [Table("ERRORSPR")]
    public class ErrorSPR
    {
        [Required]
        public int ID_SECTION { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ID_ERR { get; set; }
        [Required]
        public string EXAMPLE { get; set; }
        public byte[] TEXT { get; set; }
        public string OSN_TFOMS { get; set; }
        public DateTime D_EDIT { get; set; } = DateTime.Now;
        [NotMapped]
        public string TEXT_STR
        {
            get => TEXT == null ? "" : ByteArrayToString(TEXT);
            set => TEXT = StringToByteArray(value);
        }
        private static byte[] StringToByteArray(string hex)
        {
            return Encoding.GetEncoding(1251).GetBytes(hex);
        }
        private static string ByteArrayToString(byte[] hashValue)
        {
            return Encoding.GetEncoding(1251).GetString(hashValue);
        }
        public virtual ErrorSPRSection Section { get; set; }
    }
}
