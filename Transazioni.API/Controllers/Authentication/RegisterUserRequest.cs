using System.Text.Json.Serialization;

namespace Transazioni.API.Controllers.Authentication;

public sealed record RegisterUserRequest(
    [property: JsonRequired] string Email,
    [property: JsonRequired] string FirstName,
    [property: JsonRequired] string LastName,
    [property: JsonRequired] string Password);