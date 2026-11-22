using System.Text;

namespace HRMS.Infrastructure.Configuration;

public sealed class JwtSettings
{
    public const string SectionName = "JwtSettings";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 60;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Issuer))
            throw new InvalidOperationException("JWT issuer is not configured (JwtSettings:Issuer).");

        if (string.IsNullOrWhiteSpace(Audience))
            throw new InvalidOperationException("JWT audience is not configured (JwtSettings:Audience).");

        if (Encoding.UTF8.GetByteCount(SecretKey) < 32)
            throw new InvalidOperationException("JWT secret key must be at least 32 UTF-8 bytes (JwtSettings:SecretKey).");

        if (AccessTokenMinutes <= 0)
            throw new InvalidOperationException("JWT access token lifetime must be positive (JwtSettings:AccessTokenMinutes).");
    }
}
