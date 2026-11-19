namespace HRMS.Application.Features.JobTitle.Commands.UpdateJobTitle;

public sealed class UpdateJobTitleHandler(IJobTitleRepository repository)
    : IRequestHandler<UpdateJobTitleCommand, Result<JobTitleDto>>
{
    public async Task<Result<JobTitleDto>> Handle(
        UpdateJobTitleCommand command,
        CancellationToken cancellationToken)
    {
        var jobTitle = await repository.GetByIdAsync(command.Id);
        if (jobTitle is null)
            return Result<JobTitleDto>.Failure(Error.NotFound("Job title not found"));

        jobTitle.UpdateDetails(command.Name, command.Description);
        
        await repository.SaveChangesAsync();

        return Result<JobTitleDto>.Success(
            new JobTitleDto(jobTitle.Id, jobTitle.Name, jobTitle.Description));
    }
}

