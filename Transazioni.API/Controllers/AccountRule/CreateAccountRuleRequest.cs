using System.Text.Json.Serialization;

namespace Transazioni.API.Controllers.AccountRule;

public sealed record CreateAccountRuleRequest(
    [property: JsonRequired] Guid AccountId,
    [property: JsonRequired] string Query)
{
}
