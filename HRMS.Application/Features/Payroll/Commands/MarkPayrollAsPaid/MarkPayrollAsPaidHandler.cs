namespace HRMS.Application.Features.Payroll.Commands.MarkPayrollAsPaid;

public sealed class MarkPayrollAsPaidHandler(IPayrollRepository payrolls, TimeProvider timeProvider)
    : IRequestHandler<MarkPayrollAsPaidCommand, Result<PayrollDto>>
{
    public async Task<Result<PayrollDto>> Handle(MarkPayrollAsPaidCommand command, CancellationToken cancellationToken)
    {
        var payroll = await payrolls.GetByIdAsync(command.PayrollId);
        if (payroll is null) 
            return Result<PayrollDto>.Failure(Error.NotFound($"Payroll {command.PayrollId} was not found."));
        
        var paidAt = timeProvider.GetUtcNow().UtcDateTime;
        
        if (payroll.Status == PayrollStatus.Paid || !await payrolls.TryMarkPayrollPaidAsync(payroll.Id, paidAt, cancellationToken))
            return Result<PayrollDto>.Failure(Error.Conflict("Payroll has already been marked as paid."));
        
        return Result<PayrollDto>.Success((await payrolls.GetPayrollDtoByIdAsync(payroll.Id, cancellationToken))!);
    }
}
