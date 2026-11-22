namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR")]
public sealed class PayrollController(ISender sender) : ApiControllerBase
{
    [HttpPost(Router.PayrollRoutes.GeneratePayroll)]
    [EndpointName("GenerateEmployeePayroll")]
    [EndpointSummary("Generate payroll for one employee")]
    [EndpointDescription("Calculates and creates payroll for the specified employee and month. Returns a conflict if payroll already exists for that period.")]
    public async Task<IActionResult> Generate(int employeeId, int month, int year, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GeneratePayrollCommand(employeeId, month, year), cancellationToken);
        return HandleResult(result, "Payroll generated successfully", StatusCodes.Status201Created);
    }

    [HttpPost(Router.PayrollRoutes.GenerateMonthlyPayroll)]
    [EndpointName("GenerateMonthlyPayroll")]
    [EndpointSummary("Generate payroll for all eligible employees")]
    [EndpointDescription("Calculates and creates payroll records for eligible employees for the specified month and year, then returns the payroll records for that period.")]
    public async Task<IActionResult> GenerateMonthly(int month, int year, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GenerateMonthlyPayrollCommand(month, year), cancellationToken);
        return HandleResult(result, "Monthly payroll generated successfully");
    }

    [HttpGet(Router.PayrollRoutes.GetPayrollById)]
    [EndpointName("GetPayrollById")]
    [EndpointSummary("Get a payroll record by ID")]
    [EndpointDescription("Returns the payroll record with the specified ID, including salary amounts, deductions, and payment status.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPayrollByIdQuery(id), cancellationToken);
        return HandleResult(result, "Payroll retrieved successfully");
    }

    [HttpGet(Router.PayrollRoutes.GetEmployeePayrollHistory)]
    [EndpointName("GetEmployeePayrollHistory")]
    [EndpointSummary("Get an employee's payroll history")]
    [EndpointDescription("Returns payroll records for the specified employee. Returns not found if the employee does not exist.")]
    public async Task<IActionResult> GetEmployeeHistory(int employeeId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEmployeePayrollHistoryQuery(employeeId), cancellationToken);
        return HandleResult(result, "Employee payroll history retrieved successfully");
    }

    [HttpPost(Router.PayrollRoutes.MarkPayrollAsPaid)]
    [EndpointName("MarkPayrollAsPaid")]
    [EndpointSummary("Mark a payroll record as paid")]
    [EndpointDescription("Marks the specified payroll record as paid. Returns not found if the payroll record does not exist.")]
    public async Task<IActionResult> MarkPaid(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new MarkPayrollAsPaidCommand(id), cancellationToken);
        return HandleResult(result, "Payroll marked as paid successfully");
    }
}
