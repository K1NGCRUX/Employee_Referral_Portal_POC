using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class Opening
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public string RoleLocation { get; set; }

        [Required]
        public string MinExp { get; set; }

        [Required]
        public int Availability { get; set; }

        [Required]
        public string Description { get; set; }

        public string Applied { get; set; }
    }
}
