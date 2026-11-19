namespace HRMS.Application.Features.PublicHoliday.Commands.DeletePublicHoliday;

public sealed class DeletePublicHolidayHandler(IPublicHolidayRepository repository)
    : IRequestHandler<DeletePublicHolidayCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePublicHolidayCommand command, CancellationToken cancellationToken)
    {
        var holiday = await repository.GetByIdAsync(command.Id);
        if (holiday is null)
            return Result<bool>.Failure(Error.NotFound($"Public holiday with ID {command.Id} was not found."));

        repository.Delete(holiday);
        await repository.SaveChangesAsync();
        return Result<bool>.Success(true);
    }
}

