using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class SubjPreq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubjectPreq",
                columns: table => new
                {
                    SPSubjCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SPSubjPreCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SPSubjCategory = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectPreq", x => new { x.SPSubjCode, x.SPSubjPreCode });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectPreq");
        }
    }
}
