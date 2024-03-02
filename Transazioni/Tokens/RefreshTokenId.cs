namespace Transazioni.Domain.Tokens;

public record RefreshTokenId(Guid Value)
{
    public static RefreshTokenId New() => new(Guid.NewGuid());
}
