namespace HRMS.Application.Features.Payroll.Commands.GeneratePayroll;

public sealed class GeneratePayrollHandler(IPayrollRepository payrolls,
    Microsoft.Extensions.Options.IOptions<HRMS.Application.Common.Payroll.PayrollSettings> settings,
    TimeProvider timeProvider) : IRequestHandler<GeneratePayrollCommand, Result<PayrollDto>>
{
    public async Task<Result<PayrollDto>> Handle(GeneratePayrollCommand command, CancellationToken cancellationToken)
    {
        var settingsError = PayrollGeneration.ValidateSettings(settings.Value.WorkingDaysPerMonth);
        if (settingsError is not null) return Result<PayrollDto>.Failure(settingsError);
        var employee = await payrolls.GetEmployeeForPayrollAsync(command.EmployeeId, cancellationToken);
        if (employee is null) return Result<PayrollDto>.Failure(Error.NotFound($"Employee {command.EmployeeId} was not found."));
        if (employee.Status == EmploymentStatus.Terminated || employee.HireDate > new DateOnly(command.Year, command.Month, DateTime.DaysInMonth(command.Year, command.Month)))
            return Result<PayrollDto>.Failure(Error.Conflict("Employee is not eligible for payroll in the selected month."));
        if (await payrolls.GetForEmployeePeriodAsync(employee.Id, command.Year, command.Month, cancellationToken) is not null)
            return Result<PayrollDto>.Failure(Error.Conflict("Payroll already exists for this employee and month."));

        var (start, endExclusive) = HRMS.Application.Features.Payroll.PayrollGeneration.Period(command.Year, command.Month);
        var attendance = await payrolls.GetAttendanceDaysAsync(employee.Id, start, endExclusive, cancellationToken);
        var leave = await payrolls.GetApprovedLeavePeriodsAsync(employee.Id, start, endExclusive, cancellationToken);
        var payroll = PayrollGeneration.Calculate(employee, command.Month, command.Year,
            settings.Value.WorkingDaysPerMonth!.Value, attendance, leave, timeProvider.GetUtcNow().UtcDateTime);
        if (!await payrolls.TryCreatePayrollAsync(payroll, cancellationToken))
            return Result<PayrollDto>.Failure(Error.Conflict("Payroll was generated concurrently for this employee and month."));
        return Result<PayrollDto>.Success((await payrolls.GetPayrollDtoByIdAsync(payroll.Id, cancellationToken))!);
    }

}
