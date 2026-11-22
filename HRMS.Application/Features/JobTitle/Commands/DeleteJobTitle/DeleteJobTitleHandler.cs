namespace HRMS.Application.Features.JobTitle.Commands.DeleteJobTitle;

public sealed class DeleteJobTitleHandler(IJobTitleRepository jobTitles)
    : IRequestHandler<DeleteJobTitleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteJobTitleCommand command, CancellationToken cancellationToken)
    {
        var jobTitle = await jobTitles.GetByIdAsync(command.Id);
        if (jobTitle is null)
            return Result<bool>.Failure(Error.NotFound("Job title not found"));

        var hasEmployeesNotTerminated = await jobTitles.HasEmployeesNotTerminatedAsync(command.Id, cancellationToken);
        
        if (!jobTitle.CanBeDeleted(hasEmployeesNotTerminated))
            return Result<bool>.Failure(Error.Conflict(
                "All employees assigned to this job title must be terminated before it can be deleted."));

        jobTitles.Delete(jobTitle);
        await jobTitles.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}

