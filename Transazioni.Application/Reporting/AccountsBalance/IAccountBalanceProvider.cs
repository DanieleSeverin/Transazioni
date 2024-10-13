using Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;
using Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;

namespace Transazioni.Application.Reporting.AccountsBalance;

public interface IAccountBalanceProvider
{
    public abstract Task<List<AccountsBalanceSummary>> GetAccountsBalance(CancellationToken cancellationToken = default);
    public abstract Task<List<MonthlyAccountBalanceSummary>> GetMonthlyCumulativeBalance(
        DateTime? minDate = null,
        DateTime? maxDate = null,
        CancellationToken cancellationToken = default);
}
