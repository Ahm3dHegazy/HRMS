namespace HRMS.Application.Dtos.Employee;

public sealed class EmployeeDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string JobName { get; set; } = string.Empty;
}
