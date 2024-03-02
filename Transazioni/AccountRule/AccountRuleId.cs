namespace Transazioni.Domain.AccountRule;

public record AccountRuleId(Guid Value)
{
    public static AccountRuleId New() => new(Guid.NewGuid());
}
