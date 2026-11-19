namespace HRMS.Application.Dtos.Leave;

public sealed record ApprovedLeaveRequestDto : LeaveRequestDto
{
    public ApprovedLeaveRequestDto(LeaveRequestDto request) : base(request) { }
}
