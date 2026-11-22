using Microsoft.Extensions.Options;

namespace HRMS.Application.Features.Payroll.Commands.GenerateMonthlyPayroll;

public sealed class GenerateMonthlyPayrollHandler(IPayrollRepository payrolls,
    IOptions<HRMS.Application.Common.Payroll.PayrollSettings> settings,
    TimeProvider timeProvider) : IRequestHandler<GenerateMonthlyPayrollCommand, Result<IReadOnlyList<PayrollDto>>>
{
    public async Task<Result<IReadOnlyList<PayrollDto>>> Handle(GenerateMonthlyPayrollCommand command,
        CancellationToken cancellationToken)
    {
        var settingsError = PayrollGeneration.ValidateSettings(settings.Value.WorkingDaysPerMonth);
        if (settingsError is not null) 
            return Result<IReadOnlyList<PayrollDto>>.Failure(settingsError);
        
        var (start, endExclusive) = PayrollGeneration.Period(command.Year, command.Month);
        var employees = await payrolls.GetEligibleEmployeesAsync(endExclusive.AddDays(-1), cancellationToken);
        foreach (var employee in employees)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await payrolls.GetForEmployeePeriodAsync(employee.Id, command.Year, command.Month, cancellationToken) is not null)
                continue;
            var attendance = await payrolls.GetAttendanceDaysAsync(employee.Id, start, endExclusive, cancellationToken);
            var leave = await payrolls.GetApprovedLeavePeriodsAsync(employee.Id, start, endExclusive, cancellationToken);
            var payroll = PayrollGeneration.Calculate(employee, command.Month, command.Year,
                settings.Value.WorkingDaysPerMonth!.Value, attendance, leave, timeProvider.GetUtcNow().UtcDateTime);
            await payrolls.TryCreatePayrollAsync(payroll, cancellationToken);
        }
        var monthPayrolls = await payrolls.GetMonthPayrollDtosAsync(command.Year, command.Month, cancellationToken);
        return Result<IReadOnlyList<PayrollDto>>.Success(monthPayrolls);
    }
}
