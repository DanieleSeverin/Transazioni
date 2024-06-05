using System.Text.Json.Serialization;

namespace Transazioni.API.Controllers.Authentication;

public sealed record LogInUserRequest(
    [property: JsonRequired] string Email, 
    [property: JsonRequired] string Password);