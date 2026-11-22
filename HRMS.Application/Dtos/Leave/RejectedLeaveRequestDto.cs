namespace HRMS.Application.Dtos.Leave;

public sealed record RejectedLeaveRequestDto : LeaveRequestDto
{
    public RejectedLeaveRequestDto(LeaveRequestDto request) : base(request) { }
}
