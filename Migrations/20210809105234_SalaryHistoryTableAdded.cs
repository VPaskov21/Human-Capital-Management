using Microsoft.EntityFrameworkCore.Migrations;

namespace HCMApp.Migrations
{
    public partial class SalaryHistoryTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkMonths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    WorkDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMonths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkedOutDays = table.Column<int>(type: "int", nullable: false),
                    PaidLeaveUsed = table.Column<int>(type: "int", nullable: true),
                    UnpaidLeaveUsed = table.Column<int>(type: "int", nullable: true),
                    SickLeaveUsed = table.Column<int>(type: "int", nullable: true),
                    OtherLeaveUsed = table.Column<int>(type: "int", nullable: true),
                    Bonus = table.Column<double>(type: "float", nullable: true),
                    TotalSalary = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkMonthId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryHistories_WorkMonths_WorkMonthId",
                        column: x => x.WorkMonthId,
                        principalTable: "WorkMonths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistories_UserId",
                table: "SalaryHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryHistories_WorkMonthId",
                table: "SalaryHistories",
                column: "WorkMonthId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryHistories");

            migrationBuilder.DropTable(
                name: "WorkMonths");
        }
    }
}
