using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models.DTO
{
    public class ReferralDTO
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ForRole { get; set; }

        [Required]
        public string CandidateName { get; set; }

        [Required]
        public string CandidateContact { get; set; }

        [Required]
        public string RefEmployee { get; set; }
    }
}
