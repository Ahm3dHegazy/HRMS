namespace HRMS.Infrastructure.Data.Configurations.Employees;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees", table =>
        {
            table.HasCheckConstraint("CK_Employees_BaseSalary_Minimum", "[BaseSalary] > 10000");
            table.HasCheckConstraint("CK_Employees_ValidStatus", "[Status] IN (1, 2, 3)");
            table.HasCheckConstraint("CK_Employees_NationalId_Format",
                "LEN([NationalId]) = 14 AND [NationalId] NOT LIKE '%[^0-9]%'");
            table.HasCheckConstraint("CK_Employees_HireDate_Required", "[HireDate] > '0001-01-01'");
            table.HasCheckConstraint("CK_Employees_NonWhitespaceRequiredText",
                "LEN(LTRIM(RTRIM([FullName]))) > 0 AND LEN(LTRIM(RTRIM([Address]))) > 0 " +
                "AND LEN(LTRIM(RTRIM([Email]))) > 0 AND LEN(LTRIM(RTRIM([Phone]))) > 0 " +
                "AND LEN(LTRIM(RTRIM([NationalId]))) > 0");
        });
        builder.HasKey(employee => employee.Id);

        builder.Property(employee => employee.FullName)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.EmployeeName);
        builder.Property(employee => employee.Address)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.EmployeeAddress);
        builder.Property(employee => employee.Email)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.EmployeeEmail);
        builder.Property(employee => employee.Phone)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.EmployeePhone);
        builder.Property(employee => employee.NationalId)
            .IsRequired()
            .HasMaxLength(EntityLengthConstraints.EmployeeNationalId);
        builder.Property(employee => employee.BaseSalary)
            .HasPrecision(18, 2);
        builder.Property(employee => employee.TerminationReason)
            .HasMaxLength(EntityLengthConstraints.EmployeeTerminationReason);

        builder.HasIndex(employee => employee.Email)
            .IsUnique();
        builder.HasIndex(employee => employee.NationalId)
            .IsUnique();
        builder.HasIndex(employee => employee.Phone)
            .IsUnique();

        builder.HasOne(employee => employee.Manager)
            .WithMany(manager => manager.Subordinates)
            .HasForeignKey(employee => employee.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
