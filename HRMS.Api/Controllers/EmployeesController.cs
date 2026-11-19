namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR")]
public class EmployeesController(ISender sender, IMapper mapper) : ApiControllerBase
{
    [HttpGet(Router.EmployeeRoutes.GetAllEmployees)]
    [EndpointName("GetAllEmployees")]
    [EndpointSummary("Get all employees")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var employees = await sender.Send(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(employees, "Employees retrieved successfully"));
    }

    [HttpGet(Router.EmployeeRoutes.GetEmployeeById)]
    [EndpointName("GetEmployeeById")]
    [EndpointSummary("Get an employee by ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEmployeeByIdQuery(id), cancellationToken);
        return HandleResult(result, "Employee retrieved successfully");
    }

    [HttpPost(Router.EmployeeRoutes.CreateEmployee)]
    [EndpointName("CreateEmployee")]
    [EndpointSummary("Create an employee")]
    public async Task<IActionResult> Post(AddEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<AddEmployeeCommand>(request);

        await sender.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created,
            ApiResponseFactory.Success("Employee Added", "Employee added successfully"));
    }

    [HttpPut(Router.EmployeeRoutes.UpdateEmployee)]
    [EndpointName("ReplaceEmployee")]
    [EndpointSummary("Replace employee details")]
    public async Task<IActionResult> Put(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateEmployeeCommand>(request);
        command.EmployeeId = id;

        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Employee updated successfully");
    }

    [HttpPatch(Router.EmployeeRoutes.PatchEmployee)]
    [EndpointName("PatchEmployee")]
    [EndpointSummary("Update selected employee details")]
    public async Task<IActionResult> Patch(int id, PatchEmployeeRequest request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<PatchEmployeeCommand>(request);
        command.EmployeeId = id;

        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Employee updated successfully");
    }

    [HttpPost(Router.EmployeeRoutes.TerminateEmployee)]
    [EndpointName("TerminateEmployee")]
    [EndpointSummary("Terminate an employee")]
    public async Task<IActionResult> Terminate(int id, TerminateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<TerminateEmployeeCommand>(request);
        command.EmployeeId = id;

        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Employee terminated successfully");
    }

    [HttpGet(Router.EmployeeRoutes.GetPagedEmployees)]
    [EndpointName("GetPagedEmployees")]
    [EndpointSummary("Get employees with pagination and filters")]
    public async Task<IActionResult> GetPaged([FromQuery] EmployeeFilter filter, [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        var query = new GetPagedEmployeesQuery
        {
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            EmployeeFilter = filter
        };

        var pagedEmployees = await sender.Send(query, cancellationToken);

        return Ok(ApiResponseFactory.Success(pagedEmployees, "Employees retrieved successfully"));
    }
}
