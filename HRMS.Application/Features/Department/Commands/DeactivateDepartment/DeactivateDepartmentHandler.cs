namespace HRMS.Application.Features.Department.Commands.DeactivateDepartment;

public sealed class DeactivateDepartmentHandler(
    IDepartmentRepository departmentRepository) : IRequestHandler<DeactivateDepartmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeactivateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsync(command.Id);
        
        if (department is null)
            return Result<bool>.Failure(Error.NotFound("Department not found"));

        var hasActiveEmployees = await departmentRepository.HasActiveEmployeesAsync(command.Id, cancellationToken);
        
        if (!department.TryDeactivate(hasActiveEmployees))
            return Result<bool>.Failure(
                Error.Conflict("The department cannot be deactivated while it has active employees"));

        await departmentRepository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}

