using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FindRab.models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [Column("password")]
        public string Password { get; set; }

        // Изменяем свойство с new_role на role
        [Column("role")]
        public int Role { get; set; }
    }
}
