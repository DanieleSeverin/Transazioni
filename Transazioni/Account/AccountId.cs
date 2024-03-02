namespace Transazioni.Domain.Account;

public record AccountId(Guid Value)
{
    public static AccountId New() => new(Guid.NewGuid());
}
