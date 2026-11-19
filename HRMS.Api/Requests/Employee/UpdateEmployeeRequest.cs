namespace HRMS.Api.Requests.Employee;

public sealed class UpdateEmployeeRequest
{
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public int DepartmentId { get; set; }
    public int JobTitleId { get; set; }
    public int? ManagerId { get; set; }
}
