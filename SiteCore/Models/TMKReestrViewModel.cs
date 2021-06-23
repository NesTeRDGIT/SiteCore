using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Models
{

    public class IndexModel
    {
        ///    public List<TMKReestr> Items { get; set; }
        public List<CODE_MO> CODE_MO { get; set; }
        public List<SMO> CODE_SMO { get; set; }
        public List<NMIC_VID_NHISTORY> NMIC_VID_NHISTORY { get; set; }
        public List<NMIC_OPLATA> NMIC_OPLATA { get; set; }
        //public TMKFillter Fillter { get; set; }
    }



    public class TMKItem:IValidatableObject
    {
        public int TMK_ID { get; set; }
        [Required(ErrorMessage = "Поле \"НМИЦ\" обязательно к заполнению")]
        public int? NMIC { get; set; }
        [Required(ErrorMessage = "Поле \"Телемедицинская система\" обязательно к заполнению")]
        public int? TMIS { get; set; }
        [Required(ErrorMessage = "Поле \"Профиль\" обязательно к заполнению")]
        public int? PROFIL { get; set; }
        public DateTime? DATE_TMK { get; set; }
        [Required(ErrorMessage = "Поле \"Дата протокола ТМК\" обязательно к заполнению")]
        public DateTime? DATE_PROTOKOL { get; set; }
        [Required(ErrorMessage = "Поле \"Дата запроса\" обязательно к заполнению")]
        public DateTime? DATE_QUERY { get; set; }
        [Required(ErrorMessage = "Поле \"Дата лечения\" обязательно к заполнению")]
        public DateTime? DATE_B { get; set; }

        [Required(ErrorMessage = "Поле \"№ истории\" обязательно к заполнению")]
        [MaxLength(16)]
        public string NHISTORY { get; set; }

        [Required(ErrorMessage = "Поле \"Вид медицинской документации\" обязательно к заполнению")]
        public int? VID_NHISTORY { get; set; }
        public string VID_NHISTORY_NAM { get; set; }
        public DateTime DATE_INVITE { get; set; }
        public bool ISNOTSMO { get; set; }
        [MaxLength(16, ErrorMessage = "ЕНП не может быть больше 16 символов")]
        [RequiredIf(nameof(ISNOTSMO), new object[] { false }, ErrorMessage = "Поле \"ЕНП\" обязательно к заполнению")]
        public string ENP { get; set; }
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
        public DateTime? DR { get; set; }
        public bool NOVOR { get; set; }
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
        public string NAM_MOK { get; set; }
        public StatusTMKRow STATUS { get; set; }
        public string STATUS_COM { get; set; }
        public string SMO { get; set; }
        public string SMO_NAM { get; set; }
        public int OPLATA { get; set; }
        public string OPLATA_NAM { get; set; }
        public string SMO_COM { get; set; }
        public string PROFIL_NAM { get; set; }
        public string NMIC_NAM { get; set; }
        public string TMS_NAM { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
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
    public class ExpertizeModel
    {
        public void CopyTo(TMKReestRExpertize exp)
        {
            exp.CELL = this.CELL;
            exp.DATEACT = this.DATEACT.Value;
            exp.FIO = this.FIO;
            exp.FULL = this.FULL;
            exp.ISCOROLLARY = this.ISCOROLLARY;
            exp.ISNOTRECOMMEND = this.ISNOTRECOMMEND;
            exp.ISOSN = this.ISOSN;
            exp.ISRECOMMENDMEDDOC = this.ISRECOMMENDMEDDOC;
            exp.NOTPERFORM = this.NOTPERFORM;
            exp.N_EXP = this.N_EXP;
            exp.DATEACT = this.DATEACT.Value;
            exp.S_TIP = this.S_TIP.Value;
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
        public int EXPERTIZE_ID { get; set; }
        /// <summary>
        /// Тип экспертизы 2 МЭЭ, 3 ЭКМП
        /// </summary>
        [Required(ErrorMessage = "Поле \"Тип экспертизы\" обязательно к заполнению")]
        public ExpertTip? S_TIP { get; set; }
        /// <summary>
        /// Дата акта
        /// </summary>
        [Required(ErrorMessage = "Поле \"Дата акта\" обязательно к заполнению")]
        public DateTime? DATEACT { get; set; }
        /// <summary>
        /// Номер акта
        /// </summary>
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
        public IEnumerable<string> Validate(DateTime DATE_PROTOKOL)
        {
            if (!(DATEACT >= DATE_PROTOKOL && DATEACT <= DateTime.Now) && S_TIP != ExpertTip.MEK)
                yield return "Дата акта не может быть меньше даты протокола или больше текущей даты";

            if (OSN.GroupBy(x => x.S_OSN).Count(x => x.Count() > 1) != 0)
                yield return "Присутствуют не уникальные дефекты";
        }
        public  List<EXPERTIZE_OSN> OSN { get; set; } = new();
    }

    public class OPLATAandVID_NHISTORYModel
    {
        public int TMK_ID { get; set; }
        public int VID_NHISTORY { get; set; }
        public int OPLATA { get; set; }
        public string SMO_COM { get; set; }
    }


    public class TMKFillter
    {
        public string ENP { get; set; }
        public string FAM { get; set; }
        public string IM { get; set; }
        public string OT { get; set; }
        public DateTime? DR { get; set; }
        public DateTime? DATE_B_BEGIN { get; set; }
        public DateTime? DATE_B_END { get; set; }
        public DateTime? DATE_PROTOKOL_BEGIN { get; set; }
        public DateTime? DATE_PROTOKOL_END { get; set; }
        public DateTime? DATE_QUERY_BEGIN { get; set; }
        public DateTime? DATE_QUERY_END { get; set; }
        public DateTime? DATE_TMK_BEGIN { get; set; }
        public DateTime? DATE_TMK_END { get; set; }

        public string[] CODE_MO { get; set; }
        public string[] SMO { get; set; }
        public int[] VID_NHISTORY { get; set; }
        public int[] OPLATA { get; set; }

        public bool HasValue => !string.IsNullOrEmpty(ENP) || !string.IsNullOrEmpty(FAM) || !string.IsNullOrEmpty(IM) ||
                                !string.IsNullOrEmpty(OT) ||
                                DR.HasValue || DATE_B_BEGIN.HasValue || DATE_B_END.HasValue ||
                                DATE_PROTOKOL_BEGIN.HasValue || DATE_PROTOKOL_END.HasValue ||
                                DATE_QUERY_BEGIN.HasValue || DATE_QUERY_END.HasValue || DATE_TMK_BEGIN.HasValue ||
                                DATE_TMK_END.HasValue ||
                                CODE_MO?.Length > 0 || SMO?.Length > 0 || VID_NHISTORY?.Length > 0 ||
                                OPLATA?.Length > 0;

    }


    public class EditTMKReestrModel
    {
        public List<V002> V002 { get; set; }
        public List<TMIS> TMIS { get; set; }
        public List<NMIC> NMIC { get; set; }
        public List<NMIC_VID_NHISTORY> NMIC_VID_NHISTORY { get; set; }

    }

    public class AddMEE_EKMPModel
    {
        public List<NMIC_CELL> NMIC_CELL { get; set; }
        public List<NMIC_FULL> NMIC_FULL { get; set; }
        public List<EXPERTS> EXPERTS { get; set; }
        public List<F014> F014 { get; set; }
    }

    public class ViewModelTMK
    {
        public List<NMIC_OPLATA> OPLATA { get; set; }
        public List<NMIC_VID_NHISTORY> VID_NHISTORY { get; set; }
    }

    public class ReportTMKRow
    {
        public static List<ReportTMKRow> Get(IEnumerable<DataRow> row)
        {
            return row.Select(Get).ToList();
        }

        public static ReportTMKRow Get(DataRow row)
        {
            try
            {
                var item = new ReportTMKRow
                {
                    SUB = row["SUB"].ToString(),
                    SMO = row["SMO"].ToString(),
                    nam_smok = row["nam_smok"].ToString(),
                    NMIC = row["NMIC"].ToString(),
                    MO = row["MO"].ToString(),
                    nam_mok = row["nam_mok"].ToString(),
                    C = Convert.ToInt32(row["C"]),
                    C_V = Convert.ToInt32(row["C_V"]),
                    C_P = Convert.ToInt32(row["C_P"]),

                    C_MEK_TFOMS = Convert.ToInt32(row["C_MEK_TFOMS"]),
                    C_MEE_TFOMS = Convert.ToInt32(row["C_MEE_TFOMS"]),
                    C_EKMP_TFOMS = Convert.ToInt32(row["C_EKMP_TFOMS"]),

                    C_MEK_SMO = Convert.ToInt32(row["C_MEK_SMO"]),
                    C_MEE_SMO = Convert.ToInt32(row["C_MEE_SMO"]),
                    C_EKMP_SMO = Convert.ToInt32(row["C_EKMP_SMO"]),

                    C_MEK_D_TFOMS = Convert.ToInt32(row["C_MEK_D_TFOMS"]),
                    C_MEE_D_TFOMS = Convert.ToInt32(row["C_MEE_D_TFOMS"]),
                    C_EKMP_D_TFOMS = Convert.ToInt32(row["C_EKMP_D_TFOMS"]),

                    C_MEK_D_SMO = Convert.ToInt32(row["C_MEK_D_SMO"]),
                    C_MEE_D_SMO = Convert.ToInt32(row["C_MEE_D_SMO"]),
                    C_EKMP_D_SMO = Convert.ToInt32(row["C_EKMP_D_SMO"]),

                    S_MEK_D_TFOMS = Convert.ToInt32(row["S_MEK_D_TFOMS"]),
                    S_MEE_D_TFOMS = Convert.ToInt32(row["S_MEE_D_TFOMS"]),
                    S_EKMP_D_TFOMS = Convert.ToInt32(row["S_EKMP_D_TFOMS"]),

                    S_MEK_D_SMO = Convert.ToInt32(row["S_MEK_D_SMO"]),
                    S_MEE_D_SMO = Convert.ToInt32(row["S_MEE_D_SMO"]),
                    S_EKMP_D_SMO = Convert.ToInt32(row["S_EKMP_D_SMO"]),

                    S_SUM_TFOMS = Convert.ToInt32(row["S_SUM_TFOMS"]),
                    S_FINE_TFOMS = Convert.ToInt32(row["S_FINE_TFOMS"]),
                    S_SUM_SMO = Convert.ToDecimal(row["S_SUM_SMO"]),
                    S_FINE_SMO = Convert.ToDecimal(row["S_FINE_SMO"]),
                    S_ALL = Convert.ToInt32(row["S_ALL"]),
                    S_1_1_3 = Convert.ToInt32(row["S_1_1_3"]),
                    S_1_2_2 = Convert.ToInt32(row["S_1_2_2"]),
                    S_1_3_2 = Convert.ToInt32(row["S_1_3_2"]),
                    S_1_4 = Convert.ToInt32(row["S_1_4"]),
                    S_3_1 = Convert.ToInt32(row["S_3_1"]),
                    S_3_2_2 = Convert.ToInt32(row["S_3_2_2"]),
                    S_3_2_3 = Convert.ToInt32(row["S_3_2_3"]),
                    S_3_2_4 = Convert.ToInt32(row["S_3_2_4"]),
                    S_3_2_5 = Convert.ToInt32(row["S_3_2_5"]),
                    S_3_2_6 = Convert.ToInt32(row["S_3_2_6"]),
                    S_3_3_1 = Convert.ToInt32(row["S_3_3_1"]),
                    S_3_4 = Convert.ToInt32(row["S_3_4"]),
                    S_3_5 = Convert.ToInt32(row["S_3_5"]),
                    S_3_6 = Convert.ToInt32(row["S_3_6"]),
                    S_3_7 = Convert.ToInt32(row["S_3_7"]),
                    S_3_8 = Convert.ToInt32(row["S_3_8"]),
                    S_3_10 = Convert.ToInt32(row["S_3_10"]),
                    S_4_2 = Convert.ToInt32(row["S_4_2"]),
                    S_5_1_3 = Convert.ToInt32(row["S_5_1_3"]),
                    S_5_3_1 = Convert.ToInt32(row["S_5_3_1"]),
                    S_5_4 = Convert.ToInt32(row["S_5_4"]),
                    S_5_5 = Convert.ToInt32(row["S_5_5"]),
                    S_5_6 = Convert.ToInt32(row["S_5_6"]),
                    S_5_7 = Convert.ToInt32(row["S_5_7"]),
                    S_5_8 = Convert.ToInt32(row["S_5_8"])
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
        public string NMIC { get; set; }
        public string nam_smok { get; set; }
        public string MO { get; set; }
        public string nam_mok { get; set; }
        public int C { get; set; }
        public int C_V { get; set; }
        public int C_P { get; set; }
        public int C_MEK_TFOMS { get; set; }
        public int C_MEE_TFOMS { get; set; }
        public int C_EKMP_TFOMS { get; set; }
        public int C_MEK_SMO { get; set; }
        public int C_MEE_SMO { get; set; }
        public int C_EKMP_SMO { get; set; }
        public int C_MEK_D_TFOMS { get; set; }
        public int C_MEE_D_TFOMS { get; set; }
        public int C_EKMP_D_TFOMS { get; set; }
        public int C_MEK_D_SMO { get; set; }
        public int C_MEE_D_SMO { get; set; }
        public int C_EKMP_D_SMO { get; set; }
        public int S_MEK_D_TFOMS { get; set; }
        public int S_MEE_D_TFOMS { get; set; }
        public int S_EKMP_D_TFOMS { get; set; }
        public int S_MEK_D_SMO { get; set; }
        public int S_MEE_D_SMO { get; set; }
        public int S_EKMP_D_SMO { get; set; }
        public decimal S_SUM_TFOMS { get; set; }
        public decimal S_FINE_TFOMS { get; set; }
        public decimal S_SUM_SMO { get; set; }
        public decimal S_FINE_SMO { get; set; }

        public int S_ALL { get; set; }
        public int S_1_1_3 { get; set; }
        public int S_1_2_2 { get; set; }
        public int S_1_3_2 { get; set; }
        public int S_1_4 { get; set; }
        public int S_3_1 { get; set; }
        public int S_3_2_2 { get; set; }
        public int S_3_2_3 { get; set; }
        public int S_3_2_4 { get; set; }
        public int S_3_2_5 { get; set; }
        public int S_3_2_6 { get; set; }
        public int S_3_3_1 { get; set; }
        public int S_3_4 { get; set; }
        public int S_3_5 { get; set; }
        public int S_3_6 { get; set; }
        public int S_3_7 { get; set; }
        public int S_3_8 { get; set; }
        public int S_3_10 { get; set; }
        public int S_4_2 { get; set; }
        public int S_5_1_3 { get; set; }
        public int S_5_3_1 { get; set; }
        public int S_5_4 { get; set; }
        public int S_5_5 { get; set; }
        public int S_5_6 { get; set; }
        public int S_5_7 { get; set; }
        public int S_5_8 { get; set; }
    }


    public class CONTACT_INFOModel
    {
        public CONTACT_INFOModel()
        {

        }
        public CONTACT_INFOModel(CONTACT_INFO item)
        {
            ID_CONTACT_INFO = item.ID_CONTACT_INFO;
            FAM = item.FAM;
            IM = item.IM;
            OT = item.OT;
            TEL = item.TEL;
            CODE_MO = item.CODE_MO;
        }
        public void To(CONTACT_INFO item)
        {
            item.ID_CONTACT_INFO = ID_CONTACT_INFO;
            item.FAM = FAM;
            item.IM = IM;
            item.OT = OT;
            item.TEL = TEL;
            item.CODE_MO = CODE_MO;
        }
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
    }

    public class EditCONTACT_INFOModel
    {
        public CONTACT_INFOModel INFO { get; set; }
        public List<CODE_MO> CODE_MO { get; set; }
    }

    public class TMKReportModel
    {
        public List<NMIC_VID_NHISTORY> NMIC_VID_NHISTORY { get; set; }
    }

    public class TMKListModel
    {
        
        public string ENP { get; set; }
        public string NAM_MOK { get; set; }
        public string CODE_MO { get; set; }
        public string FIO { get; set; }
        public DateTime? DATE_B { get; set; }
        public DateTime? DATE_QUERY { get; set; }
        public DateTime? DATE_PROTOKOL { get; set; }
        public DateTime? DATE_TMK { get; set; }
        public string SMO { get; set; }
        public string VID_NHISTORY { get; set; }
        public string OPLATA { get; set; }
        public string CONTACT_INFO { get; set; }
        public string DATE_MEK { get; set; }
        public string DEF_MEK { get; set; }
        public string DATE_MEE { get; set; }
        public string DEF_MEE { get; set; }
        public string DATE_EKMP { get; set; }
        public string DEF_EKMP { get; set; }
        public string STATUS { get; set; }
        public string STATUS_COM { get; set; }
        public bool isEXP { get; set; }
        public int TMK_ID { get; set; }
        public string TMIS_NAME { get; set; }
        public string NMIC_NAME { get; set; }
    }

}
