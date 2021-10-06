using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Http;
using SiteCore.Class;

namespace SiteCore.Models
{
    public class AddRoleModel
    {
        [Required(ErrorMessage = "Отсутствует наименование роли")]
        public string CAPTION { get; set; }
        [Required(ErrorMessage = "Отсутствует префикс роли")]
        public string PREFIX { get; set; }
    }

    public class SING_LIST
    {
        public string MO_NAME;
        public string CODE_MO { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
        public string ROLE { get; set; }
        public int ID { get; set; }
        public string PublicKey { get; set; }
        public string PublicKey_ISSUER { get; set; }
    }

    public class AddCertModel
    {
        [Required(ErrorMessage = "Не указан файл для загрузки")]
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "Не указан файл подтверждающий подпись")]
        public IFormFile FileConfirm { get; set; }
        [Required(ErrorMessage = "Не указана роль")]
        public int ROLE_ID { get; set; }
        [Required(ErrorMessage = "Не указана медицинская организация")]
        public string CODE_MO { get; set; }
        [Required(ErrorMessage = "Не указана дата начала")]
        public DateTime DATE_B { get; set; }
        [Display(Name = "Дата окончания")]
        [CompareEx(nameof(DATE_B), "Дата начала", CompareExAttributeType.MoreOrEqual, true)]
        public DateTime? DATE_E { get; set; }
    }


  

    public class ISSUER_LIST
    {

        public int SING_ISSUER_ID;
        public string CAPTION { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
      

    }

    public class AddISSUERModel
    {
        public IFormFile File { get; set; }
        public string CAPTION { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime? DATE_E { get; set; }
    }

    public class DOCViewModel
    {
        public string MO_NAME { get; set; }
        public string FILENAME { get; set; }
        public List<DOCSignViewModel> SIGNS { get; set; } = new();
        public DateTime DateCreate { get; set; }
        public int DOC_FOR_SIGN_ID { get; set; }
    }

    public class DOCSignViewModel
    {
        public int ROLE_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public bool IsSIGN { get; set; }
       
    }

    public class AddFilesModel
    { 
        [Required]
        public int THEME_ID { get; set; }
        [Required]
        public IFormFile FILE { get; set; }
        [Required]
        public string CODE_MO { get; set; }
        [Required]
        public List<int> ROLE_ID { get; set; } = new();
    }


    public class AddSignModel
    {
        public string SIGN { get; set; }
        public int DOC_FOR_SIGN_ID { get; set; }
    }

    public class AddSigFileModel
    {
        public IFormFile File { get; set; }
        public int DOC_FOR_SIGN_ID { get; set; }
    }
}

