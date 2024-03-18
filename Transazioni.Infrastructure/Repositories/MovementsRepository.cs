using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;

namespace Transazioni.Infrastructure.Repositories;

internal class MovementsRepository : IMovementsRepository
{
    private readonly ApplicationDbContext DbContext;

    public MovementsRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(Movements movement)
    {
        DbContext.Set<Movements>().Add(movement);
    }

    public async Task<List<Movements>> Get(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Movements>()
            .Include(x => x.Account)
            .Include(x => x.DestinationAccount)
            .AsSingleQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveDateRange(AccountId AccountId, DateTime StartDate, DateTime EndDate, CancellationToken cancellationToken)
    {
        if(StartDate > EndDate)
        {
            throw new ArgumentException("EndDate should be bigger than StartDate.");
        }

        var movements = await DbContext.Set<Movements>()
            .Where(x => x.AccountId == AccountId && x.Date >= StartDate && x.Date <= EndDate)
            .ToListAsync(cancellationToken);

        var moneys = movements.Select(x => x.Money);
        DbContext.RemoveRange(moneys);

        DbContext.Set<Movements>().RemoveRange(movements);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
