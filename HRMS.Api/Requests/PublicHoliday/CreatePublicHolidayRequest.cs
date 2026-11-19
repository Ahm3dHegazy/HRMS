namespace HRMS.Api.Requests.PublicHoliday;

public sealed class CreatePublicHolidayRequest
{
    public string Name { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
}
