namespace HRMS.Application.Features.PublicHoliday.Queries.GetAllPublicHolidays;

public sealed class GetAllPublicHolidaysHandler(IPublicHolidayRepository repository)
    : IRequestHandler<GetAllPublicHolidaysQuery, IReadOnlyList<PublicHolidayDto>>
{
    public Task<IReadOnlyList<PublicHolidayDto>> Handle(GetAllPublicHolidaysQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAllAsync(cancellationToken);
    }
}

