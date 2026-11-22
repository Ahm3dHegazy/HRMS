namespace HRMS.Application.Authentication;

public interface IAuthService
{
    /// <summary>Registers a user and returns the new user's ID.</summary>
    Task<Result<string>> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default);

    /// <summary>Confirms ownership of a user's email address.</summary>
    Task<Result<string>> ConfirmEmailAsync(string userId, string token,
        CancellationToken cancellationToken = default);

    /// <summary>Authenticates a user and returns an access token.</summary>
    Task<Result<string>> LoginAsync(LoginDto request, CancellationToken cancellationToken = default);
}
