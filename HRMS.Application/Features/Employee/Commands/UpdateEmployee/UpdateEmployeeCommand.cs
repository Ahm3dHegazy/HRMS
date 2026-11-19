namespace HRMS.Application.Features.Employee.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommand : IRequest<Result<EmployeeDetailsDto>>
{
    public int EmployeeId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal BaseSalary { get; set; }
    public int DepartmentId { get; set; }
    public int JobTitleId { get; set; }
    public int? ManagerId { get; set; }
}

