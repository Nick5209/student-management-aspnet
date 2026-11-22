using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDWebAppInASPCore.Models
{
    public class Student
    {
        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Enter a student name.")]
        [StringLength(100)]
        [Column("StudentName", TypeName ="Varchar(100)")]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(15)]
        [Column("StudentGender", TypeName = "Varchar(15)")]
        public string Gender { get; set; } = "";
        
        [Required]
        public int? Age { get; set; }

        [Required]
        [Display(Name = "Standard")]
        public int? Standerd { get; set; }
    }
}
