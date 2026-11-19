namespace HRMS.Application.Features.JobTitle.Queries.GetJobTitleById;

public sealed class GetJobTitleByIdHandler(IJobTitleRepository repository)
    : IRequestHandler<GetJobTitleByIdQuery, Result<JobTitleDto>>
{
    public async Task<Result<JobTitleDto>> Handle(
        GetJobTitleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var jobTitle = await repository.GetByIdAsync(query.Id, cancellationToken);

        return jobTitle is null
            ? Result<JobTitleDto>.Failure(Error.NotFound("Job title not found"))
            : Result<JobTitleDto>.Success(jobTitle);
    }
}

