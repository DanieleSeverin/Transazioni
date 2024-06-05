using Transazioni.Domain.Account;

namespace Transazioni.Domain.AccountRule;

public static class AccountRuleExtensions
{
    public static AccountName? GetAccountName(this List<AccountRules> Rules, string MovementDescription)
    {
        return Rules.Find(rule => rule.Match(MovementDescription))?.AccountName;
    }
}
