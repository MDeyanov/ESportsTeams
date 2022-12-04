using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class requesterId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequesterId",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "Request");
        }
    }
}
