namespace HRMS.Domain.Entities;

public sealed class RevokedAccessToken
{
    public string TokenId { get; private set; } = null!;
    public DateTimeOffset ExpiresAt { get; private set; }

    private RevokedAccessToken() { }

    public RevokedAccessToken(string tokenId, DateTimeOffset expiresAt)
    {
        TokenId = tokenId;
        ExpiresAt = expiresAt;
    }
}
