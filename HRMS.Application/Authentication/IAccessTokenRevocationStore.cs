namespace HRMS.Application.Authentication;

public interface IAccessTokenRevocationStore
{
    Task RevokeAsync(string tokenId, DateTimeOffset expiresAt, CancellationToken cancellationToken);
    Task<bool> IsRevokedAsync(string tokenId, CancellationToken cancellationToken);
}
