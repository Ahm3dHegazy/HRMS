namespace HRMS.Api.Requests.Employee;

public sealed class PatchEmployeeRequest
{
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal? BaseSalary { get; set; }
    public int? DepartmentId { get; set; }
    public int? JobTitleId { get; set; }
    public int? ManagerId { get; set; }
    public bool ClearManagerId { get; set; }
}
