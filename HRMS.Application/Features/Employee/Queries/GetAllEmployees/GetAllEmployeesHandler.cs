namespace HRMS.Application.Features.Employee.Queries.GetAllEmployees;

public sealed class GetAllEmployeesHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDetailsDto>>
{
    public async Task<List<EmployeeDetailsDto>> Handle(GetAllEmployeesQuery query,
        CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetAllWithDetailsAsync(cancellationToken);

        return employees.ToList();
    }
}

