using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindRab.models
{
    [Table("vacancies")]
    public class Vacancy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("vacancy_id")]
        public int VacancyId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Title")]
        public string Title { get; set; }

        [Required]
        [Column("Description")]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Education")]
        public string Education { get; set; }

        [Required]
        [Column("Salary")]
        public decimal Salary { get; set; }

        [Required]
        [Column("Phone")]
        public string Phone { get; set; }

        [Required]
        [Column("Email")]
        public string Email { get; set; }

        [Column("ApplicationCount")]
        public int ApplicationCount { get; set; }

        [ForeignKey("users")]
        [Column("user_id")]
        public int UserId { get; set; }
    }
}
