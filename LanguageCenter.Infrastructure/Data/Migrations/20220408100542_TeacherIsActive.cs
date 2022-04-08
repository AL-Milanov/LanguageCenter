using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguageCenter.Infrastructure.Data.Migrations
{
    public partial class TeacherIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teacher",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teacher");
        }
    }
}
