namespace HRMS.Infrastructure.Identity;

public sealed class IdentitySeeder(
    IConfiguration configuration,
    RoleManager<IdentityRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ILogger<IdentitySeeder> logger)
{
    private static readonly string[] RoleNames = ["Admin", "HR", "Manager", "Employee"];

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var email = configuration["AdminUser:Email"]?.Trim();
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Configure AdminUser:Email using user secrets or environment variables.");

        var adminUser = await userManager.FindByEmailAsync(email);
        cancellationToken.ThrowIfCancellationRequested();

        var password = configuration["AdminUser:Password"];
        if (adminUser is null && string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException(
                "Configure AdminUser:Password using user secrets or environment variables.");

        foreach (var roleName in RoleNames)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await roleManager.RoleExistsAsync(roleName))
                continue;

            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            EnsureSucceeded(roleResult, $"Could not create the '{roleName}' role.");
            logger.LogInformation("Created Identity role {RoleName}.", roleName);
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, password!);
            EnsureSucceeded(createResult, "Could not create the configured default Admin user.");
            logger.LogInformation("Created the configured default Admin user.");
        }

        if (!adminUser.EmailConfirmed)
        {
            adminUser.EmailConfirmed = true;
            var confirmationResult = await userManager.UpdateAsync(adminUser);
            EnsureSucceeded(confirmationResult, "Could not confirm the configured default Admin user's email.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var assignRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
            EnsureSucceeded(assignRoleResult, "Could not assign the Admin role to the configured default user.");
            logger.LogInformation("Assigned the Admin role to the configured default user.");
        }
    }

    private static void EnsureSucceeded(IdentityResult result, string message)
    {
        if (result.Succeeded)
            return;

        var errors = string.Join("; ", result.Errors.Select(error => error.Description));
        throw new InvalidOperationException($"{message} {errors}");
    }
}
