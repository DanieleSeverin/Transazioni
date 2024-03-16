namespace Transazioni.API.Controllers.AccountRule;

public sealed record CreateAccountRuleRequest(Guid AccountId, string Query)
{
}
