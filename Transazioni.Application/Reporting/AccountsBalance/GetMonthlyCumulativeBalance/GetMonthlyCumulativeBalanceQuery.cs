using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;

namespace Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;

public sealed record GetMonthlyCumulativeBalanceQuery : IQuery<List<MonthlyAccountBalanceSummary>>;
