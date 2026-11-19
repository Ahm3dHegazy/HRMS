namespace HRMS.Application.Features.JobTitle.Queries.GetEmployeesByJobTitle;

public sealed record GetEmployeesByJobTitleQuery(int JobTitleId)
    : IRequest<Result<IReadOnlyList<EmployeeDetailsDto>>>;

