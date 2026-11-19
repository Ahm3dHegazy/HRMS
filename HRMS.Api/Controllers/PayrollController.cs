namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR")]
public sealed class PayrollController(ISender sender) : ApiControllerBase
{
    [HttpPost(Router.PayrollRoutes.GeneratePayroll)]
    public async Task<IActionResult> Generate(int employeeId, int month, int year, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GeneratePayrollCommand(employeeId, month, year), cancellationToken);
        return HandleResult(result, "Payroll generated successfully", StatusCodes.Status201Created);
    }

    [HttpPost(Router.PayrollRoutes.GenerateMonthlyPayroll)]
    public async Task<IActionResult> GenerateMonthly(int month, int year, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GenerateMonthlyPayrollCommand(month, year), cancellationToken);
        return HandleResult(result, "Monthly payroll generated successfully");
    }

    [HttpGet(Router.PayrollRoutes.GetPayrollById)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPayrollByIdQuery(id), cancellationToken);
        return HandleResult(result, "Payroll retrieved successfully");
    }

    [HttpGet(Router.PayrollRoutes.GetEmployeePayrollHistory)]
    public async Task<IActionResult> GetEmployeeHistory(int employeeId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEmployeePayrollHistoryQuery(employeeId), cancellationToken);
        return HandleResult(result, "Employee payroll history retrieved successfully");
    }

    [HttpPost(Router.PayrollRoutes.MarkPayrollAsPaid)]
    public async Task<IActionResult> MarkPaid(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new MarkPayrollAsPaidCommand(id), cancellationToken);
        return HandleResult(result, "Payroll marked as paid successfully");
    }
}
