namespace HRMS.Infrastructure.Data.Configurations.Organization;

public sealed class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
{
    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("JobTitles", table =>
            table.HasCheckConstraint("CK_JobTitles_Name_NotWhitespace", "LEN(LTRIM(RTRIM([Name]))) > 0"));
        builder.HasKey(jobTitle => jobTitle.Id);

        builder.Property(jobTitle => jobTitle.Name)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.OrganizationName);
        builder.Property(jobTitle => jobTitle.Description)
            .HasMaxLength(EntityLengthConstraints.OrganizationDescription);

        builder.HasIndex(jobTitle => jobTitle.Name)
            .IsUnique();

        builder.HasMany(jobTitle => jobTitle.Employees)
            .WithOne(employee => employee.JobTitle)
            .HasForeignKey(employee => employee.JobTitleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
