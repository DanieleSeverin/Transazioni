namespace Transazioni.Domain.Tokens;

public class AccessToken
{
    public string Value { get; init; }
    public DateTime ExpireAt { get; init; }

    public AccessToken(string value, DateTime expireAt)
    {
        Value = value;
        ExpireAt = expireAt;
    }
}
