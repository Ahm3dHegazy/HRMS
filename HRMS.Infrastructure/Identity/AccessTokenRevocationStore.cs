using HRMS.Application.Authentication;

namespace HRMS.Infrastructure.Identity;

public sealed class AccessTokenRevocationStore(AppDbContext dbContext) : IAccessTokenRevocationStore
{
    public async Task RevokeAsync(string tokenId, DateTimeOffset expiresAt, CancellationToken cancellationToken)
    {
        await dbContext.RevokedAccessTokens
            .Where(token => token.ExpiresAt <= DateTimeOffset.UtcNow)
            .ExecuteDeleteAsync(cancellationToken);

        dbContext.RevokedAccessTokens.Add(new RevokedAccessToken(tokenId, expiresAt));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> IsRevokedAsync(string tokenId, CancellationToken cancellationToken)
    {
        return dbContext.RevokedAccessTokens.AnyAsync(token =>
            token.TokenId == tokenId && token.ExpiresAt > DateTimeOffset.UtcNow, cancellationToken);
    }
}
