namespace HRMS.Application.Dtos.Department;

public class DepartmentDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int EmployeesCount { get; set; }
    public string? ManagerName { get; set; } = string.Empty;
}
