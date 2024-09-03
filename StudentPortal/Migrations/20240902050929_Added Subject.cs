using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SubjDesc = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubjUnits = table.Column<float>(type: "real", nullable: true),
                    SubjRegOfrng = table.Column<int>(type: "int", nullable: true),
                    SubjCategory = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SubjStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SubjCourseCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SubjCurrCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.SubjCode);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
