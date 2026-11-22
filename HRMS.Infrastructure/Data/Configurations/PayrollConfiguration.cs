namespace HRMS.Infrastructure.Data.Configurations;

public sealed class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.ToTable("Payrolls", table =>
        {
            table.HasCheckConstraint("CK_Payrolls_Period", "[Month] BETWEEN 1 AND 12 AND [Year] BETWEEN 1 AND 9999");
            table.HasCheckConstraint("CK_Payrolls_Amounts", "[BaseSalary] >= 0 AND [AbsenceDeduction] >= 0 AND [LateDeduction] >= 0 AND [UnpaidLeaveDeduction] >= 0");
            table.HasCheckConstraint("CK_Payrolls_Status", "[Status] IN (1, 2)");
        });
        builder.HasKey(payroll => payroll.Id);
        builder.HasIndex(payroll => new { payroll.EmployeeId, payroll.Year, payroll.Month }).IsUnique();
        builder.Property(payroll => payroll.BaseSalary).HasPrecision(18, 2);
        builder.Property(payroll => payroll.AbsenceDeduction).HasPrecision(18, 2);
        builder.Property(payroll => payroll.LateDeduction).HasPrecision(18, 2);
        builder.Property(payroll => payroll.UnpaidLeaveDeduction).HasPrecision(18, 2);
        builder.Property(payroll => payroll.NetSalary).HasPrecision(18, 2);
        builder.HasOne(payroll => payroll.Employee).WithMany(employee => employee.Payrolls)
            .HasForeignKey(payroll => payroll.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
