using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class Encoder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrollmentDetail",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    EDPCode = table.Column<int>(type: "int", nullable: false),
                    SubjCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentDetail", x => new { x.ID, x.EDPCode });
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentHeader",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateEnroll = table.Column<DateOnly>(type: "date", nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Encoder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalUnits = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentHeader", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    EDPCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    MaxSize = table.Column<int>(type: "int", nullable: true),
                    ClassSize = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Course = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Section = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.EDPCode);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudLName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StudFName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StudMName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    StudCourse = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StudYear = table.Column<int>(type: "int", nullable: true),
                    StudRemarks = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StudStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudID);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SubjCourseCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SubjDesc = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubjUnits = table.Column<float>(type: "real", nullable: true),
                    SubjRegOfrng = table.Column<int>(type: "int", nullable: true),
                    SubjCategory = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SubjStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    SubjCurrCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubjRequisite = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => new { x.SubjCode, x.SubjCourseCode });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentDetail");

            migrationBuilder.DropTable(
                name: "EnrollmentHeader");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
