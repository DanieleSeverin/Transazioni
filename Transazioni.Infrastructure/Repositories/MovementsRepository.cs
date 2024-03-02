using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Movements>> Get()
    {
        return await DbContext.Set<Movements>().ToListAsync();
    }

    public async Task RemoveDateRange(AccountId AccountId, DateTime StartDate, DateTime EndDate)
    {
        if(StartDate > EndDate)
        {
            throw new ArgumentException("EndDate should be bigger than StartDate.");
        }

        var movements = await DbContext.Set<Movements>()
            .Where(x => x.AccountId == AccountId && x.Date >= StartDate && x.Date <= EndDate)
            .ToListAsync();

        var moneys = movements.Select(x => x.Money);
        DbContext.RemoveRange(moneys);

        DbContext.Set<Movements>().RemoveRange(movements);

        await DbContext.SaveChangesAsync();
    }
}
