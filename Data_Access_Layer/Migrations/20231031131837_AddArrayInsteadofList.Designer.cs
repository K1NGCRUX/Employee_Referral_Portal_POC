﻿// <auto-generated />
using Data_Access_Layer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231031131837_AddArrayInsteadofList")]
    partial class AddArrayInsteadofList
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Data_Access_Layer.Models.DTO.OpeningDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Availability")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MinExp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Openings");

                    b.HasData(
                        new
                        {
                            Id = 27,
                            Availability = 10,
                            Description = "We are looking for a highly skilled Software Engineer to join our team. You will be responsible for designing, coding, and testing software applications. You should have a strong background in software development and be proficient in various programming languages.",
                            MinExp = "3 years",
                            RoleLocation = "New York",
                            RoleName = "Software Engineer"
                        },
                        new
                        {
                            Id = 28,
                            Availability = 7,
                            Description = "As a Data Scientist, you will play a key role in analyzing large datasets to provide valuable insights. You should have a deep understanding of data analysis, statistical modeling, and machine learning. Join our team to work on exciting data projects.",
                            MinExp = "5 years",
                            RoleLocation = "San Francisco",
                            RoleName = "Data Scientist"
                        },
                        new
                        {
                            Id = 29,
                            Availability = 15,
                            Description = "We are seeking a creative Web Designer to work on designing and developing user-friendly websites. You will be responsible for creating visually appealing and user-friendly web interfaces. Join us to bring innovative design ideas to life.",
                            MinExp = "2 years",
                            RoleLocation = "Los Angeles",
                            RoleName = "Web Designer"
                        });
                });

            modelBuilder.Entity("Data_Access_Layer.Models.DTO.ReferralDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CandidateContact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CandidateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefEmployee")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Referrals");
                });

            modelBuilder.Entity("Data_Access_Layer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppliedRoles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
