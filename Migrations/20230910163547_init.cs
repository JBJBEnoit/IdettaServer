using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdettaTestServer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Func = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Filename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityDetails",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDetails", x => new { x.ActivityId, x.StudentId, x.InstructorId });
                    table.ForeignKey(
                        name: "FK_ActivityDetails_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityDetails_Person_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentInstructor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInstructor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentInstructor_Person_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentInstructor_Person_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentTeacherId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    SoundRecordingPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAssigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleTask_StudentInstructor_StudentTeacherId",
                        column: x => x.StudentTeacherId,
                        principalTable: "StudentInstructor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDetails_StudentId",
                table: "ActivityDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleTask_StudentTeacherId",
                table: "ScheduleTask",
                column: "StudentTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstructor_InstructorId",
                table: "StudentInstructor",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentInstructor_StudentId",
                table: "StudentInstructor",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDetails");

            migrationBuilder.DropTable(
                name: "ScheduleTask");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "StudentInstructor");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
