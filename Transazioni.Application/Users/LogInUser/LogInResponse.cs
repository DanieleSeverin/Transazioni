using Transazioni.Domain.Tokens;

namespace Transazioni.Application.Users.LogInUser;

public sealed record LogInResponse(AccessToken AccessToken, RefreshToken RefreshToken);