using HRMS.Domain.Entities;

namespace HRMS.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
