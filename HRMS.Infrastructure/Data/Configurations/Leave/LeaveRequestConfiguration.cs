namespace HRMS.Infrastructure.Data.Configurations.Leave;

public sealed class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.ToTable("LeaveRequests", table =>
        {
            table.HasCheckConstraint("CK_LeaveRequests_DateRange",
                "[StartDate] > '0001-01-01' AND [EndDate] > '0001-01-01' AND [StartDate] <= [EndDate]");
            table.HasCheckConstraint("CK_LeaveRequests_ValidStatus", "[Status] IN (1, 2, 3, 4)");
            table.HasCheckConstraint("CK_LeaveRequests_Reason_NotWhitespace",
                "LEN(LTRIM(RTRIM([Reason]))) > 0");
        });
        builder.HasKey(request => request.Id);

        builder.Property(request => request.Reason)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.LeaveReason);
        builder.Property(request => request.RejectionReason)
            .HasMaxLength(EntityLengthConstraints.LeaveReason);

        builder.HasIndex(request => new { request.EmployeeId, request.StartDate, request.EndDate });
        builder.HasIndex(request => request.Status);

        builder.HasOne(request => request.Employee)
            .WithMany(employee => employee.LeaveRequests)
            .HasForeignKey(request => request.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(request => request.ReviewedBy)
            .WithMany(employee => employee.ReviewedLeaveRequests)
            .HasForeignKey(request => request.ReviewedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(request => request.LeaveType)
            .WithMany(leaveType => leaveType.LeaveRequests)
            .HasForeignKey(request => request.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
