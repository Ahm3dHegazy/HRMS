namespace HRMS.Application.Features.Employee.Commands.UpdateEmployee;

public sealed class UpdateEmployeeHandler(
    IEmployeeRepository employeeRepository,
    IMapper mapper)
    : IRequestHandler<UpdateEmployeeCommand, Result<EmployeeDetailsDto>>
{
    public async Task<Result<EmployeeDetailsDto>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);
        if (employee is null) 
            return Result<EmployeeDetailsDto>.Failure(Error.NotFound("Employee not found"));

        mapper.Map(command, employee);
        await employeeRepository.SaveChangesAsync();

        var updated = await employeeRepository.GetByIdWithDetailsAsync(command.EmployeeId, cancellationToken);
        return updated is null
            ? Result<EmployeeDetailsDto>.Failure(Error.NotFound("Employee not found"))
            : Result<EmployeeDetailsDto>.Success(updated);
    }
}

