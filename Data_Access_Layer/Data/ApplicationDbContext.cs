using Data_Access_Layer.Models;
using Data_Access_Layer.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<OpeningDTO> Openings { get; set; }
        public DbSet<ReferralDTO> Referrals { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpeningDTO>().HasData(
            new OpeningDTO
            {
                Id = 27,
                RoleName = "Software Engineer",
                RoleLocation = "New York",
                MinExp = "3 years",
                Availability = 10,
                Description = "We are looking for a highly skilled Software Engineer to join our team. You will be responsible for designing, coding, and testing software applications. You should have a strong background in software development and be proficient in various programming languages."
            },
            new OpeningDTO
            {
                Id = 28,
                RoleName = "Data Scientist",
                RoleLocation = "San Francisco",
                MinExp = "5 years",
                Availability = 7,
                Description = "As a Data Scientist, you will play a key role in analyzing large datasets to provide valuable insights. You should have a deep understanding of data analysis, statistical modeling, and machine learning. Join our team to work on exciting data projects."
            },
            new OpeningDTO
            {
                Id = 29,
                RoleName = "Web Designer",
                RoleLocation = "Los Angeles",
                MinExp = "2 years",
                Availability = 15,
                Description = "We are seeking a creative Web Designer to work on designing and developing user-friendly websites. You will be responsible for creating visually appealing and user-friendly web interfaces. Join us to bring innovative design ideas to life."
            }
        );
        }
    }
}
