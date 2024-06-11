using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Account;

namespace Transazioni.Application.Account.CreateAccount;

public sealed record CreateAccountCommand(string AccountName, bool IsPatrimonial, Guid UserId) : ICommand<Accounts>;
