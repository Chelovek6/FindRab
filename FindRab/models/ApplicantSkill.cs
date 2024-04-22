using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("applicant_skills")]
    public class ApplicantSkill
    {

        [ForeignKey("Applicant")]
        [Key] 
        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; }

        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        [Required]
        [StringLength(20)]
        public string SkillLevel { get; set; }
    }
}
