using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore.Infrastructure.Internal;
using Oracle.ManagedDataAccess.Client;
using SiteCore.Class;

namespace SiteCore.Data
{
    public class MyOracleSet : DbContext
    {
        private string ConnectionString = "";
        public MyOracleSet(DbContextOptions<MyOracleSet> options) : base(options)
        {
            ConnectionString = options?.GetExtension<OracleOptionsExtension>().ConnectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SITE");
     
            modelBuilder.Entity<FILES>().HasOne(x => x.FILE_L).WithOne(x=>x.PARENT).HasForeignKey<FILES>(x => x.ID_FILEL);

            modelBuilder.Entity<FILES>().HasOne(x => x.FILEPACK).WithMany(x => x.FILES).HasForeignKey(x => x.ID_PACK);

            modelBuilder.Entity<NEWS>().HasMany(x => x.NEWS_ROLE).WithOne(x => x.NEWS).HasForeignKey(x => x.ID_NEW);
            modelBuilder.Entity<NEWS_ROLE>().HasOne(x => x.NEWS).WithMany(x => x.NEWS_ROLE).HasForeignKey(x => x.ID_NEW);
            modelBuilder.Entity<NEWS_ROLE>().HasKey(x => new { x.ID_NEW, x.ID_ROLE });

            modelBuilder.Entity<ErrorSPRSection>().HasMany(x => x.Error).WithOne(x => x.Section).HasForeignKey(x => x.ID_SECTION);

            modelBuilder.Entity<SIGN_LIST>().HasOne(x => x.ROLE).WithMany(x => x.Signs).HasForeignKey(x => x.SIGN_ROLE_ID);
            modelBuilder.Entity<SIGN_LIST>().HasOne(x => x.MO_NAME).WithMany().HasForeignKey(x => x.CODE_MO);


            modelBuilder.Entity<DOC_SIGN>().HasOne(x => x.DOC).WithMany().HasForeignKey(x => x.DOC_FOR_SIGN_ID).IsRequired();
            modelBuilder.Entity<DOC_SIGN>().HasOne(x => x.ROLE).WithMany().HasForeignKey(x => x.SIGN_ROLE_ID).IsRequired();
            modelBuilder.Entity<DOC_SIGN>().HasOne(x => x.SIGN_ITEM).WithMany().HasForeignKey(x => x.SIGN_LIST_ID);


            modelBuilder.Entity<DOC_FOR_SIGN>().HasMany(x => x.SIGNs).WithOne(x=>x.DOC).HasForeignKey(x => x.DOC_FOR_SIGN_ID);
            modelBuilder.Entity<DOC_FOR_SIGN>().HasOne(x => x.CODE_MO_NAME).WithMany().HasForeignKey(x => x.CODE_MO);

            modelBuilder.Entity<DOC_FOR_SIGN>().HasOne(x => x.THEME).WithMany(x=>x.DOCs).HasForeignKey(x => x.THEME_ID);

        }

        public virtual DbSet<FILEPACK> FILEPACK { get; set; }
        public virtual DbSet<FILES> FILES { get; set; }
        public virtual DbSet<MESSAGE> MESSAGE { get; set; }
     

        
        public virtual DbSet<SNILS_SIGN> SNILS_SIGN { get; set; }
        public virtual DbSet<NEWS> NEWS { get; set; }
        public virtual DbSet<ErrorSPRSection> ErrorSPRSection { get; set; }
        public virtual DbSet<ErrorSPR> ErrorSPR { get; set; }
        public virtual DbSet<SIGN_LIST> SIGN_LIST { get; set; }
        public virtual DbSet<SIGN_ROLE> SIGN_ROLE { get; set; }
        public virtual DbSet<SIGN_ISSUER> SING_ISSUER { get; set; }

        public virtual DbSet<CODE_MO> CODE_MO { get; set; }
        public virtual DbSet<DOC_SIGN> DOC_SIGN { get; set; }
        public virtual DbSet<DOC_FOR_SIGN> DOC_FOR_SIGN { get; set; }

        public virtual  DbSet<DOC_THEME> DOC_THEME { get; set; }

        public List<Abort_Row> GetReportAbort(int YEAR)
        {
            var oda = new OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ABORT(:YEAR))", Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return Abort_Row.Get(tbl.Select());
        }

        public Task<List<Abort_Row>> GetReportAbortAsync(int YEAR)
        {
            return Task.Run(() => GetReportAbort(YEAR));

        }

        private List<ECO_MP_Row> GetEKO_MP(int YEAR, int MONTH)
        {
            using var con = new OracleConnection(ConnectionString);
            using var oda = new OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ECO_MP(:YEAR,:MONTH))", con);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            oda.SelectCommand.Parameters.Add("MONTH", MONTH);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ECO_MP_Row.Get(tbl.Select());
        }
        private Task<List<ECO_MP_Row>> GetEKO_MPAsync(int YEAR, int MONTH)
        {
            return Task.Run(() => GetEKO_MP(YEAR, MONTH));
        }

        private List<ECO_MTR_Row> GetEKO_MTR(int YEAR, int MONTH)
        {
            using var con = new OracleConnection(ConnectionString);
            var oda = new OracleDataAdapter($"select* from table(OOMS_REPORT.GET_ECO_MTR(:YEAR,:MONTH))", con);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            oda.SelectCommand.Parameters.Add("MONTH", MONTH);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ECO_MTR_Row.Get(tbl.Select());
        }

        private Task<List<ECO_MTR_Row>> GetEKO_MTRAsync(int YEAR, int MONTH)
        {
            return Task.Run(() => GetEKO_MTR(YEAR, MONTH));
        }


        public async Task<ECO_RECORD> GetECO_RECORDAsync(int YEAR, int MONTH)
        {
            var taskECO_MTR = GetEKO_MTRAsync(YEAR, MONTH);
            var taskECO_MP = GetEKO_MPAsync(YEAR, MONTH);
            return new ECO_RECORD { ECO_MTR = await taskECO_MTR, ECO_MP = await taskECO_MP };
        }

        public List<KOHL_Row> GetKOHL(DateTime dt1, DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            var oda = new OracleDataAdapter($"select* from table(OOMS_REPORT.GET_KOHL(:dt1,:dt2))", con);
            oda.SelectCommand.Parameters.Add("dt1", dt1);
            oda.SelectCommand.Parameters.Add("dt2", dt2);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return KOHL_Row.Get(tbl.Select());
        }

        public Task<List<KOHL_Row>> GetKOHLAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(()=>GetKOHL(dt1, dt2));
        }

        public List<OKS_ONMK_Row> GetOKS_ONMK(int YEAR)
        {
            var oda = new OracleDataAdapter($"select* from table(OOMS_REPORT.GET_OKS_ONMK(:YEAR))", Database.GetDbConnection().ConnectionString);
            oda.SelectCommand.Parameters.Add("YEAR", YEAR);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return OKS_ONMK_Row.Get(tbl.Select());
        }

        public Task<List<OKS_ONMK_Row>> GetOKS_ONMKAsync(int YEAR)
        {
            return Task.Run(() => GetOKS_ONMK(YEAR));
        }

        public List<ZPZ_EFFECTIVENESS> Get_ZPZ_EFFECTIVENESS(DateTime dt1, DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            using var oda = new OracleDataAdapter("select * from table(zpz_otchet.Get_RESULT(:dt1,:dt2))", con);
            oda.SelectCommand.Parameters.Add("dt1", dt1);
            oda.SelectCommand.Parameters.Add("dt2", dt2);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ZPZ_EFFECTIVENESS.Get(tbl.Select());
        }

        public Task<List<ZPZ_EFFECTIVENESS>> Get_ZPZ_EFFECTIVENESSAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(()=>Get_ZPZ_EFFECTIVENESS(dt1, dt2));
        }


        public DataTable STAT_STAC_TBL_3()
        {
            var oda = new OracleDataAdapter("select * from TAB_3_PROFIL_KD", Database.GetDbConnection().ConnectionString);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return tbl;
        }

        public DataTable STAT_STAC_TBL_4()
        {
            var oda = new OracleDataAdapter("select * from TAB_4_LEVEL_GOSP", Database.GetDbConnection().ConnectionString);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return tbl;
        }

        public DataTable STAT_STAC_TBL_2()
        {
            var oda = new OracleDataAdapter("select * from TAB_2_REPORT_MO_PROFIL_STAC t", Database.GetDbConnection().ConnectionString);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return tbl;
        }

        public DataTable STAT_STAC_TBL_1()
        {
            var oda = new OracleDataAdapter("select * from TAB_1_REPORT_KOEK_STAC t", Database.GetDbConnection().ConnectionString);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return tbl;
        }

        public async Task BUILD_STAT_STAC_TBL_1(DateTime dt1, DateTime dt2)
        {
            await using var con = new OracleConnection(Database.GetDbConnection().ConnectionString);
            var cmd = new OracleCommand("begin STAT_STAC.GET_TAB1(:dt1,:dt2);  end;", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            cmd.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task BUILD_STAT_STAC_TBL_2(DateTime dt1, DateTime dt2)
        {
            await using var con = new OracleConnection(Database.GetDbConnection().ConnectionString);
            var cmd = new OracleCommand("begin STAT_STAC.GET_TAB2(:dt1,:dt2);  end;", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            cmd.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task BUILD_STAT_STAC_TBL_3(DateTime dt1, DateTime dt2)
        {
            await using var con = new OracleConnection(Database.GetDbConnection().ConnectionString);
            var cmd = new OracleCommand("begin STAT_STAC.GET_TAB3(:dt1,:dt2);  end;", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            cmd.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

        public async Task BUILD_STAT_STAC_TBL_4(DateTime dt1, DateTime dt2)
        {
            await using var con = new OracleConnection(Database.GetDbConnection().ConnectionString);
            var cmd = new OracleCommand("begin STAT_STAC.GET_TAB4(:dt1,:dt2);  end;", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            cmd.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            cmd.Connection.Close();
        }

    
        public async Task<IEnumerable<CURRENT_VMP_OOMS2>> GetVMPReportAsync()
        {
            await using var con = new OracleConnection(Database.GetDbConnection().ConnectionString);
            await using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GET_VMP)", con);
            await con.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();
            return CURRENT_VMP_OOMS2.GetRows(reader);
        }



        public List<ResultControlVZR> GetResultControlVZR(DateTime dt1, DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(zpz_otchet.GETResultControlVZR(:dt1,:dt2))", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            con.Open();
            var reader = cmd.ExecuteReader();
            return ResultControlVZR.GetList(reader);
        }

        public Task<List<ResultControlVZR>> GetResultControlVZRAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(() => GetResultControlVZR(dt1, dt2));
        }

        public List<ResultControlDET> GetResultControlDET(DateTime dt1, DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(zpz_otchet.GETResultControlDET(:dt1,:dt2))", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            con.Open();
            var reader = cmd.ExecuteReader();
            return ResultControlDET.GetList(reader);
        }

        public Task<List<ResultControlDET>> GetResultControlDETAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(() => GetResultControlDET(dt1, dt2));
        }


        public List<SMPRow> GetSMP(DateTime dt1, DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetSMP(:dt1,:dt2))", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            con.Open();
            var reader = cmd.ExecuteReader();
            return SMPRow.GetList(reader);
        }

        public Task<List<SMPRow>> GetSMPAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(() => GetSMP(dt1, dt2));
        }


        public List<DataBaseStateRow> GetDataBaseState()
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(REPORT.GetDBState)", con);
           
            con.Open();
            var reader = cmd.ExecuteReader();
            return DataBaseStateRow.GetList(reader);
        }

        public Task<List<DataBaseStateRow>> GetDataBaseStateAsync()
        {
            return Task.Run(() => GetDataBaseState());
        }


        public List<PENSRow> GetPENS(int YEAR)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetPENS(:YEAR))", con);
            cmd.Parameters.Add("YEAR", YEAR);
            con.Open();
            var reader = cmd.ExecuteReader();
            return PENSRow.GetList(reader);
        }

        public Task<List<PENSRow>> GetPENSAsync(int YEAR)
        {
            return Task.Run(() => GetPENS(YEAR));
        }
        public List<VMP_OOMS> GetVMP_PERIOD(DateTime dt1,DateTime dt2)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetVMP_PERIOD(:dt1,:dt2))", con);
            cmd.Parameters.Add("dt1", dt1);
            cmd.Parameters.Add("dt2", dt2);
            con.Open();
            var reader = cmd.ExecuteReader();
            return VMP_OOMS.GetList(reader);
        }

        public Task<List<VMP_OOMS>> GetVMP_PERIODAsync(DateTime dt1, DateTime dt2)
        {
            return Task.Run(() => GetVMP_PERIOD(dt1,dt2));
        }

        #region Отчет по диспансеризации

        private List<DispDetRow> GetDispDetReport(DateTime dt)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetDispDetReport(:dt))", con);
            cmd.Parameters.Add("dt", dt.Date);
            con.Open();
            var reader = cmd.ExecuteReader();
            return DispDetRow.GetList(reader);
        }

        private Task<List<DispDetRow>> GetDispDetReportAsync(DateTime dt)
        {
            return Task.Run(() => GetDispDetReport(dt));
        }


        private List<DispVzrRow> GetDispVzrReport(DateTime dt)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetDispVzrReport(:dt))", con);
            cmd.Parameters.Add("dt", dt.Date);
            con.Open();
            var reader = cmd.ExecuteReader();
            return DispVzrRow.GetList(reader);
        }

        private Task<List<DispVzrRow>> GetDispVzrReportAsync(DateTime dt)
        {
            return Task.Run(() => GetDispVzrReport(dt));
        }

        private List<ProfVzrRow> GetProfVzrReport(DateTime dt)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetProfVzrReport(:dt))", con);
            cmd.Parameters.Add("dt", dt.Date);
            con.Open();
            var reader = cmd.ExecuteReader();
            return ProfVzrRow.GetList(reader);
        }

        private Task<List<ProfVzrRow>> GetProfVzrReportAsync(DateTime dt)
        {
            return Task.Run(() => GetProfVzrReport(dt));
        }

        private List<ProfDetRow> GetProfDetReport(DateTime dt)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetProfDetReport(:dt))", con);
            cmd.Parameters.Add("dt", dt.Date);
            con.Open();
            var reader = cmd.ExecuteReader();
            return ProfDetRow.GetList(reader);
        }

        private Task<List<ProfDetRow>> GetProfDetReportAsync(DateTime dt)
        {
            return Task.Run(() => GetProfDetReport(dt));
        }

        public async Task<DispRecord> GetDispReportAsync(DateTime dt)
        {
            var record = new DispRecord();
          
            var dispDetTask = this.GetDispDetReportAsync(dt);
            var dispVzrTask = this.GetDispVzrReportAsync(dt);
            var profDetTask = this.GetProfDetReportAsync(dt);
            var profVzrTask = this.GetProfVzrReportAsync(dt);

            record.DispDet = await dispDetTask;
            record.DispVzr = await dispVzrTask;
            record.ProfDet = await profDetTask;
            record.ProfVzr = await profVzrTask;

            return record;

        }

        #endregion

        public List<Kv2MtrRow> GetKV2_MTR(DateTime dt)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetKV2_MTR(:dt))", con);
            cmd.Parameters.Add("dt", dt.Date);
            con.Open();
            var reader = cmd.ExecuteReader();
            return Kv2MtrRow.GetList(reader);
        }

        public Task<List<Kv2MtrRow>> GetKV2_MTRAsync(DateTime dt)
        {
            return Task.Run(() => GetKV2_MTR(dt));
        }


        #region Отчет ДЛИ
        private List<DliTbl1Row> GetDliTbl1(int year)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetDliTbl1(:year))", con);
            cmd.Parameters.Add("year", year);
            con.Open();
            var reader = cmd.ExecuteReader();
            return DliTbl1Row.GetList(reader);
        }

        private Task<List<DliTbl1Row>> GetDliTbl1Async(int year)
        {
            return Task.Run(() => GetDliTbl1(year));
        }
        private List<DliTbl2Row> GetDliTbl2(int year)
        {
            using var con = new OracleConnection(ConnectionString);
            using var cmd = new OracleCommand("select * from table(OOMS_REPORT.GetDliTbl2(:year))", con);
            cmd.Parameters.Add("year", year);
            con.Open();
            var reader = cmd.ExecuteReader();
            return DliTbl2Row.GetList(reader);
        }

        private Task<List<DliTbl2Row>> GetDliTbl2Async(int year)
        {
            return Task.Run(() => GetDliTbl2(year));
        }


        public async Task<DliRecord> GetDliAsync(int year)
        {
            var tbl1task = GetDliTbl1Async(year);
            var tbl2task = GetDliTbl2Async(year);
            return new DliRecord { tbl1 = await tbl1task, tbl2 = await tbl2task };
        }

        #endregion

    }

    [Table("MESSAGE")]
    public class MESSAGE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string TEXT { get; set; }
        public LEV_MESSAGE? LEV { get; set; }
    }


    public class CURRENT_VMP_OOMS2
    {
        public static List<CURRENT_VMP_OOMS2> GetRows(IDataReader reader)
        {
            var result = new List<CURRENT_VMP_OOMS2>();
            while (reader.Read())
            {
                result.Add(GetRow(reader));
            }
            return result;
        }
        public static CURRENT_VMP_OOMS2 GetRow(IDataReader reader)
        {
            try
            {
                var item = new CURRENT_VMP_OOMS2();
                item.CODE_MO = Convert.ToString(reader[nameof(CODE_MO)]);
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.FIO = Convert.ToString(reader[nameof(FIO)]);
                item.VID_HMP = Convert.ToString(reader[nameof(VID_HMP)]);
                item.METOD_HMP = Convert.ToString(reader[nameof(METOD_HMP)]);
                item.GRP = Convert.ToString(reader[nameof(GRP)]);
                item.DS1 = Convert.ToString(reader[nameof(DS1)]);
                if(reader[nameof(DATE_1)]!=DBNull.Value)
                    item.DATE_1 = Convert.ToDateTime(reader[nameof(DATE_1)]);
                if (reader[nameof(DATE_2)] != DBNull.Value)
                    item.DATE_2 = Convert.ToDateTime(reader[nameof(DATE_2)]);
                if (reader[nameof(TAL_P)] != DBNull.Value)
                    item.TAL_P = Convert.ToDateTime(reader[nameof(TAL_P)]);
                if (reader[nameof(TAL_D)] != DBNull.Value)
                    item.TAL_D = Convert.ToDateTime(reader[nameof(TAL_D)]);
                item.OS_SLUCH = Convert.ToString(reader[nameof(OS_SLUCH)]);
                if (reader[nameof(SUMM)] != DBNull.Value)
                    item.SUMM = Convert.ToDecimal(reader[nameof(SUMM)]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения CURRENT_VMP_OOMS2: {ex.Message}", ex);
            }

        }
  
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
    public class VMP_OOMS
    {
        public static List<VMP_OOMS> GetList(IDataReader reader)
        {
            var result = new List<VMP_OOMS>();
            while (reader.Read())
            {
                result.Add(GetRow(reader));
            }
            return result;
        }
        public static VMP_OOMS GetRow(IDataReader reader)
        {
            try
            {
                var item = new VMP_OOMS();
                item.SLUCH_ID = Convert.ToInt64(reader[nameof(SLUCH_ID)]);
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.CODE_MO = Convert.ToString(reader[nameof(CODE_MO)]);
                item.NAME_MO = Convert.ToString(reader[nameof(NAME_MO)]);
                item.YEAR = Convert.ToInt32(reader[nameof(YEAR)]);
                item.MONTH = Convert.ToInt32(reader[nameof(MONTH)]);
                item.FIO = Convert.ToString(reader[nameof(FIO)]); 
                item.W = Convert.ToInt32(reader[nameof(W)]);
                item.VPOLIS = Convert.ToInt32(reader[nameof(VPOLIS)]);
                item.SPOLIS = Convert.ToString(reader[nameof(SPOLIS)]);
                item.NPOLIS = Convert.ToString(reader[nameof(NPOLIS)]);
                item.AGE = Convert.ToInt32(reader[nameof(AGE)]);
                item.VID_HMP = Convert.ToString(reader[nameof(VID_HMP)]);
                item.METOD_HMP = Convert.ToString(reader[nameof(METOD_HMP)]);
                item.GRP_HMP = Convert.ToString(reader[nameof(GRP_HMP)]);
                item.DAYS = Convert.ToInt32(reader[nameof(DAYS)]);
                item.MKB = Convert.ToString(reader[nameof(MKB)]);
             
                item.SUMV = Convert.ToDecimal(reader[nameof(SUMV)]);
                if(reader[nameof(SUMP)]!=DBNull.Value)
                    item.SUMP = Convert.ToDecimal(reader[nameof(SUMP)]);
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения VMP_OOMS: {ex.Message}", ex);
            }

        }
        public long SLUCH_ID { get; set; }
        public string SMO { get; set; }
        public string CODE_MO { get; set; }
        public string NAME_MO { get; set; }
        public int YEAR { get; set; }
        public int MONTH { get; set; }
        public string FIO { get; set; }
        public int W { get; set; }
        public int VPOLIS { get; set; }
        public string SPOLIS { get; set; }
        public string NPOLIS { get; set; }
        public int AGE { get; set; }
        public string VID_HMP { get; set; }
        public string METOD_HMP { get; set; }
        public string GRP_HMP { get; set; }
        public int DAYS { get; set; }
        public string MKB { get; set; }
        public decimal SUMV { get; set; }
        public decimal? SUMP { get; set; }
    }


    [Serializable]
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
    public class ECO_RECORD
    {
        public List<ECO_MTR_Row> ECO_MTR { get; set; }
        public List<ECO_MP_Row> ECO_MP { get; set; }
        public List<ECO_MP_Row> SVOD_ECO_MO
        {
            get
            {
                var groupBy = ECO_MP.GroupBy(x => new { x.SMO, x.KSLP_NAME, x.KSLP });
                return groupBy.Select(x => new ECO_MP_Row { SMO = x.Key.SMO, KSLP = x.Key.KSLP, KSLP_NAME = x.Key.KSLP_NAME, SUMV = x.Sum(y => y.SUMV), SUMP = x.Sum(y => y.SUMP ?? 0), SLUCH_ID = x.Count() }).ToList();
            }

        }
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


   

    public class SMPRow
    {
        public static List<SMPRow> GetList(IDataReader reader)
        {
            var result = new List<SMPRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static SMPRow Get(IDataReader reader)
        {
            try
            {
                var item = new SMPRow();
                item.POK = Convert.ToString(reader[nameof(POK)]);
                item.NN = Convert.ToString(reader[nameof(NN)]);
                item.KOL = Convert.ToInt32(reader[nameof(KOL)]);
                item.SUM = Convert.ToDecimal(reader[nameof(SUM)]);
                item.KOL_DET = Convert.ToInt32(reader[nameof(KOL_DET)]);
                item.SUM_DET = Convert.ToDecimal(reader[nameof(SUM_DET)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения SMPRow: {e.Message}", e);
            }
        }


        public string POK { get; set; }
        public string NN { get; set; }
        public int KOL { get; set; }
        public decimal SUM { get; set; }
        public int KOL_DET { get; set; }
        public decimal SUM_DET { get; set; }

    }

    public class DataBaseStateRow
    {
        public static List<DataBaseStateRow> GetList(IDataReader reader)
        {
            var result = new List<DataBaseStateRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static DataBaseStateRow Get(IDataReader reader)
        {
            try
            {
                var item = new DataBaseStateRow();
                item.POK = Convert.ToString(reader[nameof(POK)]);
                item.VALUE = Convert.ToString(reader[nameof(VALUE)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения DataBaseStateRow: {e.Message}", e);
            }
        }


        public string POK { get; set; }
        public string VALUE { get; set; }
    }

    public class PENSRow
    {
        public static List<PENSRow> GetList(IDataReader reader)
        {
            var result = new List<PENSRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static PENSRow Get(IDataReader reader)
        {
            try
            {
                var item = new PENSRow();
                item.CODE_MO = Convert.ToString(reader[nameof(CODE_MO)]);
                item.NAME = Convert.ToString(reader[nameof(NAME)]);
                item.DISP_M_GR1 = Convert.ToInt32(reader[nameof(DISP_M_GR1)]);
                item.DISP_G_GR1 = Convert.ToInt32(reader[nameof(DISP_G_GR1)]);
                item.PROF_M_GR1 = Convert.ToInt32(reader[nameof(PROF_M_GR1)]);
                item.PROF_G_GR1 = Convert.ToInt32(reader[nameof(PROF_G_GR1)]);

                item.DISP_M_GR2 = Convert.ToInt32(reader[nameof(DISP_M_GR2)]);
                item.DISP_G_GR2 = Convert.ToInt32(reader[nameof(DISP_G_GR2)]);
                item.PROF_M_GR2 = Convert.ToInt32(reader[nameof(PROF_M_GR2)]);
                item.PROF_G_GR2 = Convert.ToInt32(reader[nameof(PROF_G_GR2)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения PENSRow: {e.Message}", e);
            }
        }


        public string CODE_MO { get; set; }
        public string NAME { get; set; }
        public int DISP_M_GR1 { get; set; }
        public int DISP_G_GR1 { get; set; }
        public int PROF_M_GR1 { get; set; }
        public int PROF_G_GR1 { get; set; }

        public int DISP_M_GR2 { get; set; }
        public int DISP_G_GR2 { get; set; }
        public int PROF_M_GR2 { get; set; }
        public int PROF_G_GR2 { get; set; }


    }



    public class DispRecord
    {
        public List<DispDetRow> DispDet { get; set; }
        public List<DispVzrRow> DispVzr { get; set; }
        public List<ProfVzrRow> ProfVzr { get; set; }
        public List<ProfDetRow> ProfDet { get; set; }

    }
    public class DispDetRow
    {
        public static List<DispDetRow> GetList(IDataReader reader)
        {
            var result = new List<DispDetRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static DispDetRow Get(IDataReader reader)
        {
            try
            {
                var item = new DispDetRow();
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.POK = Convert.ToString(reader[nameof(POK)]);
                item.SUMV = Convert.ToDecimal(reader[nameof(SUMV)]);
                item.SUM = Convert.ToInt32(reader[nameof(SUM)]);
                item.KOL = Convert.ToInt32(reader[nameof(KOL)]);
                item.KOL_VBR = Convert.ToInt32(reader[nameof(KOL_VBR)]);
                item.SUM_VBR = Convert.ToDecimal(reader[nameof(SUM_VBR)]);

                item.SUMP = Convert.ToDecimal(reader[nameof(SUMP)]);
                item.KOL_P = Convert.ToInt32(reader[nameof(KOL_P)]);
                item.SUM_P = Convert.ToDecimal(reader[nameof(SUM_P)]);
                item.KOL_VBR_P = Convert.ToInt32(reader[nameof(KOL_VBR_P)]);
                item.SUM_VBR_P = Convert.ToDecimal(reader[nameof(SUM_VBR_P)]);
                item.C_GRP_1 = Convert.ToInt32(reader[nameof(C_GRP_1)]);
                item.C_GRP_2 = Convert.ToInt32(reader[nameof(C_GRP_2)]);
                item.C_GRP_3 = Convert.ToInt32(reader[nameof(C_GRP_3)]);
                item.C_GRP_4 = Convert.ToInt32(reader[nameof(C_GRP_4)]);
                item.C_GRP_5 = Convert.ToInt32(reader[nameof(C_GRP_5)]);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения DispDetRow: {e.Message}", e);
            }
        }


        public string SMO { get; set; }
        public string POK { get; set; }
        public decimal SUMV { get; set; }
        public int KOL { get; set; }
        public decimal SUM { get; set; }
        public int KOL_VBR { get; set; }
        public decimal SUM_VBR { get; set; }

        public decimal SUMP { get; set; }
        public int KOL_P { get; set; }
        public decimal SUM_P { get; set; }
        public int KOL_VBR_P { get; set; }
        public decimal SUM_VBR_P { get; set; }

        public int C_GRP_1 { get; set; }
        public int C_GRP_2 { get; set; }
        public int C_GRP_3 { get; set; }
        public int C_GRP_4 { get; set; }
        public int C_GRP_5 { get; set; }

    }

    public class DispVzrRow
    {
        public static List<DispVzrRow> GetList(IDataReader reader)
        {
            var result = new List<DispVzrRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static DispVzrRow Get(IDataReader reader)
        {
            try
            {
                var item = new DispVzrRow();
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.POK = Convert.ToString(reader[nameof(POK)]);
                item.ST3 = Convert.ToDecimal(reader[nameof(ST3)]);
                item.ST4 = Convert.ToDecimal(reader[nameof(ST4)]);
                item.ST5 = Convert.ToDecimal(reader[nameof(ST5)]);
                item.ST6 = Convert.ToDecimal(reader[nameof(ST6)]);
                item.ST7 = Convert.ToDecimal(reader[nameof(ST7)]);
                item.ST8 = Convert.ToDecimal(reader[nameof(ST8)]);
                item.ST9 = Convert.ToDecimal(reader[nameof(ST9)]);
                item.ST10 = Convert.ToDecimal(reader[nameof(ST10)]);
                item.ST11 = Convert.ToDecimal(reader[nameof(ST11)]);
                item.ST12 = Convert.ToDecimal(reader[nameof(ST12)]);
                item.ST13 = Convert.ToDecimal(reader[nameof(ST13)]);
                item.ST14 = Convert.ToDecimal(reader[nameof(ST14)]);
                item.ST15 = Convert.ToDecimal(reader[nameof(ST15)]);
                item.ST16 = Convert.ToDecimal(reader[nameof(ST16)]);
                item.ST17 = Convert.ToDecimal(reader[nameof(ST17)]);
                item.ST18 = Convert.ToDecimal(reader[nameof(ST18)]);
                item.ST19 = Convert.ToDecimal(reader[nameof(ST19)]);
                item.ST20 = Convert.ToDecimal(reader[nameof(ST20)]);
                item.ST21 = Convert.ToDecimal(reader[nameof(ST21)]);
                item.ST22 = Convert.ToDecimal(reader[nameof(ST22)]);
                item.ST23 = Convert.ToDecimal(reader[nameof(ST23)]);
                item.ST24 = Convert.ToDecimal(reader[nameof(ST24)]);
                item.ST25 = Convert.ToDecimal(reader[nameof(ST25)]);
                item.ST26 = Convert.ToDecimal(reader[nameof(ST26)]);
                item.ST27 = Convert.ToDecimal(reader[nameof(ST27)]);
                item.ST28 = Convert.ToDecimal(reader[nameof(ST28)]);
                item.ST29 = Convert.ToDecimal(reader[nameof(ST29)]);
                item.ST30 = Convert.ToDecimal(reader[nameof(ST30)]);
                item.ST31 = Convert.ToDecimal(reader[nameof(ST31)]);
                item.ST32 = Convert.ToDecimal(reader[nameof(ST32)]);
                item.ST33 = Convert.ToDecimal(reader[nameof(ST33)]);
                item.ST34 = Convert.ToDecimal(reader[nameof(ST34)]);
                item.ST35 = Convert.ToDecimal(reader[nameof(ST35)]);
                item.ST36 = Convert.ToDecimal(reader[nameof(ST36)]);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения DispVzrRow: {e.Message}", e);
            }
        }


        public string SMO { get; set; }
        public string POK { get; set; }
        public decimal ST3 { get; set; }
        public decimal ST4 { get; set; }
        public decimal ST5 { get; set; }
        public decimal ST6 { get; set; }
        public decimal ST7 { get; set; }
        public decimal ST8 { get; set; }
        public decimal ST9 { get; set; }
        public decimal ST10 { get; set; }
        public decimal ST11 { get; set; }
        public decimal ST12 { get; set; }
        public decimal ST13 { get; set; }
        public decimal ST14 { get; set; }
        public decimal ST15 { get; set; }
        public decimal ST16 { get; set; }
        public decimal ST17 { get; set; }
        public decimal ST18 { get; set; }
        public decimal ST19 { get; set; }
        public decimal ST20 { get; set; }
        public decimal ST21 { get; set; }
        public decimal ST22 { get; set; }
        public decimal ST23 { get; set; }
        public decimal ST24 { get; set; }
        public decimal ST25 { get; set; }
        public decimal ST26 { get; set; }
        public decimal ST27 { get; set; }
        public decimal ST28 { get; set; }
        public decimal ST29 { get; set; }
        public decimal ST30 { get; set; }
        public decimal ST31 { get; set; }
        public decimal ST32 { get; set; }
        public decimal ST33 { get; set; }
        public decimal ST34 { get; set; }
        public decimal ST35 { get; set; }
        public decimal ST36 { get; set; }

    }

    public class ProfVzrRow
    {
        public static List<ProfVzrRow> GetList(IDataReader reader)
        {
            var result = new List<ProfVzrRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static ProfVzrRow Get(IDataReader reader)
        {
            try
            {
                var item = new ProfVzrRow();
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.NAM_SMO = Convert.ToString(reader[nameof(NAM_SMO)]);
                item.NN = Convert.ToString(reader[nameof(NN)]);
                item.GRP = Convert.ToString(reader[nameof(GRP)]);
                item.KOL = Convert.ToInt32(reader[nameof(KOL)]);
                item.SUM = Convert.ToDecimal(reader[nameof(SUM)]);
                item.KOL_P = Convert.ToInt32(reader[nameof(KOL_P)]);
                item.SUM_P = Convert.ToDecimal(reader[nameof(SUM_P)]);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения ProfVzrRow: {e.Message}", e);
            }
        }


        public string SMO { get; set; }
        public string NAM_SMO { get; set; }
        public string NN { get; set; }
        public string GRP { get; set; }
        public int KOL { get; set; }
        public decimal SUM { get; set; }
        public int KOL_P { get; set; }
        public decimal SUM_P { get; set; }

    }
    public class ProfDetRow
    {
        public static List<ProfDetRow> GetList(IDataReader reader)
        {
            var result = new List<ProfDetRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static ProfDetRow Get(IDataReader reader)
        {
            try
            {
                var item = new ProfDetRow();
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.ST2 = Convert.ToDecimal(reader[nameof(ST2)]);
                item.ST3 = Convert.ToDecimal(reader[nameof(ST3)]);
                item.ST4 = Convert.ToDecimal(reader[nameof(ST4)]);
                item.ST5 = Convert.ToDecimal(reader[nameof(ST5)]);
                item.ST6 = Convert.ToDecimal(reader[nameof(ST6)]);
                item.ST7 = Convert.ToDecimal(reader[nameof(ST7)]);
                item.ST8 = Convert.ToDecimal(reader[nameof(ST8)]);
                item.ST9 = Convert.ToDecimal(reader[nameof(ST9)]);
                item.ST10 = Convert.ToDecimal(reader[nameof(ST10)]);
                item.ST11 = Convert.ToDecimal(reader[nameof(ST11)]);
                item.ST12 = Convert.ToDecimal(reader[nameof(ST12)]);
                item.ST13 = Convert.ToDecimal(reader[nameof(ST13)]);
                item.ST14 = Convert.ToDecimal(reader[nameof(ST14)]);
                item.ST15 = Convert.ToDecimal(reader[nameof(ST15)]);
                item.ST16 = Convert.ToDecimal(reader[nameof(ST16)]);
                item.ST17 = Convert.ToDecimal(reader[nameof(ST17)]);
                item.ST18 = Convert.ToDecimal(reader[nameof(ST18)]);
                item.ST19 = Convert.ToDecimal(reader[nameof(ST19)]);
                item.ST20 = Convert.ToDecimal(reader[nameof(ST20)]);
                item.ST21 = Convert.ToDecimal(reader[nameof(ST21)]);
                item.ST22 = Convert.ToDecimal(reader[nameof(ST22)]);
                item.ST23 = Convert.ToDecimal(reader[nameof(ST23)]);
                item.ST24 = Convert.ToDecimal(reader[nameof(ST24)]);
                item.ST25 = Convert.ToDecimal(reader[nameof(ST25)]);
                item.ST26 = Convert.ToDecimal(reader[nameof(ST26)]);
                item.ST27 = Convert.ToDecimal(reader[nameof(ST27)]);
                item.ST28 = Convert.ToDecimal(reader[nameof(ST28)]);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения ProfDetRow: {e.Message}", e);
            }
        }


        public string SMO { get; set; }
        public decimal ST2 { get; set; }
        public decimal ST3 { get; set; }
        public decimal ST4 { get; set; }
        public decimal ST5 { get; set; }
        public decimal ST6 { get; set; }
        public decimal ST7 { get; set; }
        public decimal ST8 { get; set; }
        public decimal ST9 { get; set; }
        public decimal ST10 { get; set; }
        public decimal ST11 { get; set; }
        public decimal ST12 { get; set; }
        public decimal ST13 { get; set; }
        public decimal ST14 { get; set; }
        public decimal ST15 { get; set; }
        public decimal ST16 { get; set; }
        public decimal ST17 { get; set; }
        public decimal ST18 { get; set; }
        public decimal ST19 { get; set; }
        public decimal ST20 { get; set; }
        public decimal ST21 { get; set; }
        public decimal ST22 { get; set; }
        public decimal ST23 { get; set; }
        public decimal ST24 { get; set; }
        public decimal ST25 { get; set; }
        public decimal ST26 { get; set; }
        public decimal ST27 { get; set; }
        public decimal ST28 { get; set; }

    }



    public class Kv2MtrRow
    {
        public static List<Kv2MtrRow> GetList(IDataReader reader)
        {
            var result = new List<Kv2MtrRow>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static Kv2MtrRow Get(IDataReader reader)
        {
            try
            {
                var item = new Kv2MtrRow();
                item.NN = Convert.ToString(reader[nameof(NN)]);
                item.KOL = Convert.ToInt32(reader[nameof(KOL)]);
                item.SUM = Convert.ToDecimal(reader[nameof(SUM)]);
                ReadNNItem(item);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения Kv2MtrRow: {e.Message}", e);
            }
        }
        private static void ReadNNItem(Kv2MtrRow item)
        {
            switch (item.NN)
            {
                case "1":
                    item.ED = "х";
                    item.NAIM = "Всего на медицинскую помощь, в том числе:"; break;
                case "1.1":
                    item.ED = "х";
                    item.NAIM = "оказание медицинской помощи пациентам с инфекционными заболеваниями органов дыхания, пневмонии, короновирусной инфекции(U07.1, U07.2, Z03.8, Z22.8, Z20.8, Z11.5, B34.2, B33.8, J12-J18), всего, из них:"; break;
                case "1.1.1":
                    item.ED = "х";
                    item.NAIM = "U07.1, U07.2"; break;
                case "2":
                    item.ED = "х";
                    item.NAIM = "медицинская помощь в амбулаторных условиях, всего, в том числе"; break;
                case "2.1":
                    item.ED = "исследования";
                    item.NAIM = "тестирование на новую короновирусную инфекцию"; break;
                case "2.2":
                    item.ED = "посещения";
                    item.NAIM = "неотложная медицинская помощь, всего, в том числе:"; break;
                case "2.2.1":
                    item.ED = "посещения";
                    item.NAIM = "оказание медицинской помощи пациентам с инфекционными заболеваниями органов дыхания, пневмонии, короновирусной инфекции(U07.1, U07.2, Z03.8, Z22.8, Z20.8, Z11.5, B34.2, B33.8, J12-J18), всего, из них:"; break;
                case "2.2.2":
                    item.ED = "посещения";
                    item.NAIM = "U07.1, U07.2"; break;
                case "3":
                    item.ED = "случай госпитализации";
                    item.NAIM = "Медицинская помощь в условиях круглосуточного стационара, всего, в том числе:"; break;
                case "3.1":
                    item.ED = "случай госпитализации";
                    item.NAIM = "госпитализации пациентов с инфекционными заболеваниями органов дыхания, пневмонии, короновирусной инфекции(U07.1, U07.2, Z03.8, Z22.8, Z20.8, Z11.5, B34.2, B33.8, J12-J18), всего, из них:"; break;
                case "3.1.1":
                    item.ED = "случай госпитализации";
                    item.NAIM = "U07.1, U07.2"; break;
                case "3.1.1.1":
                    item.ED = "случай госпитализации";
                    item.NAIM = "количество случаев экстракорпоральной мембранной оксигенации(ЭКМО)"; break;
                case "4":
                    item.ED = "вызов";
                    item.NAIM = "Скорая медицинская помощь, всего,в том числе"; break;
                case "4.1":
                    item.ED = "вызов";
                    item.NAIM = "оказание медицинской помощи пациентам с инфекционными заболеваниями органов дыхания, пневмонии, короновирусной инфекции(U07.1, U07.2, Z03.8, Z22.8, Z20.8, Z11.5, B34.2, B33.8, J12-J18),всего, из них:"; break;
                case "4.1.1":
                    item.ED = "вызов";
                    item.NAIM = "U07.1, U07.2"; break;
                default:
                    throw new Exception($"Строка с номером \"{item.NN}\" не ожидается");

            }
        }
        public string NAIM { get;  set; }
        public string ED { get;  set; }
        public string NN { get;  set; }
        public int KOL { get; set; }
        public decimal SUM { get; set; }

    }

    public class DliTbl1Row
    {
        public static List<DliTbl1Row> GetList(IDataReader reader)
        {
            var result = new List<DliTbl1Row>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static DliTbl1Row Get(IDataReader reader)
        {
            try
            {
                var item = new DliTbl1Row();
                item.SMO = Convert.ToString(reader[nameof(SMO)]);
                item.NAME = Convert.ToString(reader[nameof(NAME)]);
                item.VID = Convert.ToString(reader[nameof(VID)]);
                item.K = Convert.ToInt32(reader[nameof(K)]);
                item.ENP = Convert.ToInt32(reader[nameof(ENP)]);
                item.S = Convert.ToDecimal(reader[nameof(S)]);
                item.K_MTR = Convert.ToInt32(reader[nameof(K_MTR)]);
                item.ENP_MTR = Convert.ToInt32(reader[nameof(ENP_MTR)]);
                item.S_MTR = Convert.ToDecimal(reader[nameof(S_MTR)]);
                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения DliTbl1Row: {e.Message}", e);
            }
        }

        public string SMO { get; set; }
        public string NAME { get; set; }
        public string VID { get; set; }
        public int K { get; set; }
        public int ENP { get; set; }
        public decimal S { get; set; }
        public int K_MTR { get; set; }
        public int ENP_MTR { get; set; }
        public decimal S_MTR { get; set; }
    }

    public class DliTbl2Row
    {
        public static List<DliTbl2Row> GetList(IDataReader reader)
        {
            var result = new List<DliTbl2Row>();
            while (reader.Read())
            {
                result.Add(Get(reader));
            }
            return result;
        }
        public static DliTbl2Row Get(IDataReader reader)
        {
            try
            {
                var item = new DliTbl2Row();
                item.NAME_TFK = Convert.ToString(reader[nameof(NAME_TFK)]);
                item.OKRUG = Convert.ToInt32(reader[nameof(OKRUG)]);
                item.C_KT = Convert.ToInt32(reader[nameof(C_KT)]);
                item.E_KT = Convert.ToInt32(reader[nameof(E_KT)]);
                item.S_KT = Convert.ToDecimal(reader[nameof(S_KT)]);

                item.C_MRT = Convert.ToInt32(reader[nameof(C_MRT)]);
                item.E_MRT = Convert.ToInt32(reader[nameof(E_MRT)]);
                item.S_MRT = Convert.ToDecimal(reader[nameof(S_MRT)]);

                item.C_USI = Convert.ToInt32(reader[nameof(C_USI)]);
                item.E_USI = Convert.ToInt32(reader[nameof(E_USI)]);
                item.S_USI = Convert.ToDecimal(reader[nameof(S_USI)]);

                item.C_ENDO = Convert.ToInt32(reader[nameof(C_ENDO)]);
                item.E_ENDO = Convert.ToInt32(reader[nameof(E_ENDO)]);
                item.S_ENDO = Convert.ToDecimal(reader[nameof(S_ENDO)]);

                item.C_MOL = Convert.ToInt32(reader[nameof(C_MOL)]);
                item.E_MOL = Convert.ToInt32(reader[nameof(E_MOL)]);
                item.S_MOL = Convert.ToDecimal(reader[nameof(S_MOL)]);

                item.C_GIST = Convert.ToInt32(reader[nameof(C_GIST)]);
                item.E_GIST = Convert.ToInt32(reader[nameof(E_GIST)]);
                item.S_GIST = Convert.ToDecimal(reader[nameof(S_GIST)]);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Ошибка получения DliTbl2Row: {e.Message}", e);
            }
        }

        public string NAME_TFK { get; set; }
        public int OKRUG { get; set; }

        public int C_KT { get; set; }
        public int E_KT { get; set; }
        public decimal S_KT { get; set; }

        public int C_MRT { get; set; }
        public int E_MRT { get; set; }
        public decimal S_MRT { get; set; }


        public int C_USI { get; set; }
        public int E_USI { get; set; }
        public decimal S_USI { get; set; }


        public int C_ENDO { get; set; }
        public int E_ENDO { get; set; }
        public decimal S_ENDO { get; set; }


        public int C_MOL { get; set; }
        public int E_MOL { get; set; }
        public decimal S_MOL { get; set; }

        public int C_GIST { get; set; }
        public int E_GIST { get; set; }
        public decimal S_GIST { get; set; }

    }

    public  class   DliRecord
    {
        public List<DliTbl1Row> tbl1 { get; set; } = new();
        public List<DliTbl2Row> tbl2 { get; set; } = new();
       
    }


    [Table("FILEPACK")]
    public class FILEPACK
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
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
        public FILES(int? iD_FILEL, int iD_PACK, STATUS_FILE sTATUS)
        {
            ID_FILEL = iD_FILEL;
            ID_PACK = iD_PACK;
            STATUS = sTATUS;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get;  set; }
        public string FILENAME { get; set; }
        public DateTime? DATECREATE { get; set; }
        public int? ID_FILEL { get; set; }
        public int ID_PACK { get; set; }
        public STATUS_FILE STATUS { get; set; }
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
        LC = 21,
        DA = 22,
        DB = 23,
        LA = 24,
        LB = 25
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
        public int ID_NEW { get; set; }
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
        public int ID_ERR { get; set; }
        [Required]
        public string EXAMPLE { get; set; }
        public byte[] TEXT { get; set; }
        public string OSN_TFOMS { get; set; }
        public DateTime D_EDIT { get; set; } = DateTime.Now;
        public DateTime D_BEGIN { get; set; }
        public DateTime? D_END { get; set; }
        public bool ISMEK { get; set; }

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


    [Table("SIGN_LIST")]
    public class SIGN_LIST
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required] 
        public string PUBLICKEY { get; set; }
        [Required]
        public string PUBLICKEY_ISSUER { get; set; }
        [Required]
        public string CODE_MO { get; set; }
        [Required]
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
        [Required]
        public int SIGN_ROLE_ID { get; set; }
        [Required]
        [MaxLength(2*1024*1024)]
        public byte[] FILE_CERT { get; set; }
        [MaxLength(20 * 1024 * 1024)]
        [Required]
        public byte[] FILE_CONFIRM { get; set; }
        [Required]
        public string FILE_CONFIRM_EXT { get; set; }

        public virtual SIGN_ROLE ROLE { get; set; }

        public virtual CODE_MO MO_NAME { get; set; }
       
    }

    [Table("SIGN_ROLE")]
    public class SIGN_ROLE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SIGN_ROLE_ID { get; set; }
        [Required]
        public string CAPTION { get; set; }
        [Required]
        public string PREFIX { get; set; }
        public virtual  ICollection<SIGN_LIST> Signs { get; set; }
    }
    [Table("SIGN_ISSUER")]
    public class SIGN_ISSUER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SING_ISSUER_ID { get; set; }
        [Required]
        public string CAPTION { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
        public byte[] FILE_CERT { get; set; }

        public string PUBLICKEY { get; set; }
    }
    [Table("DOC_SIGN")]
    public class DOC_SIGN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DOC_SIGN_ID { get; set; }
        public int DOC_FOR_SIGN_ID { get; set; }
        public byte[] SIGN { get; set; }
        public int SIGN_ROLE_ID { get; set; }
        public int? SIGN_LIST_ID { get; set; }
        public DateTime DATE_CREATE { get; set; } = DateTime.Now;
        public  DateTime? DATE_SIGN { get; set; }
        public virtual DOC_FOR_SIGN DOC { get; set; }
        public virtual SIGN_ROLE ROLE { get; set; }
        public virtual SIGN_LIST SIGN_ITEM { get; set; }
    }

    [Table("DOC_FOR_SIGN")]
    public class DOC_FOR_SIGN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DOC_FOR_SIGN_ID { get; set; }
        [Column(TypeName = "BLOB")]
        public byte[] DATA { get; set; }
        public string FILENAME { get; set; }
        public string CODE_MO { get; set; }
        public DateTime DATE_CREATE { get; set; } = DateTime.Now;
        public int THEME_ID { get; set; }
        public virtual CODE_MO CODE_MO_NAME { get; set; }
        public virtual ICollection<DOC_SIGN> SIGNs { get; set; } = new List<DOC_SIGN>();
        public virtual DOC_THEME THEME { get; set; }
        
    }

    [Table("DOC_THEME")]
    public class DOC_THEME
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int THEME_ID { get; set; }
        public string CAPTION { get; set; }
        public virtual ICollection<DOC_FOR_SIGN> DOCs { get; set; }
    }



    public class ZPZ_EFFECTIVENESS
    {
       
        public string CODE_MO { get; set; }
        public string NAM_MOK { get; set; }
        public decimal COUNT { get; set; }
        public decimal COUNT_OSN { get; set; }
        public decimal DOL_OSN { get; set; }
        public decimal BAL_OSN { get; set; }
        public decimal C_MEE { get; set; }
        public decimal C_MEE_ERR { get; set; }
        public decimal DOL_MEE { get; set; }
        public decimal BAL_MEE { get; set; }
        public decimal C_EKMP { get; set; }
        public decimal C_EKMP_ERR { get; set; }
        public decimal DOL_EKMP { get; set; }
        public decimal BAL_EKMP { get; set; }

        public decimal SUM_BAL => BAL_MEE + BAL_EKMP + BAL_OSN;
        public static List<ZPZ_EFFECTIVENESS> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }
        public static ZPZ_EFFECTIVENESS Get(DataRow row)
        {
            var res = new ZPZ_EFFECTIVENESS
            {
                CODE_MO = row["CODE_MO"].ToString(),
                NAM_MOK = row["NAM_MOK"].ToString(),
                COUNT = Convert.ToDecimal(row["COUNT"]),
                COUNT_OSN = Convert.ToDecimal(row["COUNT_OSN"]),
                DOL_OSN = Convert.ToDecimal(row["DOL_OSN"]),
                BAL_OSN = Convert.ToDecimal(row["BAL_OSN"]),
                C_MEE = Convert.ToDecimal(row["C_MEE"]),
                C_MEE_ERR = Convert.ToDecimal(row["C_MEE_ERR"]),
                DOL_MEE = Convert.ToDecimal(row["DOL_MEE"]),
                BAL_MEE = Convert.ToDecimal(row["BAL_MEE"]),
                C_EKMP = Convert.ToDecimal(row["C_EKMP"]),
                C_EKMP_ERR = Convert.ToDecimal(row["C_EKMP_ERR"]),
                DOL_EKMP = Convert.ToDecimal(row["DOL_EKMP"]),
                BAL_EKMP = Convert.ToDecimal(row["BAL_EKMP"])
            };
            return res;
        }

    }



    
}
