namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin")]
public class AdminDashboardController(ISender sender) : ApiControllerBase
{
    [HttpGet(Router.AdminDashboardRoutes.GetOverview)]
    [EndpointName("GetAdminDashboardOverview")]
    [EndpointSummary("Get admin dashboard overview")]
    public async Task<IActionResult> GetOverview(CancellationToken cancellationToken)
    {
        var dashboard = await sender.Send(new GetAdminDashboardQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(dashboard, "Admin dashboard overview retrieved successfully"));
    }

    [HttpGet(Router.AdminDashboardRoutes.GetAttendance)]
    [EndpointName("GetAdminDashboardAttendance")]
    [EndpointSummary("Get today's attendance summary")]
    public async Task<IActionResult> GetAttendance(CancellationToken cancellationToken)
    {
        var attendance = await sender.Send(new GetAdminDashboardAttendanceQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(attendance, "Today's attendance retrieved successfully"));
    }

    [HttpGet(Router.AdminDashboardRoutes.GetPendingLeaves)]
    [EndpointName("GetAdminDashboardPendingLeaves")]
    [EndpointSummary("Get pending leave requests")]
    public async Task<IActionResult> GetPendingLeaves(CancellationToken cancellationToken)
    {
        var requests = await sender.Send(new GetPendingLeaveRequestsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests, "Pending leave requests retrieved successfully"));
    }

    [HttpGet(Router.AdminDashboardRoutes.GetRecentActivities)]
    [EndpointName("GetAdminDashboardRecentActivities")]
    [EndpointSummary("Get recent HR activities")]
    public async Task<IActionResult> GetRecentActivities(CancellationToken cancellationToken)
    {
        var activities = await sender.Send(new GetAdminDashboardRecentActivitiesQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(activities, "Recent activities retrieved successfully"));
    }
}
