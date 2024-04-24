using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbilloLLCApplication.Database.Migrations
{
    public partial class again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverOffers");

            migrationBuilder.AddColumn<Guid>(
                name: "CargoId",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CargoId",
                table: "Offers",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DriverId",
                table: "Offers",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Cargoes_CargoId",
                table: "Offers",
                column: "CargoId",
                principalTable: "Cargoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Drivers_DriverId",
                table: "Offers",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Cargoes_CargoId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Drivers_DriverId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_CargoId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_DriverId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Offers");

            migrationBuilder.CreateTable(
                name: "DriverOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverOffers_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverOffers_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverOffers_DriverId",
                table: "DriverOffers",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverOffers_OfferId",
                table: "DriverOffers",
                column: "OfferId");
        }
    }
}
