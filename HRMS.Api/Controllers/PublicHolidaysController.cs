using HRMS.Api.Requests.PublicHoliday;
using HRMS.Application.Features.PublicHoliday.Commands.CreatePublicHoliday;
using HRMS.Application.Features.PublicHoliday.Commands.DeletePublicHoliday;
using HRMS.Application.Features.PublicHoliday.Commands.UpdatePublicHoliday;
using HRMS.Application.Features.PublicHoliday.Queries.GetAllPublicHolidays;
using HRMS.Application.Features.PublicHoliday.Queries.GetPublicHolidayById;

namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR,Manager,Employee")]
public sealed class PublicHolidaysController(ISender sender) : ApiControllerBase
{
    [HttpGet(Router.PublicHolidayRoutes.GetAllPublicHolidays)]
    [EndpointName("GetAllPublicHolidays")]
    [EndpointSummary("Get all public holidays")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var holidays = await sender.Send(new GetAllPublicHolidaysQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(holidays, "Public holidays retrieved successfully"));
    }

    [HttpGet(Router.PublicHolidayRoutes.GetPublicHolidayById)]
    [EndpointName("GetPublicHolidayById")]
    [EndpointSummary("Get a public holiday by ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPublicHolidayByIdQuery(id), cancellationToken);
        return HandleResult(result, "Public holiday retrieved successfully");
    }

    [HttpPost(Router.PublicHolidayRoutes.CreatePublicHoliday)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("CreatePublicHoliday")]
    [EndpointSummary("Create a public holiday")]
    public async Task<IActionResult> Create(CreatePublicHolidayRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreatePublicHolidayCommand(request.Name, request.Date), cancellationToken);
        return HandleResult(result, "Public holiday created successfully", StatusCodes.Status201Created);
    }

    [HttpPut(Router.PublicHolidayRoutes.UpdatePublicHoliday)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("UpdatePublicHoliday")]
    [EndpointSummary("Update a public holiday")]
    public async Task<IActionResult> Update(int id, UpdatePublicHolidayRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdatePublicHolidayCommand(id, request.Name, request.Date), cancellationToken);
        return HandleResult(result, "Public holiday updated successfully");
    }

    [HttpDelete(Router.PublicHolidayRoutes.DeletePublicHoliday)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("DeletePublicHoliday")]
    [EndpointSummary("Delete a public holiday")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeletePublicHolidayCommand(id), cancellationToken);
        return HandleResult(result, "Public holiday deleted successfully");
    }
}
