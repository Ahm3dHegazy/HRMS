namespace HRMS.Infrastructure.Data.Configurations.Leave;

public sealed class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.ToTable("LeaveTypes", table =>
        {
            table.HasCheckConstraint("CK_LeaveTypes_DefaultDaysPerYear_Positive", "[DefaultDaysPerYear] > 0");
            table.HasCheckConstraint("CK_LeaveTypes_Name_NotWhitespace", "LEN(LTRIM(RTRIM([Name]))) > 0");
        });
        builder.HasKey(leaveType => leaveType.Id);

        builder.Property(leaveType => leaveType.Name)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.OrganizationName);

        builder.HasIndex(leaveType => leaveType.Name)
            .IsUnique();
    }
}
