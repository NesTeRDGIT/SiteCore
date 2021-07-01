using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;

namespace SiteCore.Data
{
    public enum VPOLIS_VALUES
    {
        /// <summary>
        /// Полис ОМС старого образца
        /// </summary>
        OLD = 1,
        /// <summary>
        /// Временное свидетельство в форме бумажного бланка
        /// </summary>
        TEMP_B = 2,
        /// <summary>
        /// Временное свидетельство в форме электронного документа
        /// </summary>
        TEMP_E = 3,
        /// <summary>
        /// Бумажный полис ОМС единого образца
        /// </summary>
        bOMS = 4,
        /// <summary>
        /// Электронный полис ОМС единого образца
        /// </summary>
        eOMS = 5,
        /// <summary>
        /// Полис ОМС в составе универсальной электронной карты
        /// </summary>
        uOMS = 6,
        /// <summary>
        /// Состояние на учёте без полиса ОМС
        /// </summary>
        NOT = 7
    }

    public static class ExtCSOracleSet
    {
        public static string ToRusStr(this VPOLIS_VALUES val)
        {
            return val switch
            {
                VPOLIS_VALUES.OLD => "Полис ОМС старого образца",
                VPOLIS_VALUES.TEMP_B => "Временное свидетельство в форме электронного документа",
                VPOLIS_VALUES.TEMP_E => "Временное свидетельство в форме электронного документа",
                VPOLIS_VALUES.bOMS => "Бумажный полис ОМС единого образца",
                VPOLIS_VALUES.eOMS => "Электронный полис ОМС единого образца",
                VPOLIS_VALUES.uOMS => "Полис ОМС в составе универсальной электронной карты",
                _ => ""
            };
        }

        public static string ToRusStr(this VPOLIS_VALUES? val)
        {
            return val.HasValue ? ToRusStr(val.Value) : "";
        }

        public static string ToRusStr(this StatusCS_LIST val)
        {
            return val switch
            {
                StatusCS_LIST.New => "Новый список",
                StatusCS_LIST.Send => "Список отправлен в ЦС(ожидание ФЛК)",
                StatusCS_LIST.OnSend => "Список в очереди на отправку в ЦС",
                StatusCS_LIST.FLK => "Список отправлен в ЦС(ожидание ответа)",
                StatusCS_LIST.Answer => "Ответ получен",
                StatusCS_LIST.Error => "Ошибка обработки",
                _ => ""
            };
        }

        public static string ToRusStr(this TypeSMO val)
        {
            return val switch
            {
                TypeSMO.OGRN => "ОГРН",
                TypeSMO.RNumber => "Реестровый номер",
                TypeSMO.TFOMS => "ТФОМС",
                _ => ""
            };
        }

    }
    public class CSOracleSet : DbContext
    {
        public CSOracleSet(DbContextOptions<CSOracleSet> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SITE");
            modelBuilder.Entity<CS_LIST>().HasMany(x => x.CS_LIST_IN).WithOne(x => x.CS_LIST).HasForeignKey(x => x.CS_LIST_ID).IsRequired();
            modelBuilder.Entity<CS_LIST_IN>().HasMany(ir => ir.CS_LIST_IN_RESULT).WithOne(i => i.CS_LIST_IN).HasForeignKey(x=>x.CS_LIST_IN_ID).IsRequired();
            modelBuilder.Entity<CS_LIST_IN_RESULT>().HasMany(x => x.CS_LIST_IN_RESULT_SMO).WithOne(x => x.CS_LIST_IN_RESULT).HasForeignKey(x=>x.CS_LIST_IN_RES_ID).IsRequired();
            modelBuilder.Entity<CS_LIST_IN>().HasOne(x => x.DocF011).WithOne().HasForeignKey<CS_LIST_IN>(x => x.DOC_TYPE);
            modelBuilder.Entity<F011>().Property(x=>x.IDDOC).IsUnicode(false);
        }
        public virtual DbSet<F011> F011 { get; set; }
        public virtual DbSet<TFOMS_P> TFOMS_P { get; set; }
        public virtual DbSet<CS_LIST> CS_LIST { get; set; }
        public virtual DbSet<CS_LIST_IN> CS_LIST_IN { get; set; }
        public virtual DbSet<CS_LIST_IN_RESULT> CS_LIST_IN_RESULT { get; set; }
        public virtual DbSet<CS_LIST_IN_RESULT_SMO> CS_LIST_IN_RESULT_SMO { get; set; }

        public virtual DbSet<SMO> F002 { get; set; }

        public async Task<string> FindNameSMO(TypeSMO TS, string SMO_COD, string SMO_OK)
        {
            if (string.IsNullOrEmpty(SMO_COD)) return "";
            return TS switch
            {
                TypeSMO.OGRN => (await F002.FirstOrDefaultAsync(x => x.OGRN == SMO_COD && x.TF_OKATO == SMO_OK)).NAM_SMOK,
                TypeSMO.RNumber => (await F002.FirstOrDefaultAsync(x => x.SMOCOD == SMO_COD)).NAM_SMOK,
                TypeSMO.TFOMS => (await TFOMS_P.FirstOrDefaultAsync(x => x.TF_OKATO == SMO_COD)).NAME_TFK,
                _ => ""
            };
        }
    }

    [Table("F011", Schema = "NSI")]
    public class F011
    {
        [Key]
        public string IDDOC { get; set; }
        public string DOCNAME { get; set; }
    }
    [Table("TFOMS_P", Schema = "NSI")]
    public class TFOMS_P
    {
        [Key]
        public string TF_OKATO { get; set; }
        public string NAME_TFK { get; set; }

    }
    [Table("F002", Schema = "NSI")]
    public class SMO
    {
        [Key]
        public string SMOCOD { get; set; }
        public string NAM_SMOK { get; set; }
        public string OGRN { get; set; }
        public string TF_OKATO { get; set; }

    }
    public enum StatusCS_LIST
    {
        /// <summary>
        /// Новый список
        /// </summary>
        New = 0,
        /// <summary>
        /// На отправку
        /// </summary>
        OnSend = 1,
        /// <summary>
        /// Отправлен
        /// </summary>
        Send = 2,
        /// <summary>
        /// ФЛК получен
        /// </summary>
        FLK = 3,
        /// <summary>
        /// Ответ получен
        /// </summary>
        Answer = 4,
        /// <summary>
        /// Ошибка обработки
        /// </summary>
        Error = 5
    }

    [Table("CS_LIST")]
    public class CS_LIST
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CS_LIST_ID { get; set; }

        /// <summary>
        /// Код МО
        /// </summary>
        public string CODE_MO { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime DATE_CREATE { get; set; }

        /// <summary>
        /// Статус обработки
        /// </summary>
        public StatusCS_LIST STATUS { get; set; } = StatusCS_LIST.New;
        /// <summary>
        /// Заголовок
        /// </summary>
        public string CAPTION { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public string COMM { get; set; }

        public virtual ICollection<CS_LIST_IN> CS_LIST_IN { get; set; }

    }

    [Table("CS_LIST_IN")]
    public class CS_LIST_IN
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CS_LIST_IN_ID { get; set; }
        /// <summary>
        /// Идентификатор
        /// </summary>
        [ForeignKey("CS_LIST")]
        public int CS_LIST_ID { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string FAM { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string IM { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string OT { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        [Required(ErrorMessage = "Поле \"Дата рождения\" обязательно к заполнению")]
        public DateTime? DR { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        [Required(ErrorMessage = "Поле \"Пол\" обязательно к заполнению")]
        [Range(minimum:1, maximum:2, ErrorMessage = "Поле \"Пол\" должно иметь значение 1 или 2")]
        public int? W { get; set; }
        /// <summary>
        /// Тип док-та
        /// </summary>
        public string DOC_TYPE { get; set; }
        public virtual F011 DocF011 { get; set; }
        /// <summary>
        /// Серия док-та
        /// </summary>
        public string DOC_SER { get; set; }
        /// <summary>
        /// Номер док-та
        /// </summary>
        [RequiredIf("DOC_TYPE", new object[] { null, "" }, IsInverted = true, ErrorMessage = "Поле \"Номер документа\" обязателен для заполнения")]
        public string DOC_NUM { get; set; }
        /// <summary>
        /// Тип полиса
        /// </summary>
        public VPOLIS_VALUES? VPOLIS { get; set; }
        /// <summary>
        /// Серия полиса
        /// </summary>
        [RequiredIf("VPOLIS", new object[] { 1 }, ErrorMessage = "Поле \"Серия полиса\" обязателен для заполнения")]
        public string SPOLIS { get; set; }
        /// <summary>
        /// Номер полиса
        /// </summary>
        [RequiredIf("VPOLIS", new object[] { null }, IsInverted = true, ErrorMessage = "Поле \"Номер полиса\" обязателен для заполнения")]
        public string NPOLIS { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public string SNILS { get; set; }

        public bool? STATUS { get; set; }

        public string COMM { get; set; }
        public virtual CS_LIST CS_LIST { get; set; }

        public virtual ICollection<CS_LIST_IN_RESULT> CS_LIST_IN_RESULT { get; set; }

        public List<string> IsValid
        {
            get
            {
                var err = new List<string>();
                if (string.IsNullOrEmpty(FAM) && string.IsNullOrEmpty(IM) && string.IsNullOrEmpty(OT))
                {
                    err.Add("Минимум один из компонентов ФИО должен быть заполнен!");
                }

                if (!VPOLIS.HasValue && string.IsNullOrEmpty(DOC_TYPE) && string.IsNullOrEmpty(SNILS))
                {
                    err.Add("Минимум один из компонентов ПОЛИС-ДОКУМЕНТ-СНИЛС должен быть заполнен!");
                }

                return err;
            }
        }


        public string POLIS => VPOLIS.HasValue ? $"{SPOLIS} {NPOLIS} [{VPOLIS.Value.ToRusStr()}]".Trim() : "";

        public string DOC => !string.IsNullOrEmpty(DOC_TYPE) ? $"{DOC_SER} {DOC_NUM} [{DocF011?.DOCNAME ?? DOC_TYPE}]".Trim() : "";

        public string STATUS_RUS => STATUS == null ? "Нет данных" : STATUS == true ? "Найден в ЦС" : "Не найден в ЦС";

        public string FIO => $"{FAM} {IM} {OT}".Trim();

        public CS_LIST_IN_RESULT_SMO CurrentSMO => CS_LIST_IN_RESULT?.SelectMany(x => x.CS_LIST_IN_RESULT_SMO).OrderByDescending(x => x.DATE_E ?? DateTime.Now).FirstOrDefault();


        public void CopyFrom(CS_LIST_IN item)
        {
            FAM = item.FAM;
            IM = item.IM;
            OT = item.OT;
            DR = item.DR;
            W = item.W;
            DOC_TYPE = item.DOC_TYPE;
            DOC_SER = item.DOC_SER;
            DOC_NUM = item.DOC_NUM;
            VPOLIS = item.VPOLIS;
            SPOLIS = item.SPOLIS;
            NPOLIS = item.NPOLIS;
            SNILS = item.SNILS;
        }


    }
    [Table("CS_LIST_IN_RESULT")]
    public class CS_LIST_IN_RESULT
    {
        [Key]
        public int CS_LIST_IN_RES_ID { get; set; }
        /// <summary>
        /// Внешний ключ на CS_LIST_IN
        /// </summary>

        public int CS_LIST_IN_ID { get; set; }
     
        public virtual CS_LIST_IN CS_LIST_IN { get; set; }

        /// <summary>
        /// Главный ЕНП
        /// </summary>
        public string ENP { get; set; }
        /// <summary>
        /// Дата смерти
        /// </summary>
        public DateTime? DDEATH { get; set; }
        //Дата рождения
        public DateTime? DR { get; set; }
        /// <summary>
        /// Уровень доверия к информации, возвращённой в ответ на запрос
        /// </summary>
        public string LVL_D { get; set; }
        /// <summary>
        /// Коды псевдонимизированных идентификаторов и ключей поиска, используемых для идентификации застрахованного лица
        /// </summary>
        public string LVL_D_KOD { get; set; }

        public virtual ICollection<CS_LIST_IN_RESULT_SMO> CS_LIST_IN_RESULT_SMO { get; set; } = new List<CS_LIST_IN_RESULT_SMO>();
    }

    public enum TypeSMO
    {
        OGRN = 0,
        RNumber = 1,
        TFOMS = 2
    }

    [Table("CS_LIST_IN_RESULT_SMO")]
    public class CS_LIST_IN_RESULT_SMO
    {
        public int CS_LIST_IN_RES_ID { get; set; }

        public virtual CS_LIST_IN_RESULT CS_LIST_IN_RESULT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CS_LIST_IN_RESULT_SMO_ID { get; set; }
        /// <summary>
        /// ЕНП
        /// </summary>
        public string ENP { get; set; }
        /// <summary>
        /// ОКАТО ТФОМС
        /// </summary>
        [ForeignKey("TFOMS")]
        public string TF_OKATO { get; set; }

        public virtual TFOMS_P TFOMS { get; set; }
        /// <summary>
        /// Тип СМО
        /// </summary>
        public TypeSMO TYPE_SMO { get; set; }
        public string TypeSMO_RUS
        {
            get
            {
                return TYPE_SMO switch
                {
                    TypeSMO.OGRN => "ОГРН",
                    TypeSMO.RNumber => "Реестровый номер",
                    TypeSMO.TFOMS => "ТФОМС",
                    _ => $"{TYPE_SMO} - не найдено значение"
                };
            }
        }

        [NotMapped]
        public string SMO_NAME { get; set; }
        /// <summary>
        /// Код СМО
        /// </summary>
        public string SMO { get; set; }
        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime? DATE_B { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime? DATE_E { get; set; }
        /// <summary>
        /// Тип полиса
        /// </summary>
        public VPOLIS_VALUES? VPOLIS { get; set; }
        /// <summary>
        /// Серия полиса
        /// </summary>
        public string SPOLIS { get; set; }
        /// <summary>
        /// Номер полиса
        /// </summary>
        public string NPOLIS { get; set; }
        /// <summary>
        /// Окато СМО
        /// </summary>
        public string SMO_OK { get; set; }

    }




}
