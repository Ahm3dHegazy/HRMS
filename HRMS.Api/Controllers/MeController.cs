namespace HRMS.Api.Controllers;

[Authorize(Roles = "Employee")]
public sealed class MeController(ISender sender, ICurrentUser currentUser) : ApiControllerBase
{
    [HttpGet(Router.MeRoutes.GetProfile)]
    [EndpointName("GetMyProfile")]
    [EndpointSummary("Get my employee profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var result = await sender.Send(new GetEmployeeByIdQuery(employeeId), cancellationToken);
        return HandleResult(result, "Profile retrieved successfully");
    }

    [HttpGet(Router.MeRoutes.GetAttendance)]
    [EndpointName("GetMyAttendance")]
    [EndpointSummary("Get my attendance records")]
    public async Task<IActionResult> GetAttendance(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var result = await sender.Send(new GetEmployeeAttendanceRecordsQuery(employeeId), cancellationToken);
        return HandleResult(result, "Attendance records retrieved successfully");
    }

    [HttpGet(Router.MeRoutes.GetMonthlyAttendance)]
    [EndpointName("GetMyMonthlyAttendance")]
    [EndpointSummary("Get my attendance records for a month")]
    public async Task<IActionResult> GetMonthlyAttendance(int year, int month,
        CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var result = await sender.Send(
            new GetMonthlyEmployeeAttendanceRecordsQuery(employeeId, year, month), cancellationToken);
        return HandleResult(result, "Monthly attendance records retrieved successfully");
    }

    [HttpGet(Router.MeRoutes.GetLeaveRequests)]
    [EndpointName("GetMyLeaveRequests")]
    [EndpointSummary("Get my leave requests")]
    public async Task<IActionResult> GetLeaveRequests(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var requests = await sender.Send(new GetEmployeeLeaveRequestsQuery(employeeId), cancellationToken);
        return Ok(ApiResponseFactory.Success(requests, "Leave requests retrieved successfully"));
    }

    [HttpGet(Router.MeRoutes.GetLeaveBalances)]
    [EndpointName("GetMyLeaveBalances")]
    [EndpointSummary("Get my leave balances")]
    public async Task<IActionResult> GetLeaveBalances(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var balances = await sender.Send(new GetEmployeeLeaveBalancesQuery(employeeId), cancellationToken);
        return Ok(ApiResponseFactory.Success(balances, "Leave balances retrieved successfully"));
    }

    [HttpGet(Router.MeRoutes.GetPayrollHistory)]
    [EndpointName("GetMyPayrollHistory")]
    [EndpointSummary("Get my payroll history")]
    public async Task<IActionResult> GetPayrollHistory(CancellationToken cancellationToken)
    {
        if (!TryGetEmployeeId(out var employeeId))
            return EmployeeLinkRequired();

        var result = await sender.Send(new GetEmployeePayrollHistoryQuery(employeeId), cancellationToken);
        return HandleResult(result, "Payroll history retrieved successfully");
    }

    private bool TryGetEmployeeId(out int employeeId)
    {
        employeeId = currentUser.EmployeeId.GetValueOrDefault();
        return employeeId > 0;
    }

    private ObjectResult EmployeeLinkRequired() =>
        StatusCode(StatusCodes.Status403Forbidden,
            ApiResponseFactory.Failure<object>("Your account is not linked to an employee record."));
}
