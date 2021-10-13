using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Models;

namespace SiteCore.Data
{

    public class TMKOracleSet : DbContext
    {
        public TMKOracleSet(DbContextOptions<TMKOracleSet> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TMK");
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.NMIC_NAME).WithMany().HasForeignKey(x => x.NMIC);
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.TMIS_NAME).WithMany().HasForeignKey(x => x.TMIS);
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.SMO_NAME).WithMany().HasForeignKey(x => x.SMO);
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.CODE_MO_NAME).WithMany().HasForeignKey(x => x.CODE_MO);
            modelBuilder.Entity<TMKReestr>().HasMany(x => x.Expertizes).WithOne().HasForeignKey(x => x.TMK_ID);
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.N_OPLATA).WithMany().HasForeignKey(x => x.OPLATA);
            modelBuilder.Entity<TMKReestr>().HasOne(x => x.N_VID_NHISTORY).WithMany().HasForeignKey(x => x.VID_NHISTORY);
            modelBuilder.Entity<TMKReestr>().HasMany(x => x.CONTACT_INFO).WithOne().HasForeignKey(x => x.CODE_MO).HasPrincipalKey(x => x.CODE_MO);
            modelBuilder.Entity<TMKReestr>().Property(x => x.SMO).IsUnicode(false);
            
            modelBuilder.Entity<CONTACT_INFO>().HasOne(x => x.CODE_MO_NAME).WithMany().HasForeignKey(x => x.CODE_MO).HasPrincipalKey(x => x.MCOD);
            modelBuilder.Entity<CODE_MO>().ToTable("F003", "NSI");
            modelBuilder.Entity<SMO>().Property(x => x.NAM_SMOK).IsUnicode(false);
            

            modelBuilder.Entity<TMKReestRExpertize>().HasOne(x => x.CELL_NAME).WithMany().HasForeignKey(x => x.CELL);
            modelBuilder.Entity<TMKReestRExpertize>().HasOne(x => x.FULL_NAME).WithMany().HasForeignKey(x => x.FULL);
            modelBuilder.Entity<TMKReestRExpertize>().HasMany(x => x.OSN).WithOne(x=>x.Expertize).HasForeignKey(x => x.EXPERTIZE_ID);
            modelBuilder.Entity<TMKReestRExpertize>().HasOne(x => x.TMKReestr).WithMany(x=>x.Expertizes).HasForeignKey(x => x.TMK_ID);


            modelBuilder.Entity<F014>().HasKey(x=>new {x.KOD,x.DATEBEG});




    }
        public virtual DbSet<TMKReestr> TMKReestr { get; set; }
        public virtual DbSet<V002> V002 { get; set; }
        public virtual DbSet<CODE_MO> CODE_MO { get; set; }
        public virtual DbSet<TMIS> TMIS { get; set; }
        public virtual DbSet<NMIC> NMIC { get; set; }
        public virtual DbSet<NMIC_CELL> NMIC_CELL { get; set; }
        public virtual DbSet<NMIC_FULL> NMIC_FULL { get; set; }
        public virtual DbSet<EXPERTS> EXPERTS { get; set; }
        public virtual DbSet<TMKReestRExpertize> Expertizes { get; set; }
        public virtual DbSet<EXPERTIZE_OSN> OSN { get; set; }
        public virtual DbSet<NMIC_OPLATA> OPLATA { get; set; }
        public virtual DbSet<NMIC_VID_NHISTORY> VID_NHISTORY { get; set; }
        public virtual DbSet<F014> F014 { get; set; }

        public List<ReportTMKRow> GetReport(DateTime date1, DateTime date2, bool isMO, bool isSMO, bool isNMIC, string SMO, int[] VID_HISTORY)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select * from table(NMIC_REPORT.report(:date1, :date2, :isMO, :isSMO,:isNMIC, :SMO,{VID_HISTORY.ToOraParametr() ?? new[] { -1 }.ToOraParametr() }))", this.Database.GetConnectionString());
            oda.SelectCommand.Parameters.Add("date1", date1.Date);
            oda.SelectCommand.Parameters.Add("date2", date2.Date);
            oda.SelectCommand.Parameters.Add("isMO", isMO ? 1 : 0);
            oda.SelectCommand.Parameters.Add("isSMO", isSMO ? 1 : 0);
            oda.SelectCommand.Parameters.Add("isNMIC", isNMIC ? 1 : 0);
            oda.SelectCommand.Parameters.Add("SMO", SMO);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return ReportTMKRow.Get(tbl.Select());
        }

        public List<Report2TMKRow> GetReport2(DateTime date1, DateTime date2, bool isMO, bool isSMO, bool isNMIC, string SMO, int[] VID_HISTORY)
        {
            var oda = new Oracle.ManagedDataAccess.Client.OracleDataAdapter($"select * from table(NMIC_REPORT.report2(:date1, :date2, :isMO, :isSMO,:isNMIC, :SMO,{VID_HISTORY.ToOraParametr() ?? new[] { -1 }.ToOraParametr() }))", this.Database.GetConnectionString());
            oda.SelectCommand.Parameters.Add("date1", date1.Date);
            oda.SelectCommand.Parameters.Add("date2", date2.Date);
            oda.SelectCommand.Parameters.Add("isMO", isMO ? 1 : 0);
            oda.SelectCommand.Parameters.Add("isSMO", isSMO ? 1 : 0);
            oda.SelectCommand.Parameters.Add("isNMIC", isNMIC ? 1 : 0);
            oda.SelectCommand.Parameters.Add("SMO", SMO);
            var tbl = new DataTable();
            oda.Fill(tbl);
            return Report2TMKRow.Get(tbl.Select());
        }

        public virtual DbSet<CONTACT_INFO> CONTACT_INFO { get; set; }


    }
    [Table("TMKREESTR")]
    public class TMKReestr: IValidatableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TMK_ID { get; set; }
        public bool NOVOR { get; set; }
        public bool ISNOTSMO { get; set; }
        private string enp;
        [MaxLength(16, ErrorMessage = "ЕНП не может быть больше 16 символов")]
        [RequiredIf(nameof(ISNOTSMO), new object[] { false }, ErrorMessage = "Поле \"ЕНП\" обязательно к заполнению")]
        public string ENP
        {
            get => ISNOTSMO ? "НЕТ" : enp;
            set => enp = value;
        }
        [Required(ErrorMessage = "Поле \"№ истории\" обязательно к заполнению")]
        [MaxLength(16)]
        public string NHISTORY { get; set; }
        [Required(ErrorMessage = "Поле \"Вид медицинской документации\" обязательно к заполнению")]
        public int VID_NHISTORY { get; set; }
        [Required(ErrorMessage = "Поле \"Фамилия\" обязательно к заполнению")]
        [MaxLength(40)]
        public string FAM { get; set; }
        [Required(ErrorMessage = "Поле \"Имя\" обязательно к заполнению")]
        [MaxLength(40)]
        public string IM { get; set; }
        [Required(ErrorMessage = "Поле \"Отчество\" обязательно к заполнению")]
        [MaxLength(40)]
        public string OT { get; set; }
        [Required(ErrorMessage = "Поле \"Дата рождения\" обязательно к заполнению")]
        public DateTime DR { get; set; }
        [RequiredIf(nameof(NOVOR), new object[] { true }, ErrorMessage = "Поле \"Фамилия представителя\" обязательно к заполнению")]
        [MaxLength(40)]
        public string FAM_P { get; set; }
        [RequiredIf(nameof(NOVOR), new object[] { true }, ErrorMessage = "Поле \"Имя представителя\" обязательно к заполнению")]
        [MaxLength(40)]
        public string IM_P { get; set; }
        [RequiredIf(nameof(NOVOR), new object[] { true }, ErrorMessage = "Поле \"Отчество представителя\" обязательно к заполнению")]
        [MaxLength(40)]
        public string OT_P { get; set; }
        [RequiredIf(nameof(NOVOR), new object[] { true }, ErrorMessage = "Поле \"Дата рождения представителя\" обязательно к заполнению")]
        public DateTime? DR_P { get; set; }
        public string CODE_MO { get; set; }
        [Required(ErrorMessage = "Поле \"Профиль\" обязательно к заполнению")]
        public int PROFIL { get; set; }
        [Required(ErrorMessage = "Поле \"Дата лечения\" обязательно к заполнению")]
        public DateTime DATE_B { get; set; }
        [Required(ErrorMessage = "Поле \"Дата запроса\" обязательно к заполнению")]
        public DateTime DATE_QUERY { get; set; }

        [Required(ErrorMessage = "Поле \"Дата протокола ТМК\" обязательно к заполнению")]
        public DateTime DATE_PROTOKOL { get; set; }
        public DateTime? DATE_TMK { get; set; }
        [Required(ErrorMessage = "Поле \"НМИЦ\" обязательно к заполнению")]
        public int NMIC { get; set; }
        public virtual NMIC NMIC_NAME { get; set; }
        [Required(ErrorMessage = "Поле \"Телемедицинская система\" обязательно к заполнению")]
        public int TMIS { get; set; }
        public virtual TMIS TMIS_NAME { get; set; }
        public virtual CODE_MO CODE_MO_NAME { get; set; }
        public virtual SMO SMO_NAME { get; set; }
        public StatusTMKRow STATUS { get; set; }
        public string STATUS_COM { get; set; } = "";
        public string USER_ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DATE_INVITE { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DATE_EDIT { get; set; }
    
        public string SMO { get; set; }
        public string SMO_COM { get; set; }
        public string FIO => $"{FAM.ToUpper()} {IM.ToUpper()[0]}.{OT.ToUpper()[0]}.";
        public virtual ICollection<TMKReestRExpertize> Expertizes { get; set; }
        public int OPLATA { get; set; } = 0;
        public virtual NMIC_OPLATA N_OPLATA { get; set; }
        public virtual NMIC_VID_NHISTORY N_VID_NHISTORY { get; set; }
        public virtual ICollection<CONTACT_INFO> CONTACT_INFO { get; set; } = new List<CONTACT_INFO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Дата начала лечения не может быть больше даты запроса на ТМК");
            yield return new ValidationResult("Дата начала лечения не может быть больше даты запроса на ТМК");
        
            if (DATE_B > DATE_QUERY)
                yield return new ValidationResult("Дата начала лечения не может быть больше даты запроса на ТМК");
            if (DATE_QUERY > DATE_PROTOKOL)
                yield return new ValidationResult("Дата запроса на ТМК не может быть больше даты протокола");
            if (DATE_TMK > DATE_QUERY)
                yield return new ValidationResult("Дата проведения очной консультации\\консилиума	не может быть больше даты запроса на ТМК");
            if (!string.IsNullOrEmpty(ENP) && !ISNOTSMO)
            {
                var patternV1 = (@"^[0-9]{16}$|^[0-9]{9}$|^[a-zA-Z0-9А-Яа-я]+ [a-zA-Z0-9А-Яа-я]+$");

                if (!Regex.IsMatch(ENP, patternV1))
                {
                    yield return new ValidationResult("ЕНП не соответствует ожидаемому формату");
                }
            }
            var patternRUS = (@"^[А-Яа-яЕё\s-]+");
            if (!Regex.IsMatch(FAM ?? "", patternRUS))
            {
                yield return new ValidationResult("Фамилия содержит не допустимые символы, допустимые: [символы русского алфавита;тире;пробел]");
            }
            if (!Regex.IsMatch(IM ?? "", patternRUS))
            {
                yield return new ValidationResult("Имя содержит не допустимые символы, допустимые: [символы русского алфавита;тире;пробел]");
            }
            if (!Regex.IsMatch(OT ?? "", patternRUS))
            {
                yield return new ValidationResult("Отчество содержит не допустимые символы, допустимые: [символы русского алфавита;тире;пробел]");
            }

            if (DR > DateTime.Now.Date || DR < new DateTime(1900, 1, 1))
            {
                yield return new ValidationResult("Дата рождения не может быть больше текущей даты или позже 1900 года");
            }
            if (DATE_B > DateTime.Now.Date || DATE_B < new DateTime(2018, 1, 1))
            {
                yield return new ValidationResult("Дата начала лечения не может быть больше текущей даты или позже 2018 года");
            }
            if (DATE_PROTOKOL > DateTime.Now.Date || DATE_PROTOKOL < new DateTime(2018, 1, 1))
            {
                yield return new ValidationResult("Дата протокола не может быть больше текущей даты или позже 2018 года");
            }
            if (DATE_QUERY > DateTime.Now.Date || DATE_QUERY < new DateTime(2018, 1, 1))
            {
                yield return new ValidationResult("Дата запроса на ТМК не может быть больше текущей даты или позже 2018 года");
            }
            if (DATE_TMK > DateTime.Now.Date || DATE_TMK < new DateTime(2018, 1, 1))
            {
                yield return new ValidationResult("Дата проведения очной консультации\\консилиума не может быть больше текущей даты или позже 2018 года");
            }
        }
    }

    public enum StatusTMKRow
    {
        Open = 0,
        Closed = 1,
        Error = -1
    }
    [Table("V002", Schema = "NSI")]
    public class V002
    {
        [Key]
        public int IDPR { get; set; }
        public string PRNAME { get; set; }
        public DateTime DATEBEG { get; set; }
        public DateTime? DATEEND { get; set; }
    }

    [Table("TMIS", Schema = "NSI")]
    public class TMIS
    {
        [Key]
        public int TMIS_ID { get; set; }
        public string TMS_NAME { get; set; }
    }

    [Table("NMIC", Schema = "NSI")]
    public class NMIC
    {
        [Key]
        public int NMIC_ID { get; set; }
        public string NMIC_NAME { get; set; }
    }
    public enum ExpertTip
    {
        MEK = 1,
        MEE = 2,
        EKMP = 3
    }
    [Table("TMKREESTREXPERTIZE")]
    public class TMKReestRExpertize
    {
        public void CopyTo(TMKReestRExpertize exp)
        {
            exp.CELL = this.CELL;
            exp.DATEACT = this.DATEACT;
            exp.FIO = this.FIO;
            exp.FULL = this.FULL;
            exp.ISCOROLLARY = this.ISCOROLLARY;
            exp.ISNOTRECOMMEND = this.ISNOTRECOMMEND;
            exp.ISOSN = this.ISOSN;
            exp.ISRECOMMENDMEDDOC = this.ISRECOMMENDMEDDOC;
            exp.NOTPERFORM = this.NOTPERFORM;
            exp.N_EXP = this.N_EXP;
            exp.DATEACT = this.DATEACT;
            exp.S_TIP = this.S_TIP;
            exp.FULL = this.FULL;
            exp.FIO = this.FIO;
            exp.NUMACT = this.NUMACT;
            var x = 0;
            foreach (var osn in this.OSN)
            {
                if (x == exp.OSN.Count)
                {
                    exp.OSN.Add(osn);
                }
                else
                {
                    osn.CopyTo(exp.OSN.First(z => z.OSN_ID == exp.OSN.ToList()[x].OSN_ID));
                }
                x++;
            }
        }
        /// <summary>
        /// Указатель на запись в реестре
        /// </summary>
        [Required(ErrorMessage = "Поле \"TMK_ID\" обязательно к заполнению")]
        public int TMK_ID { get; set; }
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
        /// Наличие заключения мед. работника
        /// </summary>

        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.MEE, ExpertTip.EKMP }, ErrorMessage = "Поле \"Наличие заключения мед. работника\" обязательно к заполнению")]
        public bool? ISCOROLLARY { get; set; }
        /// <summary>
        /// Цель консультации
        /// </summary>
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.MEE, ExpertTip.EKMP }, ErrorMessage = "Поле \"Цель консультации\" обязательно к заполнению")]
        public int? CELL { get; set; }
        /// <summary>
        /// Наличие факта отражения рекомендации
        /// </summary>
        ///
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.MEE }, ErrorMessage = "Поле \"Наличие факта отражения рекомендации\" обязательно к заполнению")]
        public bool? ISRECOMMENDMEDDOC { get; set; }
        /// <summary>
        /// Наличие показаний не позволяющих применить рекомендацию
        /// </summary>
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Наличие показаний не позволяющих применить рекомендацию\" обязательно к заполнению")]
        public bool? ISNOTRECOMMEND { get; set; }
        /// <summary>
        /// Оценка полноты выполнения
        /// </summary>
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Оценка полноты выполнения\" обязательно к заполнению")]
        public int? FULL { get; set; }

        /// <summary>
        ///  Констатировано неисполнение следующего
        /// </summary>
        public string NOTPERFORM { get; set; }

        /// <summary>
        ///  Заключение врача ЭКМП о обоснованности
        /// </summary>
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Заключение врача ЭКМП о обоснованности\" обязательно к заполнению")]
        public bool? ISOSN { get; set; }

        /// <summary>
        ///  ФИО специалиста или врача-эксперта
        /// </summary>
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.MEE }, ErrorMessage = "Поле \"ФИО специалиста или врача-эксперта\" обязательно к заполнению")]
        public string FIO { get; set; }

        /// <summary>
        ///   Код врача-эксперта
        /// </summary>
        ///
        [RequiredIf(nameof(S_TIP), new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Код врача эксперта\" обязательно к заполнению")]
        public string N_EXP { get; set; }
        public virtual NMIC_CELL CELL_NAME { get; set; }
        public virtual NMIC_FULL FULL_NAME { get; set; }
        public List<string> Validate(DateTime DATE_PROTOKOL)
        {
            var res = new List<string>();
            if (!(DATEACT >= DATE_PROTOKOL && DATEACT <= DateTime.Now) && S_TIP != ExpertTip.MEK)
                res.Add("Дата акта не может быть меньше даты протокола или больше текущей даты");

            if (OSN.GroupBy(x => x.S_OSN).Count(x => x.Count() > 1) != 0)
                res.Add("Присутствуют не уникальные дефекты");
            return res;
        }
        public virtual ICollection<EXPERTIZE_OSN> OSN { get; set; } = new List<EXPERTIZE_OSN>();
        public virtual TMKReestr TMKReestr { get; set; }
    }
    [Table("NMIC_CELL", Schema = "NSI")]
    public class NMIC_CELL
    {
        [Key]
        public int CELL { get; set; }
        public string CELL_NAME { get; set; }

    }
    [Table("NMIC_FULL", Schema = "NSI")]
    public class NMIC_FULL
    {
        [Key]
        public int FULL { get; set; }
        public string FULL_NAME { get; set; }

    }

    [Table("EXPERTS", Schema = "NSI")]
    public class EXPERTS
    {
        [Key]
        public string N_EXPERT { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public string FIO => $"{FAM} {IM} {OT}".Trim();
    }

    [Table("EXPERTIZE_OSN")]
    public class EXPERTIZE_OSN
    {

        public void CopyTo(EXPERTIZE_OSN item)
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
        public int? S_OSN { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public string S_COM { get; set; }
        [Required(ErrorMessage = "Поле \"Сумма санкции\" обязательно к заполнению")]
        public decimal S_SUM { get; set; }
        public decimal? S_FINE { get; set; }
        public int EXPERTIZE_ID { get; set; }
        public virtual TMKReestRExpertize Expertize { get; set; }
    }
    [Table("F014",Schema = "NSI")]
    public class F014
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
      
       
        public int KOD { get; set; }

        /// <summary>
        /// Основание отказа
        /// </summary>
        public string OSN { get; set; }

        /// <summary>
        /// Служебный комментарий
        /// </summary>
        public string KOMMENT { get; set; }

        /// <summary>
        /// Дата начала действия записи
        /// </summary>
        ///
       
        public DateTime DATEBEG { get; set; }

        /// <summary>
        /// Дата окончания действия записи
        /// </summary>
        public DateTime? DATEEND { get; set; }

        public string FullName => $"{OSN}-{KOMMENT}";
    }

    [Table("NMIC_OPLATA", Schema = "NSI")]
    public class NMIC_OPLATA
    {
        [Key]
        public int ID_OPLATA { get; set; }
        public string OPLATA { get; set; }

    }
    [Table("NMIC_VID_NHISTORY", Schema = "NSI")]
    public class NMIC_VID_NHISTORY
    {
        [Key]
        public int ID_VID_NHISTORY { get; set; }
        public string VID_NHISTORY { get; set; }
        public int ORD { get; set; }
    }


    [Table("CONTACT_INFO")]
    public class CONTACT_INFO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID_CONTACT_INFO { get; set; }
        [MaxLength(40, ErrorMessage = "Фамилия не может быть больше 40 символов")]
        public string FAM { get; set; }
        [MaxLength(40, ErrorMessage = "Имя не может быть больше 40 символов")]
        public string IM { get; set; }
        [MaxLength(40, ErrorMessage = "Отчество не может быть больше 40 символов")]
        public string OT { get; set; }
        [MaxLength(40, ErrorMessage = "Телефон не может быть больше 40 символов")]
        public string TEL { get; set; }
        [Required(ErrorMessage = "Поле \"Код МО\" обязательно к заполнению")]
        public string CODE_MO { get; set; }
        public virtual CODE_MO CODE_MO_NAME { get; set; }
        [NotMapped]
        public string TelAndFio => ($"{FAM} {IM} {OT} - {TEL}").Replace("  ", " ");
    }



    public static  class TMKOracleSetExt
    {
        public static string ToHTMLStr(this DateTime? value)
        {
            return value.HasValue ? value.Value.ToHTMLStr() : "";
        }

        public static string ToHTMLStr(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }


        public static string ToYesNo(this bool value)
        {
            return value ? "Да" : "Нет";
        }

        public static string ToYesNo(this bool? value)
        {

            return value.HasValue ? ToYesNo(value.Value) : "";
        }

        public static string ToOSN(this bool value)
        {
            return value ? "Обоснованно" : "Необоснованно";
        }
        public static string ToOSN(this bool? value)
        {
            return value.HasValue ? ToOSN(value.Value) : "";
        }
        public static string ToStr(this ExpertTip ex)
        {
            return ex switch
            {
                ExpertTip.EKMP => "ЭКМП",
                ExpertTip.MEE => "МЭЭ",
                ExpertTip.MEK => "МЭК",
                _ => ""
            };
        }

        public static void RemoveRange<T>(this ICollection<T> list, int From, int To)
        {
            var del = new List<T>();
            var items = list.ToList();
            for (var i = From; i <= To; i++)
            {
                del.Add(items[i]);
            }

            foreach (var d in del)
            {
                list.Remove(d);
            }

        }

    }


    public static class ExtOracleParametr
    {
        public static string ToOraParametr(this string value)
        {
            return $"'{value ?? ""}'";
        }
        public static string ToOraParametr(this int value)
        {
            return $"{value.ToString()}";
        }
        public static string ToOraParametr(this int? value)
        {
            return value.HasValue ? value.Value.ToOraParametr() : "null";
        }
        public static string ToOraParametr(this bool value)
        {
            return value ? "1" : "0";
        }
        public static string ToOraParametr(this bool? value)
        {
            return value.HasValue ? $"{value.Value.ToOraParametr()}" : "null";
        }
        public static string ToOraParametr(this string[] value)
        {
            return $"stringArray({string.Join(",", value.Select(x => x.ToOraParametr()))})";
        }

        public static string ToOraParametr(this int[] value)
        {
            if (value == null) return null;
            return $"intArray({string.Join(",", value)})";
        }

        public static string ToOraParametr(this DateTime value)
        {
            return $"'{value:dd.MM.yyyy}'";
        }
        public static string ToOraParametr(this DateTime? value)
        {
            return value.HasValue ? $"{value.Value.ToOraParametr()}" : "null";
        }

        public static string ToOraParametr(this decimal value)
        {
            return value.ToString();
        }
        public static string ToOraParametr(this decimal? value)
        {
            return value.HasValue ? $"{value.Value.ToOraParametr()}" : "null";
        }

        public static string ToOraParametr(this decimal[] value)
        {
            return $"intArray({string.Join(",", value)})";
        }




    }
}
