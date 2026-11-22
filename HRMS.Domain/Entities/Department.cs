namespace HRMS.Domain.Entities;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    // Relationships

    // Employee
    public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public void Activate() => IsActive = true;

    public bool TryDeactivate(bool hasActiveEmployees)
    {
        if (hasActiveEmployees)
            return false;

        IsActive = false;
        return true;
    }

    public void AssignManager(Employee? manager)
    {
        Manager = manager;
        ManagerId = manager?.Id;
    }
}
