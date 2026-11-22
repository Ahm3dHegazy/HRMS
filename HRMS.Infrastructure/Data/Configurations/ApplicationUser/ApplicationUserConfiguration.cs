namespace HRMS.Infrastructure.Data.Configurations.ApplicationUser;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<Identity.ApplicationUser>
{
    public void Configure(EntityTypeBuilder<Identity.ApplicationUser> builder)
    {
        builder.HasOne(u => u.Employee)
            .WithOne()
            .HasForeignKey<Identity.ApplicationUser>(u => u.EmployeeId)
            .IsRequired(false);
    }
}