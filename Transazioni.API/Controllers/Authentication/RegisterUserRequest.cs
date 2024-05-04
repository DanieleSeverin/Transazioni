namespace Transazioni.API.Controllers.Authentication;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);