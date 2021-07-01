using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Models
{
    public class MSEFillter
    {
        public string ENP { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime? DR { get; set; }
        public DateTime? D_PROT_BEGIN { get; set; }
        public DateTime? D_PROT_END { get; set; }
        public DateTime? D_FORM_BEGIN { get; set; }
        public DateTime? D_FORM_END { get; set; }
        public string N_PROT { get; set; }
        public string SNILS { get; set; }
        public string CODE_MO { get; set; }
        public string[] SMO { get; set; }

        public bool HasValue => !string.IsNullOrEmpty(ENP) || !string.IsNullOrEmpty(FAM) || !string.IsNullOrEmpty(IM) ||
                                !string.IsNullOrEmpty(OT) ||
                                DR.HasValue || D_PROT_BEGIN.HasValue || D_PROT_END.HasValue ||
                                D_FORM_BEGIN.HasValue || D_FORM_END.HasValue
                                || !string.IsNullOrEmpty(N_PROT) ||
                                !string.IsNullOrEmpty(SNILS) ||
                                !string.IsNullOrEmpty(CODE_MO) || SMO?.Length > 0;
    }

    public class MSEIndexModel
    {
        public List<SMO> CODE_SMO { get; set; }
    }

    public class AddMEE_EKMP_MSEModel
    {
        public int MSE_SLUCH_ID { get; set; }
        public ExpertTip Tip { get; set; }
        public MSEExpertize Expertize { get; set; }
        public List<EXPERTS> EXPERTS { get; set; }
        public List<F014> F014 { get; set; }
    }

    public class AddSLUCH_MSEModel
    {
        public int MSE_ID { get; set; }
        public MSE_TF01_SLUCHModel SLUCH { get; set; }
        public int? MSE_SLUCH_ID { get; set; }
    }


    public class MSE_TF01_SLUCHModel
    {
        [Required(ErrorMessage = "Поле \'Дата начала случая\' обязательно к заполнению")]
        public DateTime? DATE_1 { get; set; }
        [Required(ErrorMessage = "Поле \'Дата окончания случая\' обязательно к заполнению")]
        public DateTime? DATE_2 { get; set; }
        [Required(ErrorMessage = "Поле \'№истории\' обязательно к заполнению")]
        [MaxLength(100, ErrorMessage = "Поле \'№истории\' не должно быть больше 100 символов")]
        public string  N_HISTORY { get; set; }

    }

    public class sluchListModel
    {
        public sluchListModel(IEnumerable<MSE_TF01_SLUCH> _Sluch, bool _IsTFOMS)
        {
            Sluch = _Sluch;
            IsTFOMS = _IsTFOMS;
        }
        public IEnumerable<MSE_TF01_SLUCH> Sluch { get; set; }
        public bool IsTFOMS { get; set; }
    }


    public class expertizeListModel
    {
        public IEnumerable<MSEExpertize> Expertizes { get; set; }
        public bool IsTFOMS { get; set; }
        public expertizeListModel(IEnumerable<MSEExpertize> _Expertizes, bool _IsTFOMS)
        {
            Expertizes = _Expertizes;
            IsTFOMS = _IsTFOMS;
        }
    }



    public class _SMODataInputModelMSE
    {
        public int MSE_ID { get; set; }
        public string DS { get; set; }
        public int? PROFIL { get; set; }
        public string DS_NAME { get; set; }
        public List<V002> V002 { get; set; }
        public string SMO_COM { get; set; }
    }
   
    public class MSE_ITEMModel
    {
        public string DS_NAME { get; set; }
        public string SNILS { get; set; }
        public string ENP { get; set; }
        public string NAM_MOK { get; set; }
        public string FIO { get; set; }
        public DateTime D_PROT { get; set; }
        public DateTime D_FORM { get; set; }
        public string SMO { get; set; }
        public string DATE_MEK { get; set; }
        public string DEF_MEK { get; set; }
        public string DATE_MEE { get; set; }
        public string DEF_MEE { get; set; }
        public string DATE_EKMP { get; set; }
        public string DEF_EKMP { get; set; }
        public int MSE_ID { get;  set; }
        public string N_PROT { get; set; }
        public string REASON_R { get; set; }
        public string PRNAME { get; set; }
    }

    public class GetMSEListModel
    {
        public int count { get; set; }
        public List<MSE_ITEMModel> items { get; set; }
    }


  
    public class MSE_Item
    {
        public DateTime DATE_LOAD { get; set; }
        public string ENP { get; set; }
        public string NAM_MO { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime DR { get; set; }
        public string N_PROT { get; set; }
        public DateTime D_PROT { get; set; }
        public DateTime D_FORM { get; set; }
        public string SMO_NAME { get; set; }
        public string SMO { get; set; }
        public int MSE_ID { get; set; }
        public string SNILS { get; set; }
        public string REASON_R { get; set; }

        public MSE_SMO_DATAModel SMO_DATA { get; set; } = new();

        public List<MSE_SLUCHModel> SLUCH { get; set; }

    }

    public class MSE_SMO_DATAModel
    {
        public string DS { get; set; }
        public string DS_NAME { get; set; }
        public int? PROFIL { get; set; }
        public string PROFIL_NAME { get; set; }
        public string SMO_COM { get; set; }

        public int MSE_ID { get; set; }
    }
    public class MSE_SLUCHModel
    {
        public DateTime DATE_1 { get; set; }
        public DateTime DATE_2 { get; set; }
        public int MSE_SLUCH_ID { get; set; }
        public string  N_HISTORY { get; set; }
        public List<MSE_ExpertizeModel> Expertizes { get; set; }
    }



    public class MSE_ExpertizeModel:IValidatableObject
    {
        public int MSE_SLUCH_ID { get; set; }
        public int EXPERTIZE_ID { get; set; }
        [Required(ErrorMessage = "Поле \"Тип экспертизы\" обязательно к заполнению")]
        public ExpertTip S_TIP { get; set; }
        [Required(ErrorMessage = "Поле \"Номер акта\" обязательно к заполнению")]
        public string NUMACT { get; set; }
        [Required(ErrorMessage = "Поле \"Дата акта\" обязательно к заполнению")]
        public DateTime DATEACT { get; set; }
        [RequiredIf("S_TIP", new object[] { ExpertTip.MEE }, ErrorMessage = "Поле \"ФИО специалиста или врача-эксперта\" обязательно к заполнению")]
        public string FIO { get; set; }
        [RequiredIf("S_TIP", new object[] { ExpertTip.EKMP }, ErrorMessage = "Поле \"Код врача эксперта\" обязательно к заполнению")]
        public string N_EXP { get; set; }
        public List<MSE_ExpertizeOSNModel> OSN { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OSN != null && OSN.GroupBy(x => x.S_OSN).Count(x => x.Count() > 1) != 0)
                yield return new ValidationResult("Присутствуют не уникальные дефекты");
        }
    }


    public class MSE_ExpertizeOSNModel
    {
        public int? OSN_ID { get; set; }

        [Required(ErrorMessage = "Поле \"Причина санкции\" обязательно к заполнению")]
        public int S_OSN { get; set; }
        public string S_COM { get; set; }
        [Required(ErrorMessage = "Поле \"Сумма санкции\" обязательно к заполнению")]
        public decimal S_SUM { get; set; }
        public decimal? S_FINE { get; set; }
    }

}
