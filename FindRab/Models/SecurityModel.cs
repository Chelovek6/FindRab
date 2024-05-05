using System.ComponentModel.DataAnnotations;

namespace FindRab.Models
{
    public class SecurityModel
    {
        [Key]
        public int ID_Client { get; set; }

        [Required(ErrorMessage = "Не указан Login")]
        //[RegularExpression("/^([a - zA - Z0 - 9])$/", ErrorMessage = "Некоректный ввод, необходимы: A-Z a-z 0-9")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 16 символов")]
        //[Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Login уже используется")]
        private string _login { get; set; }
        public string Log_in
        {
            get => _login;
            set
            {
               // if (BDContext.Find(value))
                    _login = value;
            }
        }

        [Required(ErrorMessage = "Не указан Пароль")]
        //[RegularExpression(@"[A-Za-z0-9._%+-]", ErrorMessage = "Некоректный ввод, необходимы: A-Z a-z 0-9 ._%+-")]
        [StringLength(16, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 16 символов")]
        public string Pass_word { get; set; }

    }
}
