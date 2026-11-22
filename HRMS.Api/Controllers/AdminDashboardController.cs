using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardAttendanceTrend;
using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardLeaveSummary;
using HRMS.Application.Features.AdminDashboard.Queries.GetAdminDashboardPayrollSummary;

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
    
    [HttpGet(Router.AdminDashboardRoutes.GetAttendanceTrend)]
    [EndpointName("GetAdminDashboardAttendanceTrend")]
    [EndpointSummary("Get daily attendance counts for the last N days")]
    public async Task<IActionResult> GetAttendanceTrend(
        [FromQuery] int days = 30,
        CancellationToken cancellationToken = default)
    {
        var trend = await sender.Send(new GetAdminDashboardAttendanceTrendQuery(days), cancellationToken);
        return Ok(ApiResponseFactory.Success(trend, "Attendance trend retrieved successfully"));
    }
    
    [HttpGet(Router.AdminDashboardRoutes.GetLeaveSummary)]
    [EndpointName("GetAdminDashboardLeaveSummary")]
    [EndpointSummary("Get leave request counts by status and leave type for a year")]
    public async Task<IActionResult> GetLeaveSummary(
        [FromQuery] int? year,
        CancellationToken cancellationToken)
    {
        var summary = await sender.Send(new GetAdminDashboardLeaveSummaryQuery(year), cancellationToken);
        return Ok(ApiResponseFactory.Success(summary, "Leave summary retrieved successfully"));
    }
    
    [HttpGet(Router.AdminDashboardRoutes.GetPayrollSummary)]
    [EndpointName("GetAdminDashboardPayrollSummary")]
    [EndpointSummary("Get payroll totals for a month")]
    public async Task<IActionResult> GetPayrollSummary(
        [FromQuery] int? month,
        [FromQuery] int? year,
        CancellationToken cancellationToken)
    {
        var summary = await sender.Send(new GetAdminDashboardPayrollSummaryQuery(month, year), cancellationToken);
        return Ok(ApiResponseFactory.Success(summary, "Payroll summary retrieved successfully"));
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
