namespace HRMS.Infrastructure.Data.Configurations.Authentication;

public sealed class RevokedAccessTokenConfiguration : IEntityTypeConfiguration<RevokedAccessToken>
{
    public void Configure(EntityTypeBuilder<RevokedAccessToken> builder)
    {
        builder.ToTable("RevokedAccessTokens");
        builder.HasKey(token => token.TokenId);
        builder.Property(token => token.TokenId).HasMaxLength(64);
        builder.Property(token => token.ExpiresAt).IsRequired();
        builder.HasIndex(token => token.ExpiresAt);
    }
}
