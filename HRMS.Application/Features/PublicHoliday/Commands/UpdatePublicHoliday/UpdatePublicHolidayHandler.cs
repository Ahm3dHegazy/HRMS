namespace HRMS.Application.Features.PublicHoliday.Commands.UpdatePublicHoliday;

public sealed class UpdatePublicHolidayHandler(IPublicHolidayRepository repository)
    : IRequestHandler<UpdatePublicHolidayCommand, Result<PublicHolidayDto>>
{
    public async Task<Result<PublicHolidayDto>> Handle(UpdatePublicHolidayCommand command, CancellationToken cancellationToken)
    {
        var holiday = await repository.GetByIdAsync(command.Id);
        if (holiday is null)
            return Result<PublicHolidayDto>.Failure(Error.NotFound($"Public holiday with ID {command.Id} was not found."));
        
        if (await repository.ExistsOnDateAsync(command.Date, command.Id, cancellationToken))
            return Result<PublicHolidayDto>.Failure(Error.Conflict($"A public holiday already exists on {command.Date}."));

        holiday.Update(command.Name, command.Date);
        
        repository.Update(holiday);
        await repository.SaveChangesAsync();
        
        return Result<PublicHolidayDto>.Success(new PublicHolidayDto(holiday.Id, holiday.Name, holiday.Date));
    }
}

