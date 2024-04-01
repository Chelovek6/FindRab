using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("job_applications")]
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

        [ForeignKey("Vacancy")]
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }
    }
}
