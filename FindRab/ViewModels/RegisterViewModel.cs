using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(50, ErrorMessage = "Имя пользователя не должно превышать 50 символов")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен быть не менее {2} символов.", MinimumLength = 3)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле Код роли обязательно для заполнения")]
        [Display(Name = "Код роли")]
        public int RoleCode { get; set; }

        [Required(ErrorMessage = "Поле Код аккаунта обязательно для заполнения")]
        [Display(Name = "Код аккаунта")]
        public int AccountCode { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
