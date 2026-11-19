namespace HRMS.Application.Features.Department.Commands.AddDepartment;

public class AddDepartmentHandler(IDepartmentRepository repository, IEmployeeRepository employees, IMapper mapper)
    : IRequestHandler<AddDepartmentCommand, UpdatedDepartmentDto>
{
    public async Task<UpdatedDepartmentDto> Handle(AddDepartmentCommand command, CancellationToken cancellationToken)
    {
        var department = mapper.Map<Domain.Entities.Department>(command);

        if (command.ManagerId.HasValue)
            department.AssignManager(await employees.GetByIdAsync(command.ManagerId.Value));

        await repository.AddAsync(department);
        await repository.SaveChangesAsync();

        return mapper.Map<UpdatedDepartmentDto>(department);
    }
}

