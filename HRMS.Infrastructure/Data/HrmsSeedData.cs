namespace HRMS.Infrastructure.Data;

public static class HrmsSeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Employees.AnyAsync()) return;

        var departments = new[]
        {
            new Department { Name = "Human Resources", Description = "People operations and talent", IsActive = true },
            new Department { Name = "Engineering", Description = "Product engineering and platform", IsActive = true },
            new Department { Name = "Finance", Description = "Accounting, planning and treasury", IsActive = true },
            new Department { Name = "Sales", Description = "Business development and sales", IsActive = true },
            new Department { Name = "Marketing", Description = "Brand, communications and growth", IsActive = true },
            new Department
                { Name = "Customer Support", Description = "Customer care and service quality", IsActive = true },
            new Department { Name = "Operations", Description = "Business and office operations", IsActive = true },
            new Department
                { Name = "Information Technology", Description = "Corporate systems and security", IsActive = true },
            new Department { Name = "Legal", Description = "Legal affairs and compliance", IsActive = true },
            new Department
                { Name = "Research and Development", Description = "Research and product innovation", IsActive = true }
        };
        var titles = new[]
        {
            "HR Manager", "Software Engineer", "Financial Analyst", "Sales Executive", "Marketing Specialist",
            "Support Representative", "Operations Coordinator", "Systems Administrator", "Legal Counsel",
            "Product Researcher"
        }.Select(name => new JobTitle { Name = name, Description = $"{name} role" }).ToArray();
        var leaveTypes = new[]
        {
            new LeaveType { Name = "Annual Leave", DefaultDaysPerYear = 21, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Casual Leave", DefaultDaysPerYear = 7, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Sick Leave", DefaultDaysPerYear = 30, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Maternity Leave", DefaultDaysPerYear = 90, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Paternity Leave", DefaultDaysPerYear = 7, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Unpaid Leave", DefaultDaysPerYear = 30, IsPaid = false, RequiresApproval = true },
            new LeaveType
                { Name = "Bereavement Leave", DefaultDaysPerYear = 5, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Study Leave", DefaultDaysPerYear = 5, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Hajj Leave", DefaultDaysPerYear = 21, IsPaid = true, RequiresApproval = true },
            new LeaveType { Name = "Marriage Leave", DefaultDaysPerYear = 5, IsPaid = true, RequiresApproval = true }
        };

        db.Departments.AddRange(departments);
        db.JobTitles.AddRange(titles);
        db.LeaveTypes.AddRange(leaveTypes);
        await db.SaveChangesAsync();

        var names = new[]
        {
            "Mariam Hassan", "Omar Khaled", "Nour El-Sayed", "Youssef Adel", "Salma Ibrahim", "Karim Mostafa",
            "Laila Mahmoud", "Amr Fathy", "Dina Samir", "Hany Nabil"
        };
        var employees = names.Select((name, i) => new Employee
        {
            FullName = name, Address = $"{10 + i} El Tahrir Street, Cairo, Egypt",
            Email =
                $"{new[] { "mariam.hassan", "omar.khaled", "nour.elsayed", "youssef.adel", "salma.ibrahim", "karim.mostafa", "laila.mahmoud", "amr.fathy", "dina.samir", "hany.nabil" }[i]}@example.com",
            Phone = $"+2010{10000000 + i}", NationalId = $"2980101123456{i:00}",
            HireDate = new DateOnly(2020 + i % 5, 1 + i % 12, 5 + i % 20), BaseSalary = 18000 + i * 2500,
            Status = EmploymentStatus.Active, DepartmentId = departments[i].Id, JobTitleId = titles[i].Id,
            ManagerId = null
        }).ToArray();

        db.Employees.AddRange(employees);
        await db.SaveChangesAsync();

        for (var i = 0; i < departments.Length; i++) departments[i].ManagerId = employees[i].Id;
        for (var i = 1; i < employees.Length; i++) employees[i].ManagerId = employees[0].Id;
        db.LeaveBalances.AddRange(employees.SelectMany((employee, i) => leaveTypes.Select((type, j) => new LeaveBalance
        {
            EmployeeId = employee.Id, LeaveTypeId = type.Id, Year = 2026,
            TotalDays = type.DefaultDaysPerYear, UsedDays = (i + j) % 4
        })));

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        db.AttendanceRecords.AddRange(employees.Select((employee, i) => AttendanceRecord.CreateSeed(
            employee.Id, today.AddDays(-1), new TimeOnly(8, 45 + i % 30), new TimeOnly(17, 0),
            i % 5 == 0 ? AttendanceStatus.Late : AttendanceStatus.Present)));
        db.LeaveRequests.AddRange(Enumerable.Range(0, 10).Select(i => new LeaveRequest
        {
            EmployeeId = employees[i].Id, LeaveTypeId = leaveTypes[i % leaveTypes.Length].Id,
            StartDate = today.AddDays(10 + i * 3), EndDate = today.AddDays(11 + i * 3),
            Reason = new[]
            {
                "Family commitment", "Medical appointment", "Personal time", "Professional development", "Family visit"
            }[i % 5],
            Status = i % 3 == 0 ? LeaveStatus.Pending : i % 3 == 1 ? LeaveStatus.Approved : LeaveStatus.Rejected,
            ReviewedById = i % 3 == 0 ? null : employees[0].Id,
            ReviewedAt = i % 3 == 0 ? null : DateTime.UtcNow.AddDays(-i),
            RejectionReason = i % 3 == 2 ? "Please choose dates that meet team coverage requirements." : null
        }));
        db.PublicHolidays.AddRange(new[]
        {
            ("Coptic Christmas", new DateOnly(2026, 1, 7)), ("Revolution Day", new DateOnly(2026, 1, 25)),
            ("Eid al-Fitr", new DateOnly(2026, 3, 20)), ("Sinai Liberation Day", new DateOnly(2026, 4, 25)),
            ("Labour Day", new DateOnly(2026, 5, 1)), ("Arafat Day", new DateOnly(2026, 5, 26)),
            ("Eid al-Adha", new DateOnly(2026, 5, 27)), ("Revolution Day (June)", new DateOnly(2026, 6, 30)),
            ("Revolution Day", new DateOnly(2026, 7, 23)), ("Armed Forces Day", new DateOnly(2026, 10, 6))
        }.Select(holiday => new PublicHoliday { Name = holiday.Item1, Date = holiday.Item2 }));

        await db.SaveChangesAsync();
    }
}
