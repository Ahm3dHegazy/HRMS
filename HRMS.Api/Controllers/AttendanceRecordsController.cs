using HRMS.Api.Requests.Attendance;
using HRMS.Application.Features.AttendanceRecord.Commands.CreateAttendanceRecord;
using HRMS.Application.Features.AttendanceRecord.Commands.CheckIn;
using HRMS.Application.Features.AttendanceRecord.Commands.CheckOut;
using HRMS.Application.Features.AttendanceRecord.Commands.DeleteAttendanceRecord;
using HRMS.Application.Features.AttendanceRecord.Commands.UpdateAttendanceRecord;
using HRMS.Application.Features.AttendanceRecord.Queries.GetAllAttendanceRecords;
using HRMS.Application.Features.AttendanceRecord.Queries.GetAttendanceRecordById;
using HRMS.Application.Features.AttendanceRecord.Queries.GetEmployeeAttendanceRecords;
using HRMS.Application.Features.AttendanceRecord.Queries.GetMonthlyEmployeeAttendanceRecords;

namespace HRMS.Api.Controllers;

[Authorize(Roles = "Admin,HR,Manager,Employee")]
public sealed class AttendanceRecordsController(ISender sender) : ApiControllerBase
{
    [HttpGet(Router.AttendanceRecordRoutes.GetAllAttendanceRecords)]
    [EndpointName("GetAllAttendanceRecords")]
    [EndpointSummary("Get all attendance records")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var records = await sender.Send(new GetAllAttendanceRecordsQuery(), cancellationToken);
        return Ok(ApiResponseFactory.Success(records, "Attendance records retrieved successfully"));
    }

    [HttpGet(Router.AttendanceRecordRoutes.GetAttendanceRecordById)]
    [EndpointName("GetAttendanceRecordById")]
    [EndpointSummary("Get an attendance record by ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetAttendanceRecordByIdQuery(id), cancellationToken);
        return HandleResult(result, "Attendance record retrieved successfully");
    }

    [HttpGet(Router.AttendanceRecordRoutes.GetEmployeeAttendanceRecords)]
    [EndpointName("GetEmployeeAttendanceRecords")]
    [EndpointSummary("Get attendance records for an employee")]
    public async Task<IActionResult> GetByEmployee(int employeeId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetEmployeeAttendanceRecordsQuery(employeeId), cancellationToken);
        return HandleResult(result, "Employee attendance records retrieved successfully");
    }

    [HttpGet(Router.AttendanceRecordRoutes.GetMonthlyEmployeeAttendanceRecords)]
    [EndpointName("GetMonthlyEmployeeAttendanceRecords")]
    [EndpointSummary("Get an employee's attendance records for a month")]
    public async Task<IActionResult> GetMonthlyByEmployee(int employeeId, int year, int month,
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyEmployeeAttendanceRecordsQuery(employeeId, year, month);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result, "Monthly employee attendance records retrieved successfully");
    }

    [HttpPost(Router.AttendanceRecordRoutes.CreateAttendanceRecord)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("CreateAttendanceRecord")]
    [EndpointSummary("Create an attendance record")]
    public async Task<IActionResult> Create(AttendanceRecordRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAttendanceRecordCommand(request.EmployeeId, request.Date,
            request.CheckIn, request.CheckOut, request.Status);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Attendance record created successfully", StatusCodes.Status201Created);
    }

    [HttpPost(Router.AttendanceRecordRoutes.CheckInAttendance)]
    [EndpointName("AttendanceCheckIn")]
    [EndpointSummary("Record an employee check-in")]
    public async Task<IActionResult> CheckIn(AttendancePunchRequest request, CancellationToken cancellationToken)
    {
        var command = new CheckInCommand(request.EmployeeId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Check-in recorded successfully");
    }

    [HttpPost(Router.AttendanceRecordRoutes.CheckOutAttendance)]
    [EndpointName("AttendanceCheckOut")]
    [EndpointSummary("Record an employee check-out")]
    public async Task<IActionResult> CheckOut(AttendancePunchRequest request, CancellationToken cancellationToken)
    {
        var command = new CheckOutCommand(request.EmployeeId);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Check-out recorded successfully");
    }

    [HttpPut(Router.AttendanceRecordRoutes.UpdateAttendanceRecord)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("UpdateAttendanceRecord")]
    [EndpointSummary("Update an attendance record")]
    public async Task<IActionResult> Update(int id, AttendanceRecordRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateAttendanceRecordCommand(id, request.EmployeeId, request.Date,
            request.CheckIn, request.CheckOut, request.Status);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, "Attendance record updated successfully");
    }

    [HttpDelete(Router.AttendanceRecordRoutes.DeleteAttendanceRecord)]
    [Authorize(Roles = "Admin,HR")]
    [EndpointName("DeleteAttendanceRecord")]
    [EndpointSummary("Delete an attendance record")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteAttendanceRecordCommand(id), cancellationToken);
        return HandleResult(result, "Attendance record deleted successfully");
    }
}
