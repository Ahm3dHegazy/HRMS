namespace HRMS.Application.Features.Department.Commands.AddDepartment;

public class AddDepartmentCommand : IRequest<UpdatedDepartmentDto>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int? ManagerId { get; set; }
}

