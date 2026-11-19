namespace HRMS.Application.Dtos.Department;

public class UpdatedDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? ManagerName { get; set; }
}
