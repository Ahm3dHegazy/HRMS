namespace HRMS.Application.Dtos.Department;

public class PatchedDepartmentDto
{
    public int  Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ManagerName { get; set; }
}
