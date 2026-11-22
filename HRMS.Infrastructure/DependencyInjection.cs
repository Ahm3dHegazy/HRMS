using HRMS.Infrastructure.Identity;
using HRMS.Application.Common.Attendance;
using HRMS.Infrastructure.Attendance;

namespace HRMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<AttendanceSettings>(configuration.GetSection(AttendanceSettings.SectionName));
        services.Configure<HRMS.Application.Common.Payroll.PayrollSettings>(configuration.GetSection(HRMS.Application.Common.Payroll.PayrollSettings.SectionName));
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddSingleton<IAttendanceClock, AttendanceClock>();
        services.AddHttpContextAccessor();

        services.AddScoped<ReferenceDataCacheInvalidationInterceptor>();
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.AddInterceptors(serviceProvider.GetRequiredService<ReferenceDataCacheInvalidationInterceptor>());
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccessTokenRevocationStore, AccessTokenRevocationStore>();
        services.AddScoped<IEmailConfirmationSender, SmtpEmailConfirmationSender>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IdentitySeeder>();

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IJobTitleRepository, JobTitleRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<IPublicHolidayRepository, PublicHolidayRepository>();
        services.AddScoped<IAttendanceRecordRepository, AttendanceRecordRepository>();
        services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
        services.AddScoped<ILeaveBalanceRepository, LeaveBalanceRepository>();
        services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
        services.AddScoped<IPayrollRepository, PayrollRepository>();

        return services;
    }
}
