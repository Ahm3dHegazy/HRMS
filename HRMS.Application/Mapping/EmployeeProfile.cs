namespace HRMS.Application.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<AddEmployeeCommand, Employee>();
        CreateMap<Employee, TerminatedEmployeeDto>()
            .ForMember(destination => destination.Name,
                options => options.MapFrom(source => source.FullName))
            .ForMember(destination => destination.Reason,
                options => options.MapFrom(source => source.TerminationReason))
            .ForMember(destination => destination.TerminationType,
                options => options.MapFrom(source => source.TerminationType.ToString()));

        CreateMap<UpdateEmployeeCommand, Employee>()
            .ForMember(destination => destination.Id, options => options.Ignore());
        CreateMap<PatchEmployeeCommand, Employee>()
            .ForMember(destination => destination.Id, options => options.Ignore())
            .ForAllMembers(options =>
                options.Condition((source, destination, sourceMember) => sourceMember is not null));
        CreateMap<Employee, EmployeeDetailsDto>()
            .ForMember(destination => destination.StatusName,
                options => options.MapFrom(source => source.Status.ToString()))
            .ForMember(destination => destination.DepartmentName,
                options => options.MapFrom(source => source.Department.Name))
            .ForMember(destination => destination.JobName,
                options => options.MapFrom(source => source.JobTitle.Name));
    }
}
