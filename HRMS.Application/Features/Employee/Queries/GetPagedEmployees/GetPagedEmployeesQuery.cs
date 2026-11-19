namespace HRMS.Application.Features.Employee.Queries.GetPagedEmployees;

public class GetPagedEmployeesQuery : IRequest<PagedList<EmployeeDetailsDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public EmployeeFilter EmployeeFilter { get; set; } = new();
}

