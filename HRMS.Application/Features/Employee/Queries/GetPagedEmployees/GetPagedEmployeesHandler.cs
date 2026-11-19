namespace HRMS.Application.Features.Employee.Queries.GetPagedEmployees;

public class GetPagedEmployeesHandler(IEmployeeRepository repository)
    : IRequestHandler<GetPagedEmployeesQuery, PagedList<EmployeeDetailsDto>>
{
    public async Task<PagedList<EmployeeDetailsDto>> Handle(GetPagedEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        var totalCount = await repository.GetFilteredEmployeesCountAsync(query.EmployeeFilter,
            cancellationToken);

        var items = await repository.GetEmployeesPageAsync(
            query.PageNumber,
            query.PageSize,
            query.EmployeeFilter,
            cancellationToken);

        return new PagedList<EmployeeDetailsDto>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}

