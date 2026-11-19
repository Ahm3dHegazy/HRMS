namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckOut;

public sealed record CheckOutCommand(int EmployeeId) : IRequest<Result<AttendanceRecordDto>>;

