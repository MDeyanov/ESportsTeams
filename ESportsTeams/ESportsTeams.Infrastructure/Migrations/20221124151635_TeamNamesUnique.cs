using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class TeamNamesUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teams_Name",
                table: "Teams");
        }
    }
}
