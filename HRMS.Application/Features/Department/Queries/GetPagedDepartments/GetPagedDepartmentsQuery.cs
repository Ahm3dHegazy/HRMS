namespace HRMS.Application.Features.Department.Queries.GetPagedDepartments;

public sealed record GetPagedDepartmentsQuery(int PageNumber, int PageSize)
    : IRequest<PagedList<DepartmentDetailsDto>>;
