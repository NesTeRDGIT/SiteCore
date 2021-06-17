using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CRZ_IDENTITIFI;


namespace IdentiModel
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

 
    public class CSOracleSet : DbContext
    {
        public CSOracleSet(string nameOrConnectionString) : base(nameOrConnectionString)
        {
         
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SITE");
            modelBuilder.Entity<CS_LIST_IN>().HasMany(ir => ir.CS_LIST_IN_RESULT).WithRequired(i => i.CS_LIST_IN);
            modelBuilder.Entity<CS_LIST_IN_RESULT>().HasRequired(x => x.CS_LIST_IN).WithMany(x => x.CS_LIST_IN_RESULT);
            modelBuilder.Entity<CS_LIST_IN_RESULT_SMO>().HasRequired(x => x.CS_LIST_IN_RESULT).WithMany(x => x.CS_LIST_IN_RESULT_SMO);
        }
        public virtual DbSet<F011> F011 { get; set; }
        public virtual DbSet<TFOMS_P> TFOMS_P { get; set; }
        public virtual DbSet<CS_LIST> CS_LIST { get; set; }
        public virtual DbSet<CS_LIST_IN> CS_LIST_IN { get; set; }
        public virtual DbSet<CS_LIST_IN_RESULT> CS_LIST_IN_RESULT { get; set; }
        public virtual DbSet<CS_LIST_IN_RESULT_SMO> CS_LIST_IN_RESULT_SMO { get; set; }

        public virtual  DbSet<SMO> F002 { get; set; }

        public  string FindNameSMO(TypeSMO TS, string SMO_COD, string SMO_OK)
        {
            if (string.IsNullOrEmpty(SMO_COD)) return "";
            switch (TS)
            {
                case TypeSMO.OGRN:
                    return F002.FirstOrDefault(x => x.OGRN == SMO_COD && x.TF_OKATO==SMO_OK)?.NAM_SMOK;
                case TypeSMO.RNumber:
                    return F002.FirstOrDefault(x => x.SMOCOD == SMO_COD)?.NAM_SMOK;
                case TypeSMO.TFOMS:
                    return TFOMS_P.FirstOrDefault(x => x.TF_OKATO == SMO_COD)?.NAME_TFK;
                default:
                    return "";
            }
        }
    }

    [Table("NSI.F011")]
    public class F011
    {
        [Key]
        public string IDDOC { get; set; }
        public string DOCNAME { get; set; }
    }

    [Table("NSI.TFOMS_P")]
    public class TFOMS_P
    {
        [Key]
        public string TF_OKATO { get; set; }
        public string NAME_TFK { get; set; }
      
    }

    [Table("NSI.F002")]
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
        [Key,ForeignKey("CS_LIST_IN_RESULT")]
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

        public DateTime DR { get; set; }
        /// <summary>
        /// Пол
        /// </summary>

        public int W { get; set; }
        /// <summary>
        /// Тип док-та
        /// </summary>
        public string DOC_TYPE { get; set; }
        [ForeignKey("DOC_TYPE")]
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

        public CS_LIST_IN_RESULT_SMO CurrentSMO => CS_LIST_IN_RESULT.SelectMany(x=>x.CS_LIST_IN_RESULT_SMO)
            .OrderByDescending(x => x.DATE_E ?? DateTime.Now).FirstOrDefault();


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
        [ForeignKey("CS_LIST_IN_ID")]
        public virtual CS_LIST_IN CS_LIST_IN { get; set; }
    
        /// <summary>
        /// Главный ЕНП
        /// </summary>
        public  string ENP { get; set; }
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
        [ForeignKey("CS_LIST_IN_RESULT")]
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
        public  string TF_OKATO { get; set; }
      
        public virtual TFOMS_P TFOMS { get; set; }
        /// <summary>
        /// Тип СМО
        /// </summary>
        public TypeSMO TYPE_SMO { get; set; }
        public string TypeSMO_RUS
        {
            get
            {
                switch (TYPE_SMO)
                {
                    case TypeSMO.OGRN: return "ОГРН";
                    case TypeSMO.RNumber: return "Реестровый номер";
                    case TypeSMO.TFOMS: return "ТФОМС";
                    default:
                        return $"{TYPE_SMO} - не найдено значение";
                }
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




    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RequiredIfAttribute : ValidationAttribute
    {
        #region Properties

        /// <summary>
        /// Gets or sets the other property name that will be used during validation.
        /// </summary>
        /// <value>
        /// The other property name.
        /// </value>
        public string OtherProperty { get; }

        /// <summary>
        /// Gets or sets the display name of the other property.
        /// </summary>
        /// <value>
        /// The display name of the other property.
        /// </value>
        public string OtherPropertyDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the other property value that will be relevant for validation.
        /// </summary>
        /// <value>
        /// The other property value.
        /// </value>
        public object[] OtherPropertyValue { get; }

        /// <summary>
        /// Gets or sets a value indicating whether other property's value should match or differ from provided other property's value (default is <c>false</c>).
        /// </summary>
        /// <value>
        ///   <c>true</c> if other property's value validation should be inverted; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// How this works
        /// - true: validated property is required when other property doesn't equal provided value
        /// - false: validated property is required when other property matches provided value
        /// </remarks>
        public bool IsInverted { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the attribute requires validation context.
        /// </summary>
        /// <returns><c>true</c> if the attribute requires validation context; otherwise, <c>false</c>.</returns>
        public override bool RequiresValidationContext => true;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredIfAttribute"/> class.
        /// </summary>
        /// <param name="otherProperty">The other property.</param>
        /// <param name="otherPropertyValue">The other property value.</param>
        public RequiredIfAttribute(string otherProperty, object[] otherPropertyValue)
            : base("'{0}' is required because '{1}' has a value {3}'{2}'.")
        {
            this.OtherProperty = otherProperty;
            this.OtherPropertyValue = otherPropertyValue;
            this.IsInverted = false;
        }

        #endregion

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                this.OtherPropertyDisplayName ?? this.OtherProperty,
                this.OtherPropertyValue,
                this.IsInverted ? "other than " : "of ");
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            System.Reflection.PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherProperty == null)
            {
                return new ValidationResult(
                    string.Format(CultureInfo.CurrentCulture, "Could not find a property named '{0}'.",
                        this.OtherProperty));
            }

            object otherValue = otherProperty.GetValue(validationContext.ObjectInstance);

            // check if this value is actually required and validate it
            if (!this.IsInverted && this.OtherPropertyValue.Count(x => Equals(otherValue, x)) != 0 ||
                this.IsInverted && !this.OtherPropertyValue.Any(x => Equals(otherValue, x)))
            {
                if (value == null)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }

                // additional check for strings so they're not empty
                string val = value as string;
                if (val != null && val.Trim().Length == 0)
                {
                    return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }

    public static class Ext
    {
        public static string ToRusStr(this VPOLIS_VALUES val)
        {
            switch (val)
            {
                case VPOLIS_VALUES.OLD: return "Полис ОМС старого образца";
                case VPOLIS_VALUES.TEMP_B: return "Временное свидетельство в форме электронного документа";
                case VPOLIS_VALUES.TEMP_E: return "Временное свидетельство в форме электронного документа";
                case VPOLIS_VALUES.bOMS: return "Бумажный полис ОМС единого образца";
                case VPOLIS_VALUES.eOMS: return "Электронный полис ОМС единого образца";
                case VPOLIS_VALUES.uOMS: return "Полис ОМС в составе универсальной электронной карты";

                default: return "";
            }
        }

        public static string ToRusStr(this VPOLIS_VALUES? val)
        {
            return val.HasValue ? ToRusStr(val.Value) : "";
        }

        public static string ToRusStr(this StatusCS_LIST val)
        {
            switch (val)
            {
                case StatusCS_LIST.New: return "Новый список";
                case StatusCS_LIST.Send: return "Список отправлен в ЦС(ожидание ФЛК)";
                case StatusCS_LIST.OnSend: return "Список в очереди на отправку в ЦС";
                case StatusCS_LIST.FLK: return "Список отправлен в ЦС(ожидание ответа)";
                case StatusCS_LIST.Answer: return "Ответ получен";
                case StatusCS_LIST.Error: return "Ошибка обработки";

                default: return "";
            }
        }

        public static string ToRusStr(this TypeSMO val)
        {
            switch (val)
            {
                case TypeSMO.OGRN: return "ОГРН";
                case TypeSMO.RNumber: return "Реестровый номер";
                case TypeSMO.TFOMS: return "ТФОМС";

                default: return "";
            }
        }

    }

}
