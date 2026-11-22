namespace HRMS.Api.Requests.Department;

public class PatchDepartmentRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ManagerId { get; set; }
}
