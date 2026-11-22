namespace HRMS.Api.Requests.Leave;

public sealed class RejectLeaveRequestRequest
{
    public string RejectionReason { get; set; } = string.Empty;
}
