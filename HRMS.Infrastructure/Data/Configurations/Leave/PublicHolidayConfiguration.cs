namespace HRMS.Infrastructure.Data.Configurations.Leave;

public sealed class PublicHolidayConfiguration : IEntityTypeConfiguration<PublicHoliday>
{
    public void Configure(EntityTypeBuilder<PublicHoliday> builder)
    {
        builder.ToTable("PublicHolidays", table =>
        {
            table.HasCheckConstraint("CK_PublicHolidays_Name_NotWhitespace", "LEN(LTRIM(RTRIM([Name]))) > 0");
            table.HasCheckConstraint("CK_PublicHolidays_Date_Required", "[Date] > '0001-01-01'");
        });
        builder.HasKey(holiday => holiday.Id);

        builder.Property(holiday => holiday.Name)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.PublicHolidayName);

        builder.HasIndex(holiday => holiday.Date)
            .IsUnique();
    }
}
