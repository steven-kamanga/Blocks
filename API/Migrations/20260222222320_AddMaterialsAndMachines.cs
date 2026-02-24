using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialsAndMachines : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AggregateRatio",
                table: "BatchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AggregateUsed",
                table: "BatchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CementRatio",
                table: "BatchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CementUsed",
                table: "BatchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "BatchItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SandRatio",
                table: "BatchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SandUsed",
                table: "BatchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WaterUsed",
                table: "BatchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BlocksPerBatch = table.Column<int>(type: "INTEGER", nullable: false),
                    BlockSize = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    StockQuantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnitCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawMaterials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BatchItems_MachineId",
                table: "BatchItems",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_BatchItems_Machines_MachineId",
                table: "BatchItems",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BatchItems_Machines_MachineId",
                table: "BatchItems");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "RawMaterials");

            migrationBuilder.DropIndex(
                name: "IX_BatchItems_MachineId",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "AggregateRatio",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "AggregateUsed",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "CementRatio",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "CementUsed",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "SandRatio",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "SandUsed",
                table: "BatchItems");

            migrationBuilder.DropColumn(
                name: "WaterUsed",
                table: "BatchItems");
        }
    }
}
