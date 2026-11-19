using HRMS.Application.Mapping;

namespace HRMS.Application.Features.Department.Commands.PatchDepartment;

public class PatchDepartmentHandler(
    IDepartmentRepository repository,
    IEmployeeRepository employees, IMapper mapper)
    : IRequestHandler<PatchDepartmentCommand, Result<PatchedDepartmentDto>>
{
    public async Task<Result<PatchedDepartmentDto>> Handle(PatchDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        var department = await repository.GetByIdAsync(command.Id);
        if (department is null)
            return Result<PatchedDepartmentDto>.Failure(Error.NotFound("Department not found"));

        mapper.Map(command, department);

        if (department.ManagerId.HasValue)
            department.AssignManager(await employees.GetByIdAsync(department.ManagerId.Value));

        await repository.SaveChangesAsync();

        return Result<PatchedDepartmentDto>.Success(mapper.Map<PatchedDepartmentDto>(department));
    }
}

