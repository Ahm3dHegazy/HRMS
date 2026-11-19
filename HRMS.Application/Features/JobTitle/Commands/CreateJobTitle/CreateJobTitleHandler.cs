namespace HRMS.Application.Features.JobTitle.Commands.CreateJobTitle;

public sealed class CreateJobTitleHandler(IJobTitleRepository repository)
    : IRequestHandler<CreateJobTitleCommand, JobTitleDto>
{
    public async Task<JobTitleDto> Handle(CreateJobTitleCommand command, CancellationToken cancellationToken)
    {
        var jobTitle = new Domain.Entities.JobTitle();
        jobTitle.UpdateDetails(command.Name, command.Description);

        await repository.AddAsync(jobTitle);
        await repository.SaveChangesAsync();

        return new JobTitleDto(jobTitle.Id, jobTitle.Name, jobTitle.Description);
    }
}

