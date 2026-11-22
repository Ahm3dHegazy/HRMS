namespace HRMS.Domain.Entities;

public class JobTitle
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Relationships

    // Employee
    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    public void UpdateDetails(string name, string? description)
    {
        Name = name.Trim();
        Description = description?.Trim();
    }

    public bool CanBeDeleted(bool hasEmployeesNotTerminated) => !hasEmployeesNotTerminated;
}
