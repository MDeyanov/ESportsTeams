using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class FixSomething : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Addresses_AddressId",
                table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Teams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Addresses_AddressId",
                table: "Teams",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Addresses_AddressId",
                table: "Teams");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Addresses_AddressId",
                table: "Teams",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
