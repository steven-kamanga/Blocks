using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineSiteRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Machines",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_SiteId",
                table: "Machines",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_SiteId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Machines");
        }
    }
}
