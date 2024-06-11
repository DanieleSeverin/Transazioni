using Microsoft.EntityFrameworkCore;
using System.Threading;
using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Infrastructure.Repositories;

internal class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext DbContext;

    public AccountRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(Accounts Account)
    {
        DbContext.Set<Accounts>().Add(Account);
    }

    public void AddRange(List<Accounts> Accounts)
    {
        DbContext.Set<Accounts>().AddRange(Accounts);
    }

    public async Task<List<Accounts>> GetAccounts(UserId UserId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>()
            .Where(account => account.UserId == UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Accounts?> GetById(UserId UserId, AccountId AccountId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>().FirstOrDefaultAsync(
            account => account.UserId == UserId && account.Id == AccountId,
            cancellationToken);
    }

    public async Task<Accounts?> GetByName(UserId UserId, AccountName AccountName, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>().FirstOrDefaultAsync(
            account => account.UserId == UserId && account.AccountName == AccountName,
            cancellationToken);
    }
}
