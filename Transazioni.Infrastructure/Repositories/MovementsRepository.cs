using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Users;

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

    public async Task<List<Movements>> Get(UserId UserId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Movements>()
            .Include(account => account.Account)
            .Include(account => account.DestinationAccount)
            .Where(account => account.UserId == UserId)
            .AsSingleQuery()
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveDateRange(UserId UserId, AccountId AccountId, DateTime StartDate, DateTime EndDate, CancellationToken cancellationToken)
    {
        if(StartDate > EndDate)
        {
            throw new ArgumentException("EndDate should be bigger than StartDate.");
        }

        var movements = await DbContext.Set<Movements>()
            .Where(account => account.UserId == UserId && 
                              account.AccountId == AccountId && 
                              account.Date >= StartDate && 
                              account.Date <= EndDate)
            .ToListAsync(cancellationToken);

        var moneys = movements.Select(x => x.Money);
        DbContext.RemoveRange(moneys);

        DbContext.Set<Movements>().RemoveRange(movements);

        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
