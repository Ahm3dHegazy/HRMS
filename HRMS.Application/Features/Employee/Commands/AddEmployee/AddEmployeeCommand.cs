namespace HRMS.Application.Features.Employee.Commands.AddEmployee;

public class AddEmployeeCommand : IRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int JobTitleId { get; set; }
    public DateOnly HireDate { get; set; }
    public decimal BaseSalary { get; set; }
    public int ManagerId { get; set; }
}

