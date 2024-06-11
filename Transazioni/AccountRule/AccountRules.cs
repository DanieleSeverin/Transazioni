using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Domain.AccountRule;

public class AccountRules
{
    public AccountRuleId Id { get; init; }
    public RuleContains RuleContains { get; init; }
    public AccountName AccountName { get; init; }
    public UserId UserId { get; init; }
    public User User { get; init; }

    public AccountRules(RuleContains ruleContains, AccountName accountName)
    {
        Id = AccountRuleId.New();
        RuleContains = ruleContains;
        AccountName = accountName;
    }

    public bool Match(string MovementDescription)
    {
        MovementDescription = MovementDescription.Trim().ToLower();
        return MovementDescription.Contains(RuleContains.Value.Trim().ToLower());
    }
}
