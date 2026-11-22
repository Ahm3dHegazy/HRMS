namespace HRMS.Application.Features.JobTitle.Queries.GetAllJobTitles;

public sealed record GetAllJobTitlesQuery : IRequest<IReadOnlyList<JobTitleDto>>;

