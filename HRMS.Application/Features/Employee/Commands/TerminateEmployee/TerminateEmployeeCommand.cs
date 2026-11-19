namespace HRMS.Application.Features.Employee.Commands.TerminateEmployee;

public class TerminateEmployeeCommand : IRequest<Result<TerminatedEmployeeDto>>
{
    public int EmployeeId { get; set; }
    public TerminationType TerminationType { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateOnly TerminationDate { get; set; }
}

