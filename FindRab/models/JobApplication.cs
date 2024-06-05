using FindRab.models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindRab.Models
{
    [Table("job_applications")]
    public class JobApplication
    {
        [Key]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Required]
        [ForeignKey("VacancyId")]
        public int VacancyId { get; set; }
       
        

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }
    }
}