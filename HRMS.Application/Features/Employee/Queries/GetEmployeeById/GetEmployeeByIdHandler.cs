namespace HRMS.Application.Features.Employee.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdHandler(
    IEmployeeRepository employeeRepository)
    : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailsDto>>
{
    public async Task<Result<EmployeeDetailsDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdWithDetailsAsync(query.Id, cancellationToken);

        return employee is null
            ? Result<EmployeeDetailsDto>.Failure(Error.NotFound("Employee not found"))
            : Result<EmployeeDetailsDto>.Success(employee);
    }
}

