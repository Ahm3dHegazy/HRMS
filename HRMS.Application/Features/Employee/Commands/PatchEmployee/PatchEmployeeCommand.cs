namespace HRMS.Application.Features.Employee.Commands.PatchEmployee;

public sealed class PatchEmployeeCommand : IRequest<Result<EmployeeDetailsDto>>
{
    public int EmployeeId { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public decimal? BaseSalary { get; set; }
    public int? DepartmentId { get; set; }
    public int? JobTitleId { get; set; }
    public int? ManagerId { get; set; }
    public bool ClearManagerId { get; set; }
}

