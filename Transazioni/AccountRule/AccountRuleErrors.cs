using Transazioni.Domain.Abstractions;

namespace Transazioni.Domain.AccountRule;

public static class AccountRuleErrors
{
    public static readonly Error AlreadyExists = new(
    "AccountRule.AlreadyExists",
    "The account rule already exists.");
}
