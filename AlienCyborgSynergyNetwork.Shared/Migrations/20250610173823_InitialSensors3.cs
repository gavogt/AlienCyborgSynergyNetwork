using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlienCyborgSynergyNetwork.Shared.Migrations
{
    /// <inheritdoc />
    public partial class InitialSensors3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensors_CyborgSession_SessionId",
                table: "sensors");

            migrationBuilder.DropTable(
                name: "CyborgSession");

            migrationBuilder.DropIndex(
                name: "IX_sensors_SessionId",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "sensors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SessionId",
                table: "sensors",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CyborgSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyborgSession", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sensors_SessionId",
                table: "sensors",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_sensors_CyborgSession_SessionId",
                table: "sensors",
                column: "SessionId",
                principalTable: "CyborgSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
