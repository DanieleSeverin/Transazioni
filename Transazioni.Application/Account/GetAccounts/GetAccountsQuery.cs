using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Account;

namespace Transazioni.Application.Account.GetAccounts;

public sealed record GetAccountsQuery() : IQuery<List<Accounts>>;
