using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class addingMMRforUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchMakingRating",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CSGOMMR",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dota2MMR",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeagueOfLegendsMMR",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PUBGMMR",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VALORANTMMR",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CSGOMMR",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Dota2MMR",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LeagueOfLegendsMMR",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PUBGMMR",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VALORANTMMR",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MatchMakingRating",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
