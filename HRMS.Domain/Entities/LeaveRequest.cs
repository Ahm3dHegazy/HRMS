namespace HRMS.Domain.Entities;

public class LeaveRequest
{
    public int Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }

    // Relationships

    // Employee (the requester)
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    // Employee (the reviewer)
    public int? ReviewedById { get; set; }
    public Employee? ReviewedBy { get; set; }


    // LeaveType
    public int LeaveTypeId { get; set; }
    public LeaveType LeaveType { get; set; } = null!;

    public IReadOnlyDictionary<int, int> GetRequestedDaysByYear()
    {
        var daysByYear = new SortedDictionary<int, int>();
        if (EndDate < StartDate)
            return daysByYear;

        for (var year = StartDate.Year; year <= EndDate.Year; year++)
        {
            var segmentStart = StartDate > new DateOnly(year, 1, 1) ? StartDate : new DateOnly(year, 1, 1);
            var segmentEnd = EndDate < new DateOnly(year, 12, 31) ? EndDate : new DateOnly(year, 12, 31);
            daysByYear[year] = segmentEnd.DayNumber - segmentStart.DayNumber + 1;
        }

        return daysByYear;
    }

    public void Submit()
    {
        Status = LeaveStatus.Pending;
        ReviewedAt = null;
        ReviewedById = null;
        RejectionReason = null;
    }

    public bool Approve(DateTime reviewedAt)
    {
        if (Status != LeaveStatus.Pending)
            return false;

        Status = LeaveStatus.Approved;
        ReviewedAt = reviewedAt;
        RejectionReason = null;
        return true;
    }

    public bool Reject(string rejectionReason, DateTime reviewedAt)
    {
        if (Status != LeaveStatus.Pending)
            return false;

        Status = LeaveStatus.Rejected;
        RejectionReason = rejectionReason.Trim();
        ReviewedAt = reviewedAt;
        return true;
    }

    public bool Cancel()
    {
        if (Status != LeaveStatus.Pending)
            return false;

        Status = LeaveStatus.Cancelled;
        return true;
    }
}
