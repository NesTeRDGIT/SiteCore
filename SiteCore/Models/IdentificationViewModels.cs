using SiteCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SiteCore.Class;

namespace SiteCore.Models
{

    public class TitleResult
    {
        public int TotalRecord { get; set; }
        public List<PersonItem> PersonItems { get; set; }
    }


    public class PersonItem
    {
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public string POLIS { get; set; }
        public string DOC { get; set; }
        public bool? STATUS { get; set; }
        public string STATUS_TEXT { get; set; }
        public string CURRENT_SMO { get; set; }
        public int CS_LIST_IN_ID { get; set; }
        public string CODE_MO { get; set; }
        public StatusCS_LIST STATUS_SEND { get; set; }
        public  string STATUS_SEND_TEXT { get; set; }
        public DateTime DATE_CREATE { get; set; }

    }


    public class SprWItemModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
    }

    public class SprF011ItemModel
    {
        public string ID { get; set; }
        public string NAME { get; set; }
    }

    public class SprVPOLISItemModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
    }

    public class SPRModel
    {
        public List<SprF011ItemModel> F011 { get; set; }
        public List<SprWItemModel> W { get; set; }
        public List<SprVPOLISItemModel> VPOLIS { get; set; }
    }


    public class PersonItemModel: IValidatableObject
    {
        public PersonItemModel()
        {

        }
        public PersonItemModel(CS_LIST_IN item)
        {
            CS_LIST_IN_ID = item.CS_LIST_IN_ID == 0 ? null : item.CS_LIST_IN_ID;
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
        public CS_LIST_IN ToCS_LIST_IN()
        {
            return new()
            {
                CS_LIST_IN_ID = CS_LIST_IN_ID ?? 0,
                FAM = FAM,
                IM = IM,
                OT = OT,
                DR = DR.Value,
                W = W.Value,
                DOC_TYPE = DOC_TYPE,
                DOC_SER = DOC_SER,
                DOC_NUM = DOC_NUM,
                VPOLIS = VPOLIS,
                SPOLIS = SPOLIS,
                NPOLIS = NPOLIS,
                SNILS = SNILS
            };
        }

        public void CopyTo(CS_LIST_IN item)
        {
            item.FAM = FAM;
            item.IM = IM;
            item.OT = OT;
            item.DR = DR.Value;
            item.W = W.Value;
            item.DOC_TYPE = DOC_TYPE;
            item.DOC_SER = DOC_SER;
            item.DOC_NUM = DOC_NUM;
            item.VPOLIS = VPOLIS;
            item.SPOLIS = SPOLIS;
            item.NPOLIS = NPOLIS;
            item.SNILS = SNILS;
        }



        public int? CS_LIST_IN_ID { get; set; }
        private string _FAM;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string FAM
        {
            get => _FAM?.ToUpper().Trim();
            set => _FAM = value;
        }
        private string _IM;
        /// <summary>
        /// Имя
        /// </summary>
        public string IM
        {
            get => _IM?.ToUpper().Trim();
            set => _IM = value;
        }
        private string _OT;
        /// <summary>
        /// Отчество
        /// </summary>
        public string OT
        {
            get => _OT?.ToUpper().Trim();
            set => _OT = value;
        }
        /// <summary>
        /// Дата рождения
        /// </summary>
        [Required(ErrorMessage = "Поле \"Дата рождения\" обязательно к заполнению")]
        public DateTime? DR { get; set; }
        /// <summary>
        /// Пол
        /// </summary>
        [Required(ErrorMessage = "Поле \"Пол\" обязательно к заполнению")]
        [Range(minimum: 1, maximum: 2, ErrorMessage = "Поле \"Пол\" должно иметь значение 1 или 2")]
        public int? W { get; set; }
        private string _DOC_TYPE;
        /// <summary>
        /// Тип док-та
        /// </summary>
        public string DOC_TYPE { get; set; }

        private string _DOC_SER
        {
            get => _DOC_TYPE?.ToUpper().Trim();
            set => _DOC_TYPE = value;
        }
        /// <summary>
        /// Серия док-та
        /// </summary>
        public string DOC_SER
        {
            get => _DOC_SER?.ToUpper().Trim();
            set => _DOC_SER = value;
        }

        private string _DOC_NUM;
        /// <summary>
        /// Номер док-та
        /// </summary>
        [RequiredIf("DOC_TYPE", new object[] { null, "" }, IsInverted = true, ErrorMessage = "Поле \"Номер документа\" обязателен для заполнения")]
        public string DOC_NUM
        {
            get => _DOC_NUM?.ToUpper().Trim();
            set => _DOC_NUM = value;
        }
        /// <summary>
        /// Тип полиса
        /// </summary>
        public VPOLIS_VALUES? VPOLIS { get; set; }

        private string _SPOLIS;
        /// <summary>
        /// Серия полиса
        /// </summary>
        [RequiredIf("VPOLIS", new object[] { 1 }, ErrorMessage = "Поле \"Серия полиса\" обязателен для заполнения")]
        public string SPOLIS
        {
            get => _SPOLIS?.ToUpper().Trim();
            set => _SPOLIS = value;
        }
        private string _NPOLIS;
        /// <summary>
        /// Номер полиса
        /// </summary>
        [RequiredIf("VPOLIS", new object[] { null }, IsInverted = true, ErrorMessage = "Поле \"Номер полиса\" обязателен для заполнения")]
        public string NPOLIS
        {
            get => _NPOLIS?.ToUpper().Trim();
            set => _NPOLIS = value;
        }
        private string _SNILS;
        /// <summary>
        /// СНИЛС
        /// </summary>
        public string SNILS
        {
            get => _SNILS?.ToUpper().Trim();
            set => _SNILS = value;
        }
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


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(FAM) && string.IsNullOrEmpty(IM) && string.IsNullOrEmpty(OT))
            {
                yield return new ValidationResult("Минимум один из компонентов ФИО должен быть заполнен!");
            }

            if (!VPOLIS.HasValue && string.IsNullOrEmpty(DOC_TYPE) && string.IsNullOrEmpty(SNILS))
            {
                yield return new ValidationResult("Минимум один из компонентов ПОЛИС-ДОКУМЕНТ-СНИЛС должен быть заполнен!");
            }
        }
    }




    public class PersonView
    {
       public string FIO { get; set; }
       public DateTime DR { get; set; }
        public string W { get; set; }
        public string POLIS { get; set; }
        public string DOC { get; set; }
        public string SNILS { get; set; }
        public bool? STATUS { get; set; }
        public PersonViewResult[] RESULT { get; set; }

}

    public class PersonViewResult
    {
        public string ENP { get; set; }
        public DateTime? DR { get; set; }
        public DateTime? DDEATH { get; set; }
        public string LVL_D { get; set; }
        public string[] LVL_D_KOD { get; set; }
        public PersonViewResultSMO[] SMO { get; set; }
    }


    public class PersonViewResultSMO
    {
        public string ENP { get; set; }
        public string TF_OKATO { get; set; }
        public string NAME_TFK { get; set; }
        public string TYPE_SMO { get; set; }
        public string SMO { get; set; }
        public string SMO_NAME { get; set; }
        public DateTime? DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
        public string VPOLIS { get; set; }
        public string SPOLIS { get; set; }
        public string NPOLIS { get; set; }
        public string SMO_OK { get; set; }
    }

}
