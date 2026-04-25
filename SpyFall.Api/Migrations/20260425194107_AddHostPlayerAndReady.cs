using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpyFall.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHostPlayerAndReady : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReady",
                table: "Players",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HostPlayerId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_HostPlayerId",
                table: "Games",
                column: "HostPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_HostPlayerId",
                table: "Games",
                column: "HostPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_HostPlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_HostPlayerId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "IsReady",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "HostPlayerId",
                table: "Games");
        }
    }
}
