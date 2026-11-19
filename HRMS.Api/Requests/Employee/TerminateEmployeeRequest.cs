using System.Text.Json.Serialization;

namespace HRMS.Api.Requests.Employee;

public class TerminateEmployeeRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TerminationType TerminationType { get; set; }

    public string Reason { get; set; } = string.Empty;
    public DateOnly TerminationDate { get; set; }
}
