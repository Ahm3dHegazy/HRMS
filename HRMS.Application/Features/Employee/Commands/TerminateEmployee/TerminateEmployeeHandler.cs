namespace HRMS.Application.Features.Employee.Commands.TerminateEmployee;

public class TerminateEmployeeHandler(IEmployeeRepository repository, IMapper mapper)
    : IRequestHandler<TerminateEmployeeCommand, Result<TerminatedEmployeeDto>>
{
    public async Task<Result<TerminatedEmployeeDto>> Handle(TerminateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(command.EmployeeId);

        if (employee == null)
            return Result<TerminatedEmployeeDto>.Failure(Error.NotFound("Employee not found"));

        if (!employee.Terminate(command.TerminationType, command.Reason, command.TerminationDate))
            return Result<TerminatedEmployeeDto>.Failure(Error.Conflict("Employee is already terminated."));

        await repository.SaveChangesAsync();

        return Result<TerminatedEmployeeDto>.Success(mapper.Map<TerminatedEmployeeDto>(employee));
    }
}

