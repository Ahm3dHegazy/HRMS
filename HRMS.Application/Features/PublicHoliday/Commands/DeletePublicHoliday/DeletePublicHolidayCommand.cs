namespace HRMS.Application.Features.PublicHoliday.Commands.DeletePublicHoliday;

public sealed record DeletePublicHolidayCommand(int Id) : IRequest<Result<bool>>;

