namespace HRMS.Application.Features.JobTitle.Commands.UpdateJobTitle;

public sealed record UpdateJobTitleCommand(int Id, string Name, string? Description)
    : IRequest<Result<JobTitleDto>>;

