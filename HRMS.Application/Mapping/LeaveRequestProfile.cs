namespace HRMS.Application.Mapping;

public sealed class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<CreateLeaveRequestCommand, LeaveRequest>()
            .ForMember(leaveRequest => leaveRequest.Reason,
                options => options.MapFrom(command => command.Reason.Trim()));
    }
}
