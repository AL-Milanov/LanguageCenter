using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class StudentIdUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Students_StudentId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Students_StudentsStudentId",
                table: "CourseStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudentsStudentId",
                table: "CourseStudent",
                newName: "StudentsId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudent_StudentsStudentId",
                table: "CourseStudent",
                newName: "IX_CourseStudent_StudentsId");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Students",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentId",
                table: "Students",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Students_StudentId",
                table: "Certificates",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Students_StudentsId",
                table: "CourseStudent",
                column: "StudentsId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Students_StudentId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseStudent_Students_StudentsId",
                table: "CourseStudent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudentsId",
                table: "CourseStudent",
                newName: "StudentsStudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseStudent_StudentsId",
                table: "CourseStudent",
                newName: "IX_CourseStudent_StudentsStudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Students_StudentId",
                table: "Certificates",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseStudent_Students_StudentsStudentId",
                table: "CourseStudent",
                column: "StudentsStudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
