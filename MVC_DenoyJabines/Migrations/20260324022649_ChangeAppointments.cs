using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_DenoyJabines.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Students_StudentID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_User_CounselorID",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CounselorID",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CounselorID",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Appointments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_StudentID",
                table: "Appointments",
                newName: "IX_Appointments_UserId");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Appointments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_User_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_User_UserId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Appointments",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                newName: "IX_Appointments_StudentID");

            migrationBuilder.AddColumn<int>(
                name: "CounselorID",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CounselorID",
                table: "Appointments",
                column: "CounselorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Students_StudentID",
                table: "Appointments",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StuID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_User_CounselorID",
                table: "Appointments",
                column: "CounselorID",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
