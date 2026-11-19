namespace HRMS.Application.Features.Payroll.Queries.GetEmployeePayrollHistory;

public sealed class GetEmployeePayrollHistoryHandler(IPayrollRepository payrolls) : IRequestHandler<GetEmployeePayrollHistoryQuery, Result<IReadOnlyList<PayrollDto>>>
{
    public async Task<Result<IReadOnlyList<PayrollDto>>> Handle(GetEmployeePayrollHistoryQuery query, CancellationToken cancellationToken)
    {
        if (await payrolls.GetEmployeeForPayrollAsync(query.EmployeeId, cancellationToken) is null)
            return Result<IReadOnlyList<PayrollDto>>.Failure(Error.NotFound($"Employee {query.EmployeeId} was not found."));
        return Result<IReadOnlyList<PayrollDto>>.Success(await payrolls.GetEmployeeHistoryAsync(query.EmployeeId, cancellationToken));
    }
}
