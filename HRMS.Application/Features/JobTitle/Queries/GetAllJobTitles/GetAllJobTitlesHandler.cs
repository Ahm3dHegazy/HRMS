namespace HRMS.Application.Features.JobTitle.Queries.GetAllJobTitles;

public sealed class GetAllJobTitlesHandler(IJobTitleRepository repository)
    : IRequestHandler<GetAllJobTitlesQuery, IReadOnlyList<JobTitleDto>>
{
    public Task<IReadOnlyList<JobTitleDto>> Handle(GetAllJobTitlesQuery query,
        CancellationToken cancellationToken)
    {
        return repository.GetAllAsync(cancellationToken);
    }
}

