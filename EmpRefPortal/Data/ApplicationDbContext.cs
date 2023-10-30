using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmpRefPortal.Models;
using Data_Access_Layer.Models.DTO;

namespace EmpRefPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EmpRefPortal.Models.OpeningDTO>? OpeningDTO { get; set; }
        public DbSet<EmpRefPortal.Models.ReferralDTO>? ReferralDTO { get; set; }
        public DbSet<Data_Access_Layer.Models.DTO.ReferralDTO>? ReferralDTO_1 { get; set; }
        
    }
}