using FindRab.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindRab.Models
{
    [Table("job_applications")]
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}