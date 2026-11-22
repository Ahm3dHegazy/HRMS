namespace HRMS.Application.Features.AttendanceRecord.Commands.CheckOut;

public sealed record CheckOutCommand : IRequest<Result<AttendanceRecordDto>>;

