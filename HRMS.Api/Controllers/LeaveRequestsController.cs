namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR,Manager,Employee")]
public sealed class LeaveRequestsController(ISender sender, IMapper mapper) : ApiControllerBase
{
    [HttpGet(Router.LeaveRequestRoutes.GetLeaveRequestById)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("GetLeaveRequestById")]
    [EndpointSummary("Get leave request details")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetLeaveRequestDetailsQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet(Router.LeaveRequestRoutes.GetEmployeeLeaveRequests)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("GetEmployeeLeaveRequests")]
    [EndpointSummary("Get leave requests for an employee")]
    public async Task<IActionResult> GetByEmployee(int employeeId, CancellationToken cancellationToken)
    {
        var requests = await sender.Send(new GetEmployeeLeaveRequestsQuery(employeeId), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests));
    }

    [HttpGet(Router.LeaveRequestRoutes.GetPendingLeaveRequests)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("GetPendingLeaveRequests")]
    [EndpointSummary("Get all pending leave requests")]
    public async Task<IActionResult> GetPending(CancellationToken cancellationToken)
    {
        var requests = await sender.Send(new GetPendingLeaveRequestsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests));
    }

    [HttpGet(Router.LeaveRequestRoutes.GetApprovedLeaveRequests)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("GetApprovedLeaveRequests")]
    [EndpointSummary("Get all approved leave requests")]
    public async Task<IActionResult> GetApproved(CancellationToken cancellationToken)
    {
        var requests = await sender.Send(new GetApprovedLeaveRequestsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests));
    }

    [HttpGet(Router.LeaveRequestRoutes.GetRejectedLeaveRequests)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("GetRejectedLeaveRequests")]
    [EndpointSummary("Get all rejected leave requests")]
    public async Task<IActionResult> GetRejected(CancellationToken cancellationToken)
    {
        var requests = await sender.Send(new GetRejectedLeaveRequestsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests));
    }

    [HttpPost(Router.LeaveRequestRoutes.CreateLeaveRequest)]
    [EndpointName("CreateLeaveRequest")]
    [EndpointSummary("Submit a leave request")]
    public async Task<IActionResult> Create(CreateLeaveRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<CreateLeaveRequestCommand>(request);
        var result = await sender.Send(command, cancellationToken);

        return HandleResult(result, "Leave request submitted successfully", StatusCodes.Status201Created);
    }

    [HttpPost(Router.LeaveRequestRoutes.ApproveLeaveRequest)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("ApproveLeaveRequest")]
    [EndpointSummary("Approve a pending leave request")]
    public async Task<IActionResult> Approve(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ApproveLeaveRequestCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost(Router.LeaveRequestRoutes.RejectLeaveRequest)]
    [Authorize(Roles = "Admin,HR,Manager")]
    [EndpointName("RejectLeaveRequest")]
    [EndpointSummary("Reject a pending leave request")]
    public async Task<IActionResult> Reject(int id, RejectLeaveRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RejectLeaveRequestCommand(id, request.RejectionReason);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost(Router.LeaveRequestRoutes.CancelLeaveRequest)]
    [EndpointName("CancelLeaveRequest")]
    [EndpointSummary("Cancel a pending leave request")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelLeaveRequestCommand(id), cancellationToken);
        return HandleResult(result);
    }
}
