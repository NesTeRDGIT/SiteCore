using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Models;

namespace SiteCore.Data
{
    public class MSEOracleSet : DbContext
    {
        public MSEOracleSet(DbContextOptions<MSEOracleSet> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MSE");
            modelBuilder.Entity<MSE_TF01>().HasOne(x => x.DS_NAME).WithMany().HasForeignKey(x => x.DS);
            modelBuilder.Entity<MSE_TF01>().HasOne(x => x.SMO_NAME).WithMany().HasForeignKey(x => x.SMO_IDENT);
            modelBuilder.Entity<MSE_TF01>().HasMany(x => x.Sluch).WithOne(x=>x.MSE_TF01).HasForeignKey(x => x.MSE_ID);

            modelBuilder.Entity<MSE_TF01_SLUCH>().HasMany(x => x.Expertizes).WithOne(x => x.MSE_SLUCH).HasForeignKey(x => x.MSE_SLUCH_ID);
            modelBuilder.Entity<MSEExpertize>().HasMany(x => x.OSN).WithOne(x => x.Expertize).HasForeignKey(x => x.EXPERTIZE_ID);

            modelBuilder.Entity<MSE_TF01>().Property(x => x.SMO_IDENT).IsUnicode(false);
        }
       
        public virtual DbSet<MSE_TF01> MSE_TF01 { get; set; }
        public virtual DbSet<EXPERTS> EXPERTS { get; set; }
        public virtual DbSet<MSEExpertize> Expertizes { get; set; }
        public virtual DbSet<MSE_TF01_SLUCH> SLUCH { get; set; }
        public virtual DbSet<F014> F014 { get; set; }
        public virtual DbSet<MSEExpertizeOSN> OSN { get; set; }
        public virtual DbSet<V002> V002 { get; set; }
        public virtual DbSet<MKB_SPR> MKB { get; set; }

        public Task<List<ReportMSERow>> GetReport(DateTime date1, DateTime date2, bool isSMO)
        {
            return Task.Run(() =>
            {
                var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select* from table(MSE_REPORT.report(:date1, :date2, :isSMO))", this.Database.GetConnectionString());
                oda.SelectCommand.Parameters.Add("date1", date1.Date);
                oda.SelectCommand.Parameters.Add("date2", date2.Date);
                oda.SelectCommand.Parameters.Add("isSMO", isSMO ? 1 : 0);
                var tbl = new DataTable();
                oda.Fill(tbl);
                return ReportMSERow.Get(tbl.Select());
            });
        }
    }
    [Table("MSE_TF01")]
    public class MSE_TF01
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ID")]
        public int MSE_ID { get; set; }
        public DateTime DATE_LOAD { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public string SNILS { get; set; }
        public string NAM_MO { get; set; }
        public string REASON_R { get; set; }
        public string N_PROT { get; set; }
        public DateTime D_PROT { get; set; }
        public DateTime D_FORM { get; set; }
        public string SMO_IDENT { get; set; }
        public string ENP_IDENT { get; set; }
        public string DS { get; set; }
        public int? PROFIL { get; set; }
        public string SMO_COM { get; set; }
        public virtual MKB_SPR DS_NAME { get; set; }
        public virtual SMO SMO_NAME { get; set; }
        public virtual ICollection<MSE_TF01_SLUCH> Sluch { get; set; }
        public string FIO => $"{(string.IsNullOrEmpty(FAM) ? "" : FAM.ToUpper())} {(string.IsNullOrEmpty(IM) ? "" : IM[0].ToString().ToUpper())}{(string.IsNullOrEmpty(OT) ? "" : ".")}.{(string.IsNullOrEmpty(OT) ? "" : OT[0].ToString().ToUpper())}.";
    }

    [Table("MSE_TF01_SLUCH")]
    public class MSE_TF01_SLUCH
    {
        public void CopyTo(MSE_TF01_SLUCH sl)
        {
            sl.DATE_1 = this.DATE_1;
            sl.DATE_2 = this.DATE_2;
            sl.N_HISTORY = this.N_HISTORY;
        }
        public int MSE_ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MSE_SLUCH_ID { get; set; }
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public string N_HISTORY { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public string USER_ID { get; set; }
        public virtual ICollection<MSEExpertize> Expertizes { get; set; }
        public virtual MSE_TF01 MSE_TF01 { get; set; }
    }

    [Table("MSE_TF01EXPERTIZE")]
    public class MSEExpertize
    {
        public void CopyFrom(MSE_ExpertizeModel exp)
        {
            DATEACT = exp.DATEACT;
            FIO = exp.FIO;
            N_EXP = exp.N_EXP;
            S_TIP = exp.S_TIP;
            NUMACT = exp.NUMACT;
            var listOSN = OSN.ToList();
            var RemoveOSN = listOSN.Where(x => exp.OSN.Count(y => y.OSN_ID == x.OSN_ID) == 0);
            var AddOSN = exp.OSN.Where(x => !x.OSN_ID.HasValue);
            var UpdateOSN = exp.OSN.Where(x => x.OSN_ID.HasValue);
            foreach (var osn in RemoveOSN)
            {
                OSN.Remove(osn);
            }
            foreach (var osn in AddOSN)
            {
                OSN.Add(new MSEExpertizeOSN()
                {
                    S_COM = osn.S_COM,
                    S_FINE = osn.S_FINE,
                    S_OSN = osn.S_OSN,
                    S_SUM = osn.S_SUM
                });
            }
            foreach (var osn in UpdateOSN)
            {
                var old = OSN.FirstOrDefault(x => x.OSN_ID == osn.OSN_ID);
                if (old != null)
                {
                    old.S_COM = osn.S_COM;
                    old.S_FINE = osn.S_FINE;
                    old.S_OSN = osn.S_OSN;
                    old.S_SUM = osn.S_SUM;
                }
            }
        }
        /// <summary>
        /// Указатель на запись в реестре
        /// </summary>
        [Required(ErrorMessage = "Поле \"MSE_SLUCH_ID\" обязательно к заполнению")]
        public int MSE_SLUCH_ID { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public string USER_ID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EXPERTIZE_ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DATE_INVITE { get; set; }
        /// <summary>
        /// Тип экспертизы 2 МЭЭ, 3 ЭКМП
        /// </summary>

        [Required(ErrorMessage = "Поле \"Тип экспертизы\" обязательно к заполнению")]
        public ExpertTip S_TIP { get; set; }
        /// <summary>
        /// Дата акта
        /// </summary>
        [Required(ErrorMessage = "Поле \"Дата акта\" обязательно к заполнению")]
        public DateTime DATEACT { get; set; }
        /// <summary>
        /// Номер акта
        /// </summary>
        ///
        [Required(ErrorMessage = "Поле \"Номер акта\" обязательно к заполнению")]
        public string NUMACT { get; set; }


        /// <summary>
        ///  ФИО специалиста или врача-эксперта
        /// </summary>
        ///
        [RequiredIf("S_TIP", new object[] { ExpertTip.MEE }, ErrorMessage = "Поле \"ФИО специалиста или врача-эксперта\" обязательно к заполнению")]
        public string FIO { get; set; }

        /// <summary>
        ///   Код врача-эксперта
        /// </summary>
        ///
        [RequiredIf("S_TIP", new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Код врача эксперта\" обязательно к заполнению")]
        public string N_EXP { get; set; }

        public List<string> Validate()
        {
            var res = new List<string>();
            if (OSN.GroupBy(x => x.S_OSN).Count(x => x.Count() > 1) != 0)
                res.Add("Присутствуют не уникальные дефекты");
            return res;
        }
        public virtual ICollection<MSEExpertizeOSN> OSN { get; set; } = new List<MSEExpertizeOSN>();
      
        public virtual MSE_TF01_SLUCH MSE_SLUCH { get; set; }
    }
    [Table("MSE_TF01EXPERTIZE_OSN")]
    public class MSEExpertizeOSN
    {
        public void CopyTo(MSEExpertizeOSN item)
        {
            item.S_OSN = this.S_OSN;
            item.S_SUM = this.S_SUM;
            item.S_COM = this.S_COM;
            item.S_FINE = this.S_FINE;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OSN_ID { get; set; }
        /// <summary>
        /// Код отказа F014
        /// </summary>
        [Required(ErrorMessage = "Поле \"Причина санкции\" обязательно к заполнению")]
        public int S_OSN { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public string S_COM { get; set; }
        [Required(ErrorMessage = "Поле \"Сумма санкции\" обязательно к заполнению")]
        public decimal S_SUM { get; set; }
        public decimal? S_FINE { get; set; }
        public int EXPERTIZE_ID { get; set; }

        /*[NotMapped]
        public string S_OSN_NAME
        {
            get
            {
                using (var context = new MSEOracleSet())
                {
                    var dateB = this.Expertize.DATEACT ?? DateTime.Now.Date;
                    var v2 = context.F014.FirstOrDefault(x => x.KOD == S_OSN && dateB >= x.DATEBEG && dateB <= (x.DATEEND ?? DateTime.Now));
                    return v2 != null ? v2.FullName : $"Нет значения из справочника F014, код = {S_OSN}";
                }
            }
        }

    */
        public virtual MSEExpertize Expertize { get; set; }
    }

    [Table("MKB10",Schema = "NSI")]
    public class MKB_SPR
    {
        [Key]
        public string MKB { get; set; }
        public string NAME { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
    }


   

    public class ReportMSERow
    {
        public static List<ReportMSERow> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static ReportMSERow Get(DataRow row)
        {
            try
            {
                var item = new ReportMSERow
                {
                    SUB = row["SUB"].ToString(),
                    SMO = row["SMO"].ToString(),
                    NAM_SMOK = row["NAM_SMOK"].ToString(),
                    ST8 = Convert.ToInt32(row["ST8"]),
                    ST9 = Convert.ToInt32(row["ST9"]),
                    ST10 = Convert.ToInt32(row["ST10"]),
                    ST11 = Convert.ToInt32(row["ST11"]),
                    ST12 = Convert.ToInt32(row["ST12"]),
                    ST16 = Convert.ToInt32(row["ST16"]),
                    ST17 = Convert.ToInt32(row["ST17"]),
                    ST18 = Convert.ToInt32(row["ST18"]),
                    ST19 = Convert.ToInt32(row["ST19"]),
                    ST20 = Convert.ToInt32(row["ST20"]),
                    S_ALL = Convert.ToInt32(row["S_ALL"]),
                    S_1_1_3 = Convert.ToInt32(row["S_1_1_3"]),
                    S_1_1_4 = Convert.ToInt32(row["S_1_1_4"]),
                    S_1_2_2 = Convert.ToInt32(row["S_1_2_2"]),
                    S_1_3_2 = Convert.ToInt32(row["S_1_3_2"]),
                    S_1_4 = Convert.ToInt32(row["S_1_4"]),
                    S_2_2_1 = Convert.ToInt32(row["S_2_2_1"]),
                    S_2_2_2 = Convert.ToInt32(row["S_2_2_2"]),
                    S_2_4_1 = Convert.ToInt32(row["S_2_4_1"]),
                    S_2_4_2 = Convert.ToInt32(row["S_2_4_2"]),
                    S_3_1 = Convert.ToInt32(row["S_3_1"]),
                    S_3_2_2 = Convert.ToInt32(row["S_3_2_2"]),
                    S_3_2_3 = Convert.ToInt32(row["S_3_2_3"]),
                    S_3_2_4 = Convert.ToInt32(row["S_3_2_4"]),
                    S_3_2_5 = Convert.ToInt32(row["S_3_2_5"]),
                    S_3_3_1 = Convert.ToInt32(row["S_3_3_1"]),
                    S_3_5 = Convert.ToInt32(row["S_3_5"]),
                    S_3_6 = Convert.ToInt32(row["S_3_6"]),
                    S_3_9 = Convert.ToInt32(row["S_3_9"]),
                    S_4_1 = Convert.ToInt32(row["S_4_1"]),
                    S_4_2 = Convert.ToInt32(row["S_4_2"]),
                    S_4_4 = Convert.ToInt32(row["S_4_4"]),
                    S_4_5 = Convert.ToInt32(row["S_4_5"]),
                    S_4_6_1 = Convert.ToInt32(row["S_4_6_1"]),
                    S_4_6_2 = Convert.ToInt32(row["S_4_6_2"]),
                    S_5_3_1 = Convert.ToInt32(row["S_5_3_1"]),
                    S_5_4_1 = Convert.ToInt32(row["S_5_4_1"]),
                    S_5_4_2 = Convert.ToInt32(row["S_5_4_2"]),
                    S_5_5 = Convert.ToInt32(row["S_5_5"]),
                    S_5_6 = Convert.ToInt32(row["S_5_6"]),
                    S_5_7 = Convert.ToInt32(row["S_5_7"])
                };
                return item;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения ReportRow: {ex.Message}", ex);
            }

        }

        public string SUB { get; set; }
        public string SMO { get; set; }
        public string NAM_SMOK { get; set; }

        public int ST8 { get; set; }
        public int ST9 { get; set; }
        public int ST10 { get; set; }
        public decimal ST11 { get; set; }
        public decimal ST12 { get; set; }
        public int ST16 { get; set; }
        public int ST17 { get; set; }
        public int ST18 { get; set; }
        public decimal ST19 { get; set; }
        public decimal ST20 { get; set; }


        public int S_ALL { get; set; }
        public int S_1_1_3 { get; set; }
        public int S_1_1_4 { get; set; }
        public int S_1_2_2 { get; set; }
        public int S_1_3_2 { get; set; }
        public int S_1_4 { get; set; }
        public int S_2_2_1 { get; set; }
        public int S_2_2_2 { get; set; }
        public int S_2_4_1 { get; set; }
        public int S_2_4_2 { get; set; }
        public int S_3_1 { get; set; }
        public int S_3_2_2 { get; set; }
        public int S_3_2_3 { get; set; }
        public int S_3_2_4 { get; set; }
        public int S_3_2_5 { get; set; }
        public int S_3_3_1 { get; set; }
        public int S_3_5 { get; set; }
        public int S_3_6 { get; set; }
        public int S_3_9 { get; set; }
        public int S_4_1 { get; set; }
        public int S_4_2 { get; set; }
        public int S_4_4 { get; set; }
        public int S_4_5 { get; set; }
        public int S_4_6_1 { get; set; }
        public int S_4_6_2 { get; set; }
        public int S_5_3_1 { get; set; }
        public int S_5_4_1 { get; set; }
        public int S_5_4_2 { get; set; }
        public int S_5_5 { get; set; }
        public int S_5_6 { get; set; }
        public int S_5_7 { get; set; }
    }
}
