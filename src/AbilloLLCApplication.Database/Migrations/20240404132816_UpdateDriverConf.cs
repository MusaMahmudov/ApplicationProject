using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbilloLLCApplication.Database.Migrations
{
    public partial class UpdateDriverConf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "HeightPositive",
                table: "Drivers",
                sql: "Height > 0");

            migrationBuilder.AddCheckConstraint(
                name: "LengthPositive",
                table: "Drivers",
                sql: "Length > 0");

            migrationBuilder.AddCheckConstraint(
                name: "WidthPositive",
                table: "Drivers",
                sql: "Width > 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "HeightPositive",
                table: "Drivers");

            migrationBuilder.DropCheckConstraint(
                name: "LengthPositive",
                table: "Drivers");

            migrationBuilder.DropCheckConstraint(
                name: "WidthPositive",
                table: "Drivers");
        }
    }
}
