namespace HRMS.Application.Features.Department.Queries.GetPagedDepartments;

public sealed class GetPagedDepartmentsHandler(IDepartmentRepository repository)
    : IRequestHandler<GetPagedDepartmentsQuery, PagedList<DepartmentDetailsDto>>
{
    public Task<PagedList<DepartmentDetailsDto>> Handle(GetPagedDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetPagedDetailsAsync(query.PageNumber, query.PageSize, cancellationToken);
    }
}
