using System.Text.Json.Serialization;

namespace Transazioni.API.Controllers.Account;

public sealed record CreateAccountRequest(
    [property: JsonRequired] string AccountName,
    [property: JsonRequired] string AccountType,
    [property: JsonRequired] bool IsPatrimonial);

