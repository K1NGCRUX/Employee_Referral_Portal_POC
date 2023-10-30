using System.ComponentModel.DataAnnotations.Schema;

namespace EmpRefPortal.Models
{
    public class OpeningDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleLocation { get; set; }
        public string MinExp { get; set; }
        public int Availability { get; set; }
        public string Description { get; set; }
    }
}
