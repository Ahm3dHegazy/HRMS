namespace HRMS.Api.Controllers;

[AllowAnonymous]
public class AuthController(ISender sender) : ApiControllerBase
{
    [HttpPost(Router.AuthenticationRoutes.RegisterUser)]
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
    [EndpointName("Login")]
    [EndpointSummary("Authenticate and receive a JWT access token")]
    public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new LoginCommand(request), cancellationToken);
        return HandleResult(result, "Login successful");
    }

    [HttpGet(Router.AuthenticationRoutes.ConfirmEmail)]
    [AllowAnonymous]
    [EndpointName("ConfirmEmail")]
    [EndpointSummary("Confirm ownership of a registered email address")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ConfirmEmailCommand(userId, token), cancellationToken);
        return HandleResult(result, "Email confirmed successfully. You can now log in.");
    }

}
