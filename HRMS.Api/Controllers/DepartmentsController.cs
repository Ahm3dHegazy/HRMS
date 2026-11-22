namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR")]
public class DepartmentsController(ISender sender, IMapper mapper) : ApiControllerBase
{
    [HttpGet(Router.DepartmentRoutes.GetAllDepartments)]
    [EndpointName("GetAllDepartments")]
    [EndpointSummary("Get all departments")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var departments = await sender.Send(new GetAllDepartmentsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(departments, "Departments retrieved successfully"));
    }

    [HttpGet(Router.DepartmentRoutes.GetPagedDepartments)]
    [EndpointName("GetPagedDepartments")]
    [EndpointSummary("Get departments with pagination")]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
    {
        var departments = await sender.Send(
            new GetPagedDepartmentsQuery(pagination.PageNumber, pagination.PageSize), cancellationToken);
        
        return Ok(ApiResponseFactory.Success(departments, "Departments retrieved successfully"));
    }

    [HttpGet(Router.DepartmentRoutes.GetDepartmentById)]
    [EndpointName("GetDepartmentById")]
    [EndpointSummary("Get a department by ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return HandleResult(result, "Department retrieved successfully");
    }

    [HttpPost(Router.DepartmentRoutes.CreateDepartment)]
    [EndpointName("CreateDepartment")]
    [EndpointSummary("Create a department")]
    public async Task<IActionResult> Post(AddDepartmentRequest request)
    {
        var response = await sender.Send(mapper.Map<AddDepartmentCommand>(request));
        return Ok(ApiResponseFactory.Success(response, "Department added successfully"));
    }

    [HttpPatch(Router.DepartmentRoutes.PatchDepartment)]
    [EndpointName("PatchDepartment")]
    [EndpointSummary("Update department details")]
    public async Task<IActionResult> Patch(int id, PatchDepartmentRequest request)
    {
        var command = new PatchDepartmentCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            ManagerId = request.ManagerId
        };

        var result = await sender.Send(command);
        return HandleResult(result, "Department updated successfully");
    }

    [HttpPost(Router.DepartmentRoutes.DeactivateDepartment)]
    [EndpointName("DeactivateDepartment")]
    [EndpointSummary("Deactivate a department")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeactivateDepartmentCommand(id), cancellationToken);
        if (result.IsFailure)
            return HandleResult(result, "Department deactivated successfully");

        return Ok(ApiResponseFactory.Success<object?>(null, "Department deactivated successfully"));
    }

    [HttpPost(Router.DepartmentRoutes.ActivateDepartment)]
    [EndpointName("ActivateDepartment")]
    [EndpointSummary("Activate a department")]
    public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ActivateDepartmentCommand(id), cancellationToken);
        if (result.IsFailure)
            return HandleResult(result, "Department activated successfully");

        return Ok(ApiResponseFactory.Success<object?>(null, "Department activated successfully"));
    }
}
