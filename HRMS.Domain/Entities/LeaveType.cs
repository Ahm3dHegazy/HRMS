namespace HRMS.Domain.Entities;

public class LeaveType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DefaultDaysPerYear { get; set; }
    public bool IsPaid { get; set; }
    public bool RequiresApproval { get; set; }

    // Relationships

    // LeaveRequest
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new HashSet<LeaveRequest>();

    // LeaveBalance
    public ICollection<LeaveBalance> LeaveBalances { get; set; } = new HashSet<LeaveBalance>();
}
