namespace Transazioni.Domain.Tokens;

public class AccessToken
{
    public string Value { get; init; }

    public AccessToken(string value)
    {
        Value = value;
    }
}
