namespace HRMS.Application.Features.JobTitle.Commands.CreateJobTitle;

public sealed record CreateJobTitleCommand(string Name, string? Description) : IRequest<JobTitleDto>;

