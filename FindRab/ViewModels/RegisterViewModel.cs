using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Password)]
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
