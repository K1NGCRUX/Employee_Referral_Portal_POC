using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpRefPortal_POC.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; } // Specify a single primary key property

        [Required]
        public string EmpName { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@perficient\.com$")]
        public string EmpEmail { get; set; }
    }
}
