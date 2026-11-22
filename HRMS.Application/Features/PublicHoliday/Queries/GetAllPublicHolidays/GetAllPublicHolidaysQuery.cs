namespace HRMS.Application.Features.PublicHoliday.Queries.GetAllPublicHolidays;

public sealed record GetAllPublicHolidaysQuery : IRequest<IReadOnlyList<PublicHolidayDto>>;

