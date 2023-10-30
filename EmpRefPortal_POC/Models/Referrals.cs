using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Referrals
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RefId { get; set; }

    [Required]
    public string RefName { get; set; }

    [Required]
    public int RefAge { get; set; }

    [Required]
    [RegularExpression("^[6-9]\\d{9}$")]
    public string RefContact { get; set; }

    [Required]
    public string RefEmpName { get; set; } // Match data type with Employee's EmpName

}
