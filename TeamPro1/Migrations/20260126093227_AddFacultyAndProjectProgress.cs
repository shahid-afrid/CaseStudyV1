using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamPro1.Migrations
{
    /// <inheritdoc />
    public partial class AddFacultyAndProjectProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    AssignedFacultyId = table.Column<int>(type: "int", nullable: true),
                    ProblemStatement = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompletionPercentage = table.Column<int>(type: "int", nullable: false),
                    ProofUploads = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyReview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectProgresses_Faculties_AssignedFacultyId",
                        column: x => x.AssignedFacultyId,
                        principalTable: "Faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProjectProgresses_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProgresses_AssignedFacultyId",
                table: "ProjectProgresses",
                column: "AssignedFacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProgresses_TeamId",
                table: "ProjectProgresses",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectProgresses");

            migrationBuilder.DropTable(
                name: "Faculties");
        }
    }
}
