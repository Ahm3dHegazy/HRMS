namespace HRMS.Application.Features.Department.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdHandler(IDepartmentRepository repository)
    : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDetailsDto>>
{
    public async Task<Result<DepartmentDetailsDto>> Handle(GetDepartmentByIdQuery query,
        CancellationToken cancellationToken)
    {
        var department = await repository.GetDetailsAsync(query.Id, cancellationToken);

        return department is null
            ? Result<DepartmentDetailsDto>.Failure(Error.NotFound("Department not found"))
            : Result<DepartmentDetailsDto>.Success(department);
    }
}

