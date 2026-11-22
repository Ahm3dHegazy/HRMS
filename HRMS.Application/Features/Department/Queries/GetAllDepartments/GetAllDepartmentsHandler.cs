namespace HRMS.Application.Features.Department.Queries.GetAllDepartments;

public sealed class GetAllDepartmentsHandler(IDepartmentRepository repository)
    : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<DepartmentDetailsDto>>
{
    public Task<IReadOnlyList<DepartmentDetailsDto>> Handle(GetAllDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAllDetailsAsync(cancellationToken);
    }
}

