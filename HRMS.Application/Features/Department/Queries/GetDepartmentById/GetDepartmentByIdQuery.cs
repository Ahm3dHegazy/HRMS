namespace HRMS.Application.Features.Department.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(int Id) : IRequest<Result<DepartmentDetailsDto>>;

