using Microsoft.EntityFrameworkCore;
using Transazioni.Application.Reporting.GetAccountsBalance;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Infrastructure.Services.Reporting;

public class AccountBalanceProvider : IAccountBalanceProvider
{
    private readonly ApplicationDbContext DbContext;

    public AccountBalanceProvider(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<AccountsBalanceSummary>> GetAccountsBalance()
    {
        var today = DateTime.Today;

        var query = from account in DbContext.Set<Accounts>()
                    where account.IsPatrimonial
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

        var materializedQuery = await query.ToListAsync();

        return materializedQuery.Select(result => new AccountsBalanceSummary(
            AccountId: result.AccountId.Value,
            AccountName: result.AccountName.Value,
            Balance: result.Balance,
            Currency: result.Currency
        )).ToList();
    }


}
