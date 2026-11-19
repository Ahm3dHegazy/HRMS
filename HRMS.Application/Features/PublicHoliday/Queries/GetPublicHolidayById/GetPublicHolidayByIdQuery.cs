namespace HRMS.Application.Features.PublicHoliday.Queries.GetPublicHolidayById;

public sealed record GetPublicHolidayByIdQuery(int Id) : IRequest<Result<PublicHolidayDto>>;

