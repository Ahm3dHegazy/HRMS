namespace HRMS.Api.Controllers;

public class AuthController(ISender sender, IAccessTokenRevocationStore tokenRevocations) : ApiControllerBase
{
    [HttpPost(Router.AuthenticationRoutes.RegisterUser)]
    [AllowAnonymous]
    [EndpointName("Register")]
    [EndpointSummary("Register an active HRMS employee account")]
    public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterCommand(request), cancellationToken);
        return HandleResult(result,
            "Registration successful. Check your email to confirm your account before logging in.",
            StatusCodes.Status201Created);
    }

    [HttpPost(Router.AuthenticationRoutes.LoginUser)]
    [AllowAnonymous]
    [EndpointName("Login")]
    [EndpointSummary("Authenticate and receive a JWT access token")]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginCommand(request), cancellationToken);
        return HandleResult(result, "Login successful");
    }

    [HttpPost(Router.AuthenticationRoutes.LogoutUser)]
    [Authorize]
    [EndpointName("Logout")]
    [EndpointSummary("Log out and revoke the current access token")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var tokenId = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;
        var expiresAtValue = User.FindFirst("exp")?.Value;
        if (string.IsNullOrWhiteSpace(tokenId) || !long.TryParse(expiresAtValue, out var expiresAtSeconds))
            return Unauthorized(ApiResponseFactory.Failure<object>("The access token is missing required claims."));

        await tokenRevocations.RevokeAsync(tokenId, DateTimeOffset.FromUnixTimeSeconds(expiresAtSeconds), cancellationToken);
        return Ok(ApiResponseFactory.Success<object?>(null, "Logged out successfully. The access token has been revoked."));
    }

    [HttpGet(Router.AuthenticationRoutes.ConfirmEmail)]
    [EndpointName("ConfirmEmail")]
    [EndpointSummary("Confirm ownership of a registered email address")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ConfirmEmailCommand(userId, token), cancellationToken);
        return HandleResult(result, "Email confirmed successfully. You can now log in.");
    }
}
