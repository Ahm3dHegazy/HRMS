namespace HRMS.Application.Features.Department.Commands.DeactivateDepartment;

public sealed record DeactivateDepartmentCommand(int Id) : IRequest<Result<bool>>;

