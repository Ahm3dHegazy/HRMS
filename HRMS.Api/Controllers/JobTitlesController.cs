using CreateJobTitleRequest = HRMS.Api.Requests.JobTitle.CreateJobTitleRequest;

namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR")]
public sealed class JobTitlesController(ISender sender) : ApiControllerBase
{
    [HttpPost(Router.JobTitleRoutes.CreateJobTitle)]
    [EndpointName("CreateJobTitle")]
    [EndpointSummary("Create a job title")]
    public async Task<IActionResult> Create(CreateJobTitleRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateJobTitleCommand(request.Name, request.Description);
        var jobTitle = await sender.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponseFactory.Success(jobTitle, "Job title created successfully"));
    }

    [HttpPut(Router.JobTitleRoutes.UpdateJobTitle)]
    [EndpointName("UpdateJobTitle")]
    [EndpointSummary("Update a job title")]
    public async Task<IActionResult> Update(int id, UpdateJobTitleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateJobTitleCommand(id, request.Name, request.Description);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Job title updated successfully");
    }

    [HttpDelete(Router.JobTitleRoutes.DeleteJobTitle)]
    [EndpointName("DeleteJobTitle")]
    [EndpointSummary("Delete a job title after all assigned employees are terminated")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteJobTitleCommand(id), cancellationToken);
        return HandleResult(result, "Job title deleted successfully");
    }

    [HttpGet(Router.JobTitleRoutes.GetAllJobTitles)]
    [EndpointName("GetAllJobTitles")]
    [EndpointSummary("Get all job titles")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var jobTitles = await sender.Send(new GetAllJobTitlesQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(jobTitles, "Job titles retrieved successfully"));
    }

    [HttpGet(Router.JobTitleRoutes.GetJobTitleById)]
    [EndpointName("GetJobTitleById")]
    [EndpointSummary("Get a job title by ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetJobTitleByIdQuery(id), cancellationToken);
        return HandleResult(result, "Job title retrieved successfully");
    }

    [HttpGet(Router.JobTitleRoutes.GetEmployeesByJobTitle)]
    [EndpointName("GetEmployeesByJobTitle")]
    [EndpointSummary("Get all employees assigned to a job title")]
    public async Task<IActionResult> GetEmployees(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEmployeesByJobTitleQuery(id), cancellationToken);
        return HandleResult(result, "Employees retrieved successfully");
    }
}
