namespace HRMS.Application.Features.Employee.Commands.AddEmployee;

public sealed class AddEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper) : IRequestHandler<AddEmployeeCommand>
{
    public async Task Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = mapper.Map<Domain.Entities.Employee>(command);
        
        employee.Hire();

        await employeeRepository.AddAsync(employee);
        await employeeRepository.SaveChangesAsync();
    }
}

