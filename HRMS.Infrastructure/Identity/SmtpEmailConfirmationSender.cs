using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace HRMS.Infrastructure.Identity;

public sealed class SmtpEmailConfirmationSender(
    IConfiguration configuration,
    IHttpContextAccessor httpContextAccessor) : IEmailConfirmationSender
{
    public async Task SendConfirmationAsync(string email, string userId, string token,
        CancellationToken cancellationToken = default)
    {
        var host = configuration["Email:Smtp:Host"];
        var from = configuration["Email:Smtp:From"];
        var confirmationUrl = GetConfirmationUrl();
        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(from) ||
            string.IsNullOrWhiteSpace(confirmationUrl))
        {
            throw new InvalidOperationException(
                "Email:Smtp:Host, Email:Smtp:From, and Email:ConfirmationUrl must be configured.");
        }

        var port = configuration.GetValue<int?>("Email:Smtp:Port") ?? 587;
        var enableSsl = configuration.GetValue("Email:Smtp:EnableSsl", true);
        var username = configuration["Email:Smtp:Username"];
        var password = configuration["Email:Smtp:Password"];
        var link = BuildConfirmationLink(confirmationUrl, userId, token);

        using var message = new MailMessage(from, email)
        {
            Subject = "Confirm your HRMS email address",
            Body = $"<p>Confirm your email address to activate your HRMS account:</p>" +
                   $"<p><a href=\"{WebUtility.HtmlEncode(link)}\">Confirm email</a></p>",
            IsBodyHtml = true
        };
        using var client = new SmtpClient(host, port) { EnableSsl = enableSsl };
        if (!string.IsNullOrWhiteSpace(username))
            client.Credentials = new NetworkCredential(username, password);

        await client.SendMailAsync(message, cancellationToken);
    }

    private static string BuildConfirmationLink(string baseUrl, string userId, string token)
    {
        var separator = baseUrl.Contains('?') ? "&" : "?";
        return $"{baseUrl}{separator}userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(token)}";
    }

    private string? GetConfirmationUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request is not null)
        {
            var pathBase = request.PathBase.Value?.TrimEnd('/');
            return $"{request.Scheme}://{request.Host}{pathBase}/api/v1/auth/confirm-email";
        }

        return configuration["Email:ConfirmationUrl"];
    }
}
