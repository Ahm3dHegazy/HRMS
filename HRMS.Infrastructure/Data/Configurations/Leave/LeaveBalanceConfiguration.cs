namespace HRMS.Infrastructure.Data.Configurations.Leave;

public sealed class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
{
    public void Configure(EntityTypeBuilder<LeaveBalance> builder)
    {
        builder.ToTable("LeaveBalances", table =>
            table.HasCheckConstraint("CK_LeaveBalances_ValidDaysAndYear",
                "[Year] BETWEEN 1 AND 9999 AND [TotalDays] >= 0 AND [UsedDays] >= 0 AND [UsedDays] <= [TotalDays]"));
        builder.HasKey(balance => balance.Id);

        builder.HasIndex(balance => new
        {
            balance.EmployeeId,
            balance.LeaveTypeId,
            balance.Year
        }).IsUnique();

        builder.HasOne(balance => balance.Employee)
            .WithMany(employee => employee.LeaveBalances)
            .HasForeignKey(balance => balance.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(balance => balance.LeaveType)
            .WithMany(leaveType => leaveType.LeaveBalances)
            .HasForeignKey(balance => balance.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
