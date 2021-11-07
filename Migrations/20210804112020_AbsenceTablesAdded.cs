using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HCMApp.Migrations
{
    public partial class AbsenceTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Absence_Reasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReasonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalLeaveDays = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absence_Reasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leave_Days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalLeaveDays = table.Column<int>(type: "int", nullable: false),
                    AvailableLeaveDays = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave_Days", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leave_Days_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Absence_Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Absence_ReasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absence_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absence_Requests_Absence_Reasons_Absence_ReasonId",
                        column: x => x.Absence_ReasonId,
                        principalTable: "Absence_Reasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Absence_Requests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absence_Requests_Absence_ReasonId",
                table: "Absence_Requests",
                column: "Absence_ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Absence_Requests_UserId",
                table: "Absence_Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_Days_UserId",
                table: "Leave_Days",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absence_Requests");

            migrationBuilder.DropTable(
                name: "Leave_Days");

            migrationBuilder.DropTable(
                name: "Absence_Reasons");
        }
    }
}
