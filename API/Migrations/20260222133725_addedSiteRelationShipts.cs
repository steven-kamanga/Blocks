using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class addedSiteRelationShipts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Sites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Sites",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_CreatedById",
                table: "Sites",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Sites_AspNetUsers_CreatedById",
                table: "Sites",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sites_AspNetUsers_CreatedById",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_CreatedById",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Sites");
        }
    }
}
