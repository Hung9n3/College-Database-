using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class ver01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Students",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "IdCard");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "IdCard",
                table: "Students",
                newName: "Id");
        }
    }
}
