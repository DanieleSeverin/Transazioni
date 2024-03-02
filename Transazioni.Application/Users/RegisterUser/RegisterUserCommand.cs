using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string Password) : ICommand<Guid>;