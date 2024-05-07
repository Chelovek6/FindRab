using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    // Модель для авторизации
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
