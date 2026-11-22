namespace HRMS.Api.ApiMapping;

public sealed class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<CreateLeaveRequest, CreateLeaveRequestCommand>();
    }
}
