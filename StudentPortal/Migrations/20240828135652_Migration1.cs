using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudRemarks",
                table: "Student");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudRemarks",
                table: "Student",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
