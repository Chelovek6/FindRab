using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

