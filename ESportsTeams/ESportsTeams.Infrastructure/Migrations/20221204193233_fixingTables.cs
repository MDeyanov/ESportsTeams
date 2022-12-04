using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ESportsTeams.Infrastructure.Migrations
{
    public partial class fixingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_AppUserId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_Request_AppUserId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Request",
                newName: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_TeamId",
                table: "Request",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Teams_TeamId",
                table: "Request",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Teams_TeamId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_Request_TeamId",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Request",
                newName: "Category");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Request",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Request_AppUserId",
                table: "Request",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_AppUserId",
                table: "Request",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
