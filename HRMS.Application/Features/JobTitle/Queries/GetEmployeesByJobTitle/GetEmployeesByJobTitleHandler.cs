namespace HRMS.Application.Features.JobTitle.Queries.GetEmployeesByJobTitle;

public sealed class GetEmployeesByJobTitleHandler(IJobTitleRepository jobTitles, IEmployeeRepository employees)
    : IRequestHandler<GetEmployeesByJobTitleQuery, Result<IReadOnlyList<EmployeeDetailsDto>>>
{
    public async Task<Result<IReadOnlyList<EmployeeDetailsDto>>> Handle(GetEmployeesByJobTitleQuery query,
        CancellationToken cancellationToken)
    {
        var jobTitle = await jobTitles.GetByIdAsync(query.JobTitleId, cancellationToken);
        if (jobTitle is null)
            return Result<IReadOnlyList<EmployeeDetailsDto>>.Failure(Error.NotFound("Job title not found"));

        var result = await employees.GetByJobTitleIdAsync(query.JobTitleId, cancellationToken);
        
        return Result<IReadOnlyList<EmployeeDetailsDto>>.Success(result);
    }
}

