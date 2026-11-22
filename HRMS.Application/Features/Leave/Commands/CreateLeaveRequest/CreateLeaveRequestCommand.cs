namespace HRMS.Application.Features.Leave.Commands.CreateLeaveRequest;

public sealed class CreateLeaveRequestCommand : IRequest<Result<int>>
{
    public int EmployeeId { get; set; }
    public int LeaveTypeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}

