using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Users.LogInUser;

namespace Transazioni.Application.Auth.RefreshJwt;

public sealed record RefreshJwtCommand(string AccessToken, string RefreshToken) : ICommand<LogInResponse>;
