namespace HRMS.Application.Features.Department.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery : IRequest<IReadOnlyList<DepartmentDetailsDto>>;

