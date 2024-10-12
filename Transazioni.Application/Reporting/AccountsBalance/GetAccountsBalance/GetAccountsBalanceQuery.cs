using Transazioni.Application.Abstractions.Messaging;

namespace Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;

public sealed record GetAccountsBalanceQuery : IQuery<List<AccountsBalanceSummary>>;
