using Microsoft.EntityFrameworkCore;
using Transazioni.Application.Reporting.AccountsBalance;
using Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;
using Transazioni.Application.Reporting.AccountsBalance.GetMonthlyCumulativeBalance;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;

namespace Transazioni.Infrastructure.Services.Reporting;

public class AccountBalanceProvider : IAccountBalanceProvider
{
    private readonly ApplicationDbContext DbContext;

    public AccountBalanceProvider(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<AccountsBalanceSummary>> GetAccountsBalance(CancellationToken cancellationToken)
    {
        var today = DateTime.Today;

        var query = from account in DbContext.Set<Accounts>()
                    where account.AccountType == new AccountType(DefaultAccountTypes.Bank)
                    join movement in DbContext.Set<Movements>()
                        on account.Id equals movement.AccountId into movementGroup
                    from movementTotal in movementGroup.DefaultIfEmpty()
                    where movementTotal.Date <= today
                    group movementTotal by new { account.Id, account.AccountName, movementTotal.Money.Currency } into grouped
                    select new
                    {
                        AccountId = grouped.Key.Id,
                        AccountName = grouped.Key.AccountName,
                        Balance = grouped.Sum(g => g.Money.Amount),
                        Currency = grouped.Key.Currency.Code
                    } into result
                    orderby result.Balance descending
                    select result;

        var materializedQuery = await query.ToListAsync(cancellationToken);

        return materializedQuery.Select(result => new AccountsBalanceSummary(
            AccountId: result.AccountId.Value,
            AccountName: result.AccountName.Value,
            Balance: result.Balance,
            Currency: result.Currency
        )).ToList();
    }

    public async Task<List<MonthlyAccountBalanceSummary>> GetMonthlyCumulativeBalance(CancellationToken cancellationToken)
    {
        // Step 1: Aggregate movements to get monthly balances for each account
        var monthlyMovementsQuery = from account in DbContext.Set<Accounts>()
                                    where account.AccountType == new AccountType(DefaultAccountTypes.Bank)
                                    join movement in DbContext.Set<Movements>()
                                        on account.Id equals movement.AccountId into movementGroup
                                    from movementTotal in movementGroup.DefaultIfEmpty()
                                    where movementTotal != null
                                    group movementTotal by new
                                    {
                                        account.Id,
                                        account.AccountName,
                                        Month = new DateTime(movementTotal.Date.Year, movementTotal.Date.Month, 1)
                                    } into grouped
                                    select new
                                    {
                                        AccountId = grouped.Key.Id,
                                        AccountName = grouped.Key.AccountName,
                                        Month = grouped.Key.Month,
                                        MonthlyBalance = grouped.Sum(m => m.Money.Amount)
                                    };

        // Step 2: Materialize the monthly balances and order by AccountId and Month
        var monthlyMovements = await monthlyMovementsQuery
            .OrderBy(result => result.AccountId)
            .ThenBy(result => result.Month)
            .ToListAsync(cancellationToken);

        // Step 3: Calculate cumulative balance by account
        var cumulativeBalances = new List<MonthlyAccountBalanceSummary>();
        foreach (var accountGroup in monthlyMovements.GroupBy(x => x.AccountId))
        {
            decimal cumulativeBalance = 0;
            foreach (var movement in accountGroup.OrderBy(x => x.Month))
            {
                cumulativeBalance += movement.MonthlyBalance;
                cumulativeBalances.Add(new MonthlyAccountBalanceSummary(
                    AccountId: movement.AccountId.Value,
                    AccountName: movement.AccountName.Value,
                    Month: movement.Month,
                    CumulativeBalance: cumulativeBalance
                ));
            }
        }

        return cumulativeBalances.OrderBy(x => x.AccountId).ThenBy(x => x.Month).ToList();
    }


}
