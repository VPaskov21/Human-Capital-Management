using Microsoft.EntityFrameworkCore.Migrations;

namespace HCMApp.Migrations
{
    public partial class SalaryTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Users",
                newName: "SalaryId");

            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grossSalary = table.Column<int>(type: "int", nullable: false),
                    netSalary = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SalaryId",
                table: "Users",
                column: "SalaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Salaries_SalaryId",
                table: "Users",
                column: "SalaryId",
                principalTable: "Salaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Salaries_SalaryId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Salaries");

            migrationBuilder.DropIndex(
                name: "IX_Users_SalaryId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SalaryId",
                table: "Users",
                newName: "Salary");
        }
    }
}
