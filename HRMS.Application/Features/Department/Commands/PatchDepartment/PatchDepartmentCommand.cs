using HRMS.Application.Mapping;

namespace HRMS.Application.Features.Department.Commands.PatchDepartment;

public class PatchDepartmentCommand : IRequest<Result<PatchedDepartmentDto>>
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int? ManagerId { get; set; }
}

