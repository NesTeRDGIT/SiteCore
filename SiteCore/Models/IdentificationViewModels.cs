using SiteCore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SiteCore.Class;

namespace SiteCore.Models
{

    public class NewCSItemModel
    {
        public NewCSItemModel()
        {

        }
        public  NewCSItemModel(CS_LIST_IN item)
        {
            CS_LIST_IN_ID = item.CS_LIST_IN_ID==0? null : item.CS_LIST_IN_ID;
            CS_LIST_ID = item.CS_LIST_ID;
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
            return  new()
            {
                CS_LIST_ID = CS_LIST_ID,
                CS_LIST_IN_ID = CS_LIST_IN_ID ?? 0,
                FAM = FAM,
                IM = IM,
                OT = OT,
                DR = DR,
                W = W,
                DOC_TYPE = DOC_TYPE,
                DOC_SER = DOC_SER,
                DOC_NUM = DOC_NUM,
                VPOLIS = VPOLIS,
                SPOLIS = SPOLIS,
                NPOLIS = NPOLIS,
                SNILS = SNILS
            };

        }
        public int? CS_LIST_IN_ID { get; set; }
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
        [Range(minimum: 1, maximum: 2, ErrorMessage = "Поле \"Пол\" должно иметь значение 1 или 2")]
        public int? W { get; set; }
        /// <summary>
        /// Тип док-та
        /// </summary>
        public string DOC_TYPE { get; set; }
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
      

    }
    public class NewCSItemViewModel
    {
        public int CS_LIST_ID { get; set; }
        public NewCSItemModel ITEM { get; set; }
        public List<F011> F011 { get; set; } = new();
    }

    public class NewCSListViewModel
    {
        public int CS_LIST_ID { get; set; }
        public string Caption { get; set; }
    }
}
