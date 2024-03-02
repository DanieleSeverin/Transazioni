using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<LogInResponse>;