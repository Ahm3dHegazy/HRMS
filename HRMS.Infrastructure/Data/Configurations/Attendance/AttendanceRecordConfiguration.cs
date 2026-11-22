namespace HRMS.Infrastructure.Data.Configurations.Attendance;

public sealed class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords", table =>
        {
            table.HasCheckConstraint("CK_AttendanceRecords_CheckOutAfterCheckIn",
                "[CheckOut] IS NULL OR [CheckIn] IS NULL OR [CheckOut] >= [CheckIn]");
            table.HasCheckConstraint("CK_AttendanceRecords_ValidStatus", "[Status] IN (1, 2, 3, 4)");
            table.HasCheckConstraint("CK_AttendanceRecords_Date_Required", "[Date] > '0001-01-01'");
        });
        builder.HasKey(record => record.Id);

        builder.HasIndex(record => new { record.EmployeeId, record.Date })
            .IsUnique();

        builder.HasOne(record => record.Employee)
            .WithMany(employee => employee.AttendanceRecords)
            .HasForeignKey(record => record.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
