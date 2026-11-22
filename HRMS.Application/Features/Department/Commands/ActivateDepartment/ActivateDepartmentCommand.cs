namespace HRMS.Application.Features.Department.Commands.ActivateDepartment;

public sealed record ActivateDepartmentCommand(int Id) : IRequest<Result<bool>>;

