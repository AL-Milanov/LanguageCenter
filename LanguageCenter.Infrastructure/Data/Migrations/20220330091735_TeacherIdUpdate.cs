using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class TeacherIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teacher_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Teacher",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_TeacherId",
                table: "Teacher",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teacher_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teacher_TeacherId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_TeacherId",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Teacher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teacher",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teacher_TeacherId",
                table: "Courses",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "TeacherId");
        }
    }
}
