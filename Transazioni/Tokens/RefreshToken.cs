using Transazioni.Domain.Users;

namespace Transazioni.Domain.Tokens;

public class RefreshToken
{
    public RefreshTokenId Id { get; init; }
    public string Value { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpireAt { get; init; }
    public UserId UserId { get; init; }

    public User User { get; init; } = null!;

    public RefreshToken(string value, UserId userId)
    {
        Id = RefreshTokenId.New();
        Value = value;
        CreatedAt = DateTime.Now;
        ExpireAt = CreatedAt.AddHours(24);
        UserId = userId;
    }

    private RefreshToken() { }
}
