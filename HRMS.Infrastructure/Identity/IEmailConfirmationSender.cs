namespace HRMS.Infrastructure.Identity;

public interface IEmailConfirmationSender
{
    Task SendConfirmationAsync(string email, string userId, string token,
        CancellationToken cancellationToken = default);
}
