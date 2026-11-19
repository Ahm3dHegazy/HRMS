namespace HRMS.Application.Features.Employee.Queries.GetAllEmployees;

public sealed record GetAllEmployeesQuery : IRequest<List<EmployeeDetailsDto>>;

