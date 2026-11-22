namespace HRMS.Api.ApiMapping.Department;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<AddDepartmentRequest, AddDepartmentCommand>();
    }
}
