using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class addingIsDelitedToReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Teams",
                newName: "IsBanned");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "IsBanned",
                table: "Teams",
                newName: "IsDeleted");
        }
    }
}
