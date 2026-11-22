namespace HRMS.Api.Requests.Department;

public class AddDepartmentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? ManagerId { get; set; }
}
