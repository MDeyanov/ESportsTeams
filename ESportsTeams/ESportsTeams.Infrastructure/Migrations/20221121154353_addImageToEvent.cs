using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class addImageToEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Events");
        }
    }
}
