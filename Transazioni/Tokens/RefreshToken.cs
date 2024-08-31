using Transazioni.Domain.Users;

namespace Transazioni.Domain.Tokens;

public class RefreshToken
{
    public RefreshTokenId Id { get; init; }
    public string Value { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpireAt { get; init; }
    public UserId UserId { get; init; }
    public bool Valid { get; private set; }
    public string? InvalidityReason { get; private set; }
    public DateTime? InvalidatedAt { get; private set; }

    public User User { get; init; } = null!;

    private readonly int _durationDays = 7;

    public RefreshToken(string value, UserId userId)
    {
        Id = RefreshTokenId.New();
        Value = value;
        CreatedAt = DateTime.Now;
        ExpireAt = CreatedAt.AddDays(_durationDays);
        UserId = userId;
        Valid = true;
    }

    #pragma warning disable CS8618
    private RefreshToken() { }
    #pragma warning restore CS8618

    public void Invalidate(string? reason = null)
    {
        if (!Valid) throw new RefreshTokenAlreadyInvalidException(Id);

        Valid = false;
        InvalidityReason = reason;
        InvalidatedAt = DateTime.Now;
    }
}
