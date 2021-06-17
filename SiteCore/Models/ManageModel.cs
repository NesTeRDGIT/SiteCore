using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiteCore.Models
{
    public class IndexViewModel
    {
        [StringLength(40, ErrorMessage = "Значение {0} должно содержать символов не более: {1}.")]
        public string FAM { get; set; }
        [StringLength(40, ErrorMessage = "Значение {0} должно содержать символов не более: {1}.")]
        public string IM { get; set; }
        [StringLength(40, ErrorMessage = "Значение {0} должно содержать символов не более: {1}.")]
        public string OT { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Текущий пароль обязателен к заполнению")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Новый пароль обязателен к заполнению")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать символов не менее: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают.")]
        public string ConfirmNewPassword { get; set; }
    }
}
