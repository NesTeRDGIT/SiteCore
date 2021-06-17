using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SiteCore.Class;

namespace SiteCore.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно к заполнению")]
        [Display(Name = "Имя пользователя")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Поле 'Пароль' обязательно к заполнению")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }


    public class RegisterNewViewModel
    {
        [Display(Name = "Код МО")]
        public string CODE_MO { get; set; }
        [Display(Name = "Код СМО")]
        public string CODE_SMO { get; set; }
        [Required(ErrorMessage = "Поле Имя пользователя обязательно к заполнению")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        [RequiredIf("UserId", new object[]{null, ""}, ErrorMessage = "Поле Пароль обязательно к заполнению")]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Роли")]
        public string[] Roles { get; set; } = new string[0];
        [Display(Name = "Сдача без подписи")]
        public bool WithSing { get; set; } = false;
        public string UserId { get; set; }
    }

    public class RegisterUser
    {
        public string UserName { get; set; }
        public string CODE_MO { get; set; }
        public string NAME_MO { get; set; }
        public bool Online { get; set; }
        public DateTime OnlineTime { get; set; }
        public string Roles { get; set; }
        public string FIO { get; set; }
        public string NUMBER { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
    }
  
    public class RegisterUsersViewModel
    {
        public List<RegisterUser> Users { get; set; } = new List<RegisterUser>();
    }

    public class MassRegisterViewModel
    {
        [Required(ErrorMessage = "Поле 'Значение МО от' обязательно к заполнению")]
        [Display(Name = "Значение МО от")]
        public int FirstValue { get; set; }
        [Required(ErrorMessage = "Поле 'Значение МО до' обязательно к заполнению")]
        [Display(Name = "Значение МО до")]
        public int LastValue { get; set; }

        [Display(Name = "Префикс имени пользователя")]
        [Required(ErrorMessage = "Префикс имени пользователя обязателен к заполнению")]
        [MinLength(2)]
        public string UserNamePrefix { get; set; }
        [Display(Name = "Роли")]
        public string[] Roles { get; set; } = Array.Empty<string>();

    }
}
