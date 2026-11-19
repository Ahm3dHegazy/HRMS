namespace HRMS.Application.Features.PublicHoliday.Queries.GetPublicHolidayById;

public sealed class GetPublicHolidayByIdHandler(IPublicHolidayRepository repository)
    : IRequestHandler<GetPublicHolidayByIdQuery, Result<PublicHolidayDto>>
{
    public async Task<Result<PublicHolidayDto>> Handle(GetPublicHolidayByIdQuery query, CancellationToken cancellationToken)
    {
        var holiday = await repository.GetByIdAsync(query.Id, cancellationToken);
        return holiday is null
            ? Result<PublicHolidayDto>.Failure(Error.NotFound($"Public holiday with ID {query.Id} was not found."))
            : Result<PublicHolidayDto>.Success(holiday);
    }
}

