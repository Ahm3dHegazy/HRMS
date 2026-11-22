namespace HRMS.Infrastructure.Identity;

public sealed class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
    AppDbContext dbContext,
    IJwtTokenService jwtTokenService,
    IEmailConfirmationSender emailConfirmationSender,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<Result<string>> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            return Result<string>.Failure(Error.Validation("A valid email address is required."));

        var email = request.Email.Trim();
        var normalizedEmail = userManager.NormalizeEmail(email);

        if (await userManager.FindByEmailAsync(email) is not null ||
            await userManager.FindByNameAsync(request.UserName) is not null)
        {
            return Result<string>.Failure(Error.Conflict("A user with this email or username already exists."));
        }

        var employee = await dbContext.Employees
            .SingleOrDefaultAsync(employee => employee.Email.Trim().ToUpper() == normalizedEmail,
                cancellationToken);

        if (employee is null)
        {
            return Result<string>.Failure(Error.Forbidden(
                "Registration is only available to employees with a matching HRMS employee record."));
        }

        if (employee.Status != EmploymentStatus.Active)
        {
            return Result<string>.Failure(Error.Forbidden(
                "Only active employees can register an account."));
        }

        if (await userManager.Users.AnyAsync(user => user.EmployeeId == employee.Id, cancellationToken))
        {
            return Result<string>.Failure(Error.Conflict("This employee already has an account."));
        }

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = email,
            EmployeeId = employee.Id
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
            return IdentityFailure(createResult);

        var roleResult = await userManager.AddToRoleAsync(user, "Employee");
        if (!roleResult.Succeeded)
        {
            var cleanupResult = await userManager.DeleteAsync(user);
            if (!cleanupResult.Succeeded)
            {
                logger.LogError("Could not remove user {UserId} after Employee role assignment failed.", user.Id);
            }

            return IdentityFailure(roleResult);
        }

        try
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(
                System.Text.Encoding.UTF8.GetBytes(token));
            
            await emailConfirmationSender.SendConfirmationAsync(user.Email!, user.Id, encodedToken,
                cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not send email confirmation to user {UserId}.", user.Id);
            
            var cleanupResult = await userManager.DeleteAsync(user);
            
            if (!cleanupResult.Succeeded)
                logger.LogError("Could not remove user {UserId} after confirmation email delivery failed.", user.Id);

            return Result<string>.Failure(Error.Validation(
                "The confirmation email could not be sent. Please try registering again later."));
        }

        return Result<string>.Success(user.Id);
    }

    public async Task<Result<string>> ConfirmEmailAsync(string userId, string token,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<string>.Failure(Error.NotFound("The confirmation link is invalid or expired."));

        if (user.EmailConfirmed)
            return Result<string>.Success(user.Id);

        try
        {
            var decodedToken = System.Text.Encoding.UTF8.GetString(
                Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(token));
            var confirmationResult = await userManager.ConfirmEmailAsync(user, decodedToken);
            return confirmationResult.Succeeded
                ? Result<string>.Success(user.Id)
                : Result<string>.Failure(Error.Validation("The confirmation link is invalid or expired."));
        }
        catch (FormatException)
        {
            return Result<string>.Failure(Error.Validation("The confirmation link is invalid or expired."));
        }
    }

    public async Task<Result<string>> LoginAsync(LoginDto request, CancellationToken cancellationToken = default)
    {
        
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return InvalidCredentials();

        if (!user.EmailConfirmed)
            return Result<string>.Failure(Error.Forbidden("Please confirm your email before logging in."));

        var passwordResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);

        if (!passwordResult.Succeeded)
            return InvalidCredentials();

        if (user.EmployeeId is { } employeeId)
        {
            var employeeStatus = await dbContext.Employees
                .AsNoTracking()
                .Where(employee => employee.Id == employeeId)
                .Select(employee => (EmploymentStatus?)employee.Status)
                .SingleOrDefaultAsync(cancellationToken);

            if (employeeStatus != EmploymentStatus.Active)
            {
                return Result<string>.Failure(Error.Forbidden(
                    "Inactive or terminated employees cannot log in."));
            }
        }

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = jwtTokenService.GenerateAccessToken(user.Id, user.Email ?? request.Email, roles,
            user.EmployeeId);

        return Result<string>.Success(accessToken);
    }

    private static Result<string> InvalidCredentials()
        => Result<string>.Failure(Error.Unauthorized("Invalid email or password."));

    private static Result<string> IdentityFailure(IdentityResult identityResult)
    {
        var descriptions = identityResult.Errors.Select(error => error.Description).ToArray();
        var message = string.Join(" ", descriptions);
        var isConflict = identityResult.Errors.Any(error =>
            error.Code.StartsWith("Duplicate", StringComparison.OrdinalIgnoreCase));

        return Result<string>.Failure(isConflict
            ? Error.Conflict(message)
            : Error.Validation(message));
    }
}
