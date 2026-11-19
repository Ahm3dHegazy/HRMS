namespace HRMS.Application.Features.JobTitle.Commands.DeleteJobTitle;

public sealed record DeleteJobTitleCommand(int Id) : IRequest<Result<bool>>;

