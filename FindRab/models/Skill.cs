using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("skills")]
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }

        [Required]
        [StringLength(50)]
        public string SkillName { get; set; }
    }
}
