using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class TeacherDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Teachers",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Teachers");
        }
    }
}
