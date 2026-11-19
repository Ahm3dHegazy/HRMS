namespace HRMS.Application.Features.PublicHoliday.Commands.UpdatePublicHoliday;

public sealed record UpdatePublicHolidayCommand(int Id, string Name, DateOnly Date) : IRequest<Result<PublicHolidayDto>>;

