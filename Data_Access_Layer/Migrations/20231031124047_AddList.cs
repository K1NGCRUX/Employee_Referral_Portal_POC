using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class AddList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applied",
                table: "Openings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Applied",
                table: "Openings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 27,
                column: "Applied",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 28,
                column: "Applied",
                value: "[]");

            migrationBuilder.UpdateData(
                table: "Openings",
                keyColumn: "Id",
                keyValue: 29,
                column: "Applied",
                value: "[]");
        }
    }
}
