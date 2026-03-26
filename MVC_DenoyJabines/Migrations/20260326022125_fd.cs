using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_DenoyJabines.Migrations
{
    /// <inheritdoc />
    public partial class fd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseNotes",
                columns: table => new
                {
                    CaseNoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StuID = table.Column<int>(type: "int", nullable: false),
                    BackgroundOfCase = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CounselingApproach = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CounselingGoals = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Recommendations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseNotes", x => x.CaseNoteId);
                    table.ForeignKey(
                        name: "FK_CaseNotes_Students_StuID",
                        column: x => x.StuID,
                        principalTable: "Students",
                        principalColumn: "StuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseNotes_StuID",
                table: "CaseNotes",
                column: "StuID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseNotes");
        }
    }
}
