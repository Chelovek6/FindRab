using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("vacancies")]
    public class Vacancy
    {
        [Key]
        public int VacancyId { get; set; }

        [ForeignKey("Employer")]
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }

        
    }

}
