using EmpRefPortal_POC.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpRefPortal_POC.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Referrals> Referrals { get; set; }
    }
}
