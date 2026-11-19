namespace HRMS.Api.ApiMapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<AddEmployeeRequest, AddEmployeeCommand>();
        CreateMap<UpdateEmployeeRequest, UpdateEmployeeCommand>();
        CreateMap<PatchEmployeeRequest, PatchEmployeeCommand>();
        CreateMap<TerminateEmployeeRequest, TerminateEmployeeCommand>();
    }
}
