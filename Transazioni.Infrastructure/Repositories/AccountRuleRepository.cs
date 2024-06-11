using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.Users;

namespace Transazioni.Infrastructure.Repositories;

internal class AccountRuleRepository : IAccountRuleRepository
{
    private readonly ApplicationDbContext DbContext;

    public AccountRuleRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void Add(AccountRules AccountRule)
    {
        DbContext.Set<AccountRules>().Add(AccountRule);
    }

    public async Task<List<AccountRules>> GetAccountRules(UserId UserId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<AccountRules>()
            .Where(account => account.UserId == UserId)
            .ToListAsync(cancellationToken);
    }
}