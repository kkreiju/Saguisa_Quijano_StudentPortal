using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
<<<<<<<< Updated upstream:StudentPortal/Migrations/20240902050929_Added Subject.cs
    public partial class AddedSubject : Migration
========
    public partial class Schedule : Migration
>>>>>>>> Stashed changes:StudentPortal/Migrations/20240925025514_Schedule.cs
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
<<<<<<<< Updated upstream:StudentPortal/Migrations/20240902050929_Added Subject.cs
========
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
                    MaxSize = table.Column<int>(type: "int", nullable: false),
                    ClassSize = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
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
                    StudLName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StudFName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StudMName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StudCourse = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StudYear = table.Column<int>(type: "int", nullable: true),
                    StudRemarks = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StudStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudID);
                });

            migrationBuilder.CreateTable(
>>>>>>>> Stashed changes:StudentPortal/Migrations/20240925025514_Schedule.cs
                name: "Subject",
                columns: table => new
                {
                    SubjCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SubjDesc = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubjUnits = table.Column<float>(type: "real", nullable: true),
                    SubjRegOfrng = table.Column<int>(type: "int", nullable: true),
                    SubjCategory = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    SubjStatus = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
<<<<<<<< Updated upstream:StudentPortal/Migrations/20240902050929_Added Subject.cs
                    SubjCourseCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    SubjCurrCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
========
                    SubjCurrCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubjRequisite = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
>>>>>>>> Stashed changes:StudentPortal/Migrations/20240925025514_Schedule.cs
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
<<<<<<<< Updated upstream:StudentPortal/Migrations/20240902050929_Added Subject.cs
========
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
>>>>>>>> Stashed changes:StudentPortal/Migrations/20240925025514_Schedule.cs
                name: "Subject");
        }
    }
}
