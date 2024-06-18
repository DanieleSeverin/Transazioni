using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Auth.Logout;

public sealed record LogoutCommand(string Token) : ICommand;
