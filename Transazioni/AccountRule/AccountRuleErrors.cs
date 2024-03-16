using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.AccountRule;

public static class AccountRuleErrors
{
    public static Error AlreadyExists = new(
    "AccountRule.AlreadyExists",
    "The account rule already exists.");
}
