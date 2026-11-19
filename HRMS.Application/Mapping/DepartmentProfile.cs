using HRMS.Application.Features.Department.Commands.PatchDepartment;

namespace HRMS.Application.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<AddDepartmentCommand, Department>();
        CreateMap<Department, UpdatedDepartmentDto>()
            .ForMember(dto => dto.ManagerName,
                options => options.MapFrom(department => department.Manager == null
                    ? null
                    : department.Manager.FullName));

        CreateMap<PatchDepartmentCommand, Department>()
            .ForAllMembers(options => options.Condition((_, _, sourceMember) => sourceMember is not null));

        CreateMap<Department, PatchedDepartmentDto>()
            .ForMember(dto => dto.ManagerName,
                options => options.MapFrom(department => department.Manager == null
                    ? null
                    : department.Manager.FullName));
    }
}
