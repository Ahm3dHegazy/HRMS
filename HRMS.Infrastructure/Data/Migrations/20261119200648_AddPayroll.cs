using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Data.Migrations;

public partial class AddPayroll : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Payrolls",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                EmployeeId = table.Column<int>(type: "int", nullable: false),
                Month = table.Column<int>(type: "int", nullable: false),
                Year = table.Column<int>(type: "int", nullable: false),
                BaseSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                AbsenceDeduction = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                LateDeduction = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                UnpaidLeaveDeduction = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                NetSalary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payrolls", payroll => payroll.Id);
                table.CheckConstraint("CK_Payrolls_Amounts", "[BaseSalary] >= 0 AND [AbsenceDeduction] >= 0 AND [LateDeduction] >= 0 AND [UnpaidLeaveDeduction] >= 0");
                table.CheckConstraint("CK_Payrolls_Period", "[Month] BETWEEN 1 AND 12 AND [Year] BETWEEN 1 AND 9999");
                table.CheckConstraint("CK_Payrolls_Status", "[Status] IN (1, 2)");
                table.ForeignKey("FK_Payrolls_Employees_EmployeeId", payroll => payroll.EmployeeId,
                    "Employees", "Id", onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex("IX_Payrolls_EmployeeId_Year_Month", "Payrolls",
            new[] { "EmployeeId", "Year", "Month" }, unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable("Payrolls");
}
