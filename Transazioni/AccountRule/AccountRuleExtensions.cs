using Transazioni.Domain.Account;

namespace Transazioni.Domain.AccountRule;

public static class AccountRuleExtensions
{
    public static AccountName? GetAccountName(this List<AccountRules> Rules, string MovementDescription)
    {
        foreach (var Rule in Rules)
        {
            if (Rule.Match(MovementDescription))
            {
                return Rule.AccountName;
            }
        }

        return null;
    }
}
