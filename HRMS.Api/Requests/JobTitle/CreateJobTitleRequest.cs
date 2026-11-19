namespace HRMS.Api.Requests.JobTitle;

public sealed class CreateJobTitleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
