namespace HRMS.Application.Features.JobTitle.Queries.GetJobTitleById;

public sealed record GetJobTitleByIdQuery(int Id) : IRequest<Result<JobTitleDto>>;

