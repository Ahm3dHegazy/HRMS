namespace HRMS.Application.Features.Employee.Queries.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(int Id) : IRequest<Result<EmployeeDetailsDto>>;

