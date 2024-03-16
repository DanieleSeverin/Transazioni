using Microsoft.EntityFrameworkCore;
using Transazioni.Domain.AccountRule;

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

    public async Task<List<AccountRules>> GetAccountRules(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<AccountRules>().ToListAsync(cancellationToken);
    }
}