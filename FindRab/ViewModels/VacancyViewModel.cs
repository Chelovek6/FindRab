using System.ComponentModel.DataAnnotations;

namespace FindRab.ViewModels
{
    public class VacancyViewModel
    {
        [Required]
        public int VacancyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Education { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int ApplicationCount { get; set; }

    }
}


