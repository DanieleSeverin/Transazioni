using Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;
using Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;

namespace Transazioni.Application.Reporting.AccountsBalance;

public interface IAccountBalanceProvider
{
    public abstract Task<List<AccountsBalanceSummary>> GetAccountsBalance(CancellationToken cancellationToken);
    public abstract Task<List<MonthlyAccountBalanceSummary>> GetMonthlyCumulativeBalance(CancellationToken cancellationToken);
}
