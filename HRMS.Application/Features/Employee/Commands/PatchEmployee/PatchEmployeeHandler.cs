namespace HRMS.Application.Features.Employee.Commands.PatchEmployee;

public sealed class PatchEmployeeHandler(
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<PatchEmployeeCommand, Result<EmployeeDetailsDto>>
{
    public async Task<Result<EmployeeDetailsDto>> Handle(PatchEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);
        
        if (employee is null) 
            return Result<EmployeeDetailsDto>.Failure(Error.NotFound("Employee not found"));

        mapper.Map(command, employee);
        employee.UpdateManager(command.ManagerId, command.ClearManagerId);

        await employeeRepository.SaveChangesAsync();
        
        var updated = await employeeRepository.GetByIdWithDetailsAsync(command.EmployeeId, cancellationToken);
        return updated is null
            ? Result<EmployeeDetailsDto>.Failure(Error.NotFound("Employee not found"))
            : Result<EmployeeDetailsDto>.Success(updated);
    }
}

