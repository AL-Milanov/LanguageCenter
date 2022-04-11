using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class TeachersInLanguages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_TeacherId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Languages");

            migrationBuilder.CreateTable(
                name: "LanguageTeacher",
                columns: table => new
                {
                    LanguagesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeachersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTeacher", x => new { x.LanguagesId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_LanguageTeacher_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LanguageTeacher_Teacher_TeachersId",
                        column: x => x.TeachersId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTeacher_TeachersId",
                table: "LanguageTeacher",
                column: "TeachersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LanguageTeacher");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Languages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_TeacherId",
                table: "Languages",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_Teacher_TeacherId",
                table: "Languages",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "Id");
        }
    }
}
