namespace Transazioni.Application.Reporting.GetAccountsBalance;

public interface IAccountBalanceProvider
{
    public abstract Task<List<AccountsBalanceSummary>> GetAccountsBalance();
}
