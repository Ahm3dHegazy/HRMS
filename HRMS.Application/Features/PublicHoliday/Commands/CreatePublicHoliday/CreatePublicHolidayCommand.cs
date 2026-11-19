namespace HRMS.Application.Features.PublicHoliday.Commands.CreatePublicHoliday;

public sealed record CreatePublicHolidayCommand(string Name, DateOnly Date) : IRequest<Result<PublicHolidayDto>>;

