using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SiteCore.Data;

namespace SiteCore.Models
{
    public class NewsEditModelView:IValidatableObject
    {
        public int? ID_NEW { get; set; }
        public DateTime DT { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Заголовок обязателен для заполнения")]
        public string HEADER { get; set; }
        [Required(ErrorMessage = "Текст обязателен для заполнения")]
        public string TEXT { get; set; }
        public string[] NEWS_ROLE { get; set; }
        public ApplicationRole[] Roles { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NEWS_ROLE == null || NEWS_ROLE.Length == 0)
                yield return new ValidationResult("Поле \"Роли\" обязательно к заполнению");
        }
    }
}