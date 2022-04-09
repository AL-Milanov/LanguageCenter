using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class UpdatedIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_StudentId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_AspNetUsers_TeacherId",
                table: "Teacher");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Teacher",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_TeacherId",
                table: "Teacher",
                newName: "IX_Teacher_UserId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_StudentId",
                table: "Students",
                newName: "IX_Students_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_AspNetUsers_UserId",
                table: "Teacher",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_AspNetUsers_UserId",
                table: "Teacher");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Teacher",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_UserId",
                table: "Teacher",
                newName: "IX_Teacher_TeacherId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Students",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_UserId",
                table: "Students",
                newName: "IX_Students_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_StudentId",
                table: "Students",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_AspNetUsers_TeacherId",
                table: "Teacher",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
