using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbilloLLCApplication.Database.Migrations
{
    public partial class LengthOfUserNameTelegram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "TelegramUserIdLimit",
                table: "Drivers",
                sql: "Len(TelegramUserId) > 6");

            migrationBuilder.AddCheckConstraint(
                name: "TelegramUserNameLimit",
                table: "Drivers",
                sql: "Len(TelegramUserName) > 2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "TelegramUserIdLimit",
                table: "Drivers");

            migrationBuilder.DropCheckConstraint(
                name: "TelegramUserNameLimit",
                table: "Drivers");
        }
    }
}
