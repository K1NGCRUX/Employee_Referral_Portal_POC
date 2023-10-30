using System.ComponentModel.DataAnnotations.Schema;

namespace EmpRefPortal.Models
{
    public class ReferralDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ForRole { get; set; }
        public string CandidateName { get; set; }
        public string CandidateContact { get; set; }
        public string RefEmployee { get; set; }
        public string Status { get; set; }
    }
}
