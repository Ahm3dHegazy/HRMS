namespace HRMS.Application.Dtos.Leave;

public sealed record PendingLeaveRequestDto : LeaveRequestDto
{
    public PendingLeaveRequestDto(LeaveRequestDto request) : base(request) { }
}
