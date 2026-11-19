namespace HRMS.Application.Features.Department.Commands.ActivateDepartment;

public sealed class ActivateDepartmentHandler(IDepartmentRepository departmentRepository)
    : IRequestHandler<ActivateDepartmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(command.Id);
        
        if (department is null)
            return Result<bool>.Failure(Error.NotFound("Department not found"));

        department.Activate();
        await departmentRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}

