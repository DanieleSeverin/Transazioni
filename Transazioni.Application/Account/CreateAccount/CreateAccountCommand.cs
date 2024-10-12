using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Account;

namespace Transazioni.Application.Account.CreateAccount;

public sealed record CreateAccountCommand(
    string AccountName, 
    string AccountType, 
    Guid UserId) : ICommand<Accounts>;
