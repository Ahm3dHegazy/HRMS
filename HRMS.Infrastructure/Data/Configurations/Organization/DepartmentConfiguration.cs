namespace HRMS.Infrastructure.Data.Configurations.Organization;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments", table =>
            table.HasCheckConstraint("CK_Departments_Name_NotWhitespace", "LEN(LTRIM(RTRIM([Name]))) > 0"));
        builder.HasKey(department => department.Id);

        builder.Property(department => department.Name)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.OrganizationName);
        builder.Property(department => department.Description)
            .HasMaxLength(EntityLengthConstraints.OrganizationDescription);

        builder.HasIndex(department => department.Name)
            .IsUnique();

        builder.HasMany(department => department.Employees)
            .WithOne(employee => employee.Department)
            .HasForeignKey(employee => employee.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(department => department.Manager)
            .WithMany()
            .HasForeignKey(department => department.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
