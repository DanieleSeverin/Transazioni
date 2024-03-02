using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Reporting.GetAccountsBalance;

public sealed record GetAccountsBalanceQuery : IQuery<List<AccountsBalanceSummary>>;
