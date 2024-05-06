using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("work_experience")]
    public class WorkExperience
    {
        [Key]
        public int ExperienceId { get; set; }

        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

    }
}
