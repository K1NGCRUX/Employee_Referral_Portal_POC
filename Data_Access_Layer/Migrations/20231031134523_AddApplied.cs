using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddApplied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.AddColumn<string>(
                name: "Applied",
                table: "Openings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applied",
                table: "Openings");

            migrationBuilder.InsertData(
                table: "Openings",
                columns: new[] { "Id", "Availability", "Description", "MinExp", "RoleLocation", "RoleName" },
                values: new object[,]
                {
                    { 27, 10, "We are looking for a highly skilled Software Engineer to join our team. You will be responsible for designing, coding, and testing software applications. You should have a strong background in software development and be proficient in various programming languages.", "3 years", "New York", "Software Engineer" },
                    { 28, 7, "As a Data Scientist, you will play a key role in analyzing large datasets to provide valuable insights. You should have a deep understanding of data analysis, statistical modeling, and machine learning. Join our team to work on exciting data projects.", "5 years", "San Francisco", "Data Scientist" },
                    { 29, 15, "We are seeking a creative Web Designer to work on designing and developing user-friendly websites. You will be responsible for creating visually appealing and user-friendly web interfaces. Join us to bring innovative design ideas to life.", "2 years", "Los Angeles", "Web Designer" }
                });
        }
    }
}
