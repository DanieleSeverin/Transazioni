using Microsoft.EntityFrameworkCore;
using System.Threading;
using Transazioni.Domain.Account;

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

    public async Task<List<Accounts>> GetAccounts(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>().ToListAsync(cancellationToken);
    }

    public async Task<Accounts?> GetById(AccountId AccountId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>().FirstOrDefaultAsync(
            account => account.Id == AccountId,
            cancellationToken);
    }

    public async Task<Accounts?> GetByName(AccountName AccountName, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Accounts>().FirstOrDefaultAsync(
            account => account.AccountName == AccountName,
            cancellationToken);
    }
}
