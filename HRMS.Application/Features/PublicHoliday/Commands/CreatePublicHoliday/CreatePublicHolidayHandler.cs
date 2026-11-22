namespace HRMS.Application.Features.PublicHoliday.Commands.CreatePublicHoliday;

public sealed class CreatePublicHolidayHandler(IPublicHolidayRepository repository)
    : IRequestHandler<CreatePublicHolidayCommand, Result<PublicHolidayDto>>
{
    public async Task<Result<PublicHolidayDto>> Handle(CreatePublicHolidayCommand command, CancellationToken cancellationToken)
    {
        if (await repository.ExistsOnDateAsync(command.Date, null, cancellationToken))
            return Result<PublicHolidayDto>.Failure(Error.Conflict($"A public holiday already exists on {command.Date}."));

        var holiday = Domain.Entities.PublicHoliday.Create(command.Name, command.Date);
        
        await repository.AddAsync(holiday);
        await repository.SaveChangesAsync();
        
        return Result<PublicHolidayDto>.Success(new PublicHolidayDto(holiday.Id, holiday.Name, holiday.Date));
    }
}

