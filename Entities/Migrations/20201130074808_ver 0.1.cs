using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class ver01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "seat",
                table: "Courses",
                newName: "StartPeriod");

            migrationBuilder.RenameColumn(
                name: "AvailableSeat",
                table: "Courses",
                newName: "Size");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rest",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Rest",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "StartPeriod",
                table: "Courses",
                newName: "seat");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Courses",
                newName: "AvailableSeat");
        }
    }
}
