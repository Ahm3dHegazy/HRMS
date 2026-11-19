namespace HRMS.Application.Features.Payroll.Queries.GetPayrollById;

public sealed class GetPayrollByIdHandler(IPayrollRepository payrolls) : IRequestHandler<GetPayrollByIdQuery, Result<PayrollDto>>
{
    public async Task<Result<PayrollDto>> Handle(GetPayrollByIdQuery query, CancellationToken cancellationToken)
    {
        var payroll = await payrolls.GetPayrollDtoByIdAsync(query.Id, cancellationToken);
        return payroll is null ? Result<PayrollDto>.Failure(Error.NotFound($"Payroll {query.Id} was not found."))
            : Result<PayrollDto>.Success(payroll);
    }
}
