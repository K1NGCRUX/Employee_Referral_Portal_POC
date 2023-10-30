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
    [Migration("20231012103459_UpdateTables1")]
    partial class UpdateTables1
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
                            Id = 1,
                            Availability = 10,
                            MinExp = "3 years",
                            RoleLocation = "New York",
                            RoleName = "Software Engineer"
                        },
                        new
                        {
                            Id = 2,
                            Availability = 7,
                            MinExp = "5 years",
                            RoleLocation = "San Francisco",
                            RoleName = "Data Scientist"
                        },
                        new
                        {
                            Id = 3,
                            Availability = 15,
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

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Referrals");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CandidateContact = "johndoe@example.com",
                            CandidateName = "John Doe",
                            ForRole = "Software Engineer",
                            RefEmployee = "Alice Smith",
                            Status = "Pending"
                        },
                        new
                        {
                            Id = 2,
                            CandidateContact = "janesmith@example.com",
                            CandidateName = "Jane Smith",
                            ForRole = "Data Scientist",
                            RefEmployee = "Bob Johnson",
                            Status = "Accepted"
                        },
                        new
                        {
                            Id = 3,
                            CandidateContact = "tombrown@example.com",
                            CandidateName = "Tom Brown",
                            ForRole = "Web Designer",
                            RefEmployee = "Mary Davis",
                            Status = "Rejected"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
