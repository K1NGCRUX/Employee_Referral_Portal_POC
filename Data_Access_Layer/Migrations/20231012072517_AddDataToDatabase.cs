using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Openings",
                columns: new[] { "Id", "CandidatesRegistered", "MinExp", "RoleLocation", "RoleName" },
                values: new object[,]
                {
                    { 1, 10, "3 years", "New York", "Software Engineer" },
                    { 2, 7, "5 years", "San Francisco", "Data Scientist" },
                    { 3, 15, "2 years", "Los Angeles", "Web Designer" }
                });

            migrationBuilder.InsertData(
                table: "Referrals",
                columns: new[] { "Id", "CandidateContact", "CandidateName", "ForRole", "RefEmployee", "Status" },
                values: new object[,]
                {
                    { 1, "johndoe@example.com", "John Doe", "Software Engineer", "Alice Smith", "Pending" },
                    { 2, "janesmith@example.com", "Jane Smith", "Data Scientist", "Bob Johnson", "Accepted" },
                    { 3, "tombrown@example.com", "Tom Brown", "Web Designer", "Mary Davis", "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Referrals",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Referrals",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Referrals",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
