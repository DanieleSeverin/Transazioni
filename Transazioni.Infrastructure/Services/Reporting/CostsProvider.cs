using Microsoft.EntityFrameworkCore;
using Transazioni.Application.Reporting.GetCosts;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;

namespace Transazioni.Infrastructure.Services.Reporting;

internal class CostsProvider : ICostsProvider
{
    private readonly ApplicationDbContext dbContext;

    public CostsProvider(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<CostsSummary>> GetCosts(CancellationToken cancellationToken)
    {
        var query = from movement in dbContext.Set<Movements>()
                    join account in dbContext.Set<Accounts>() on movement.DestinationAccountId equals account.Id
                    where movement.Money.Amount < 0 && !account.IsPatrimonial
                    group new { movement.DestinationAccountId, account.AccountName, movement.Money.Currency, movement.Money.Amount }
                    by new { movement.DestinationAccountId, account.AccountName, movement.Money.Currency } into grouped
                    orderby grouped.Sum(m => m.Amount)
                    select new
                    {
                        DestinationAccountId = grouped.Key.DestinationAccountId,
                        AccountName = grouped.Key.AccountName,
                        Amount = grouped.Sum(m => m.Amount),
                        Currency = grouped.Key.Currency
                    };

        var materializedQuery = await query.ToListAsync(cancellationToken);

        return materializedQuery.Select(result => new CostsSummary(
            DestinationAccountId : result.DestinationAccountId.Value,
            AccountName : result.AccountName.Value,
            Amount : result.Amount,
            Currency : result.Currency.Code
        )).ToList();
    }
}
