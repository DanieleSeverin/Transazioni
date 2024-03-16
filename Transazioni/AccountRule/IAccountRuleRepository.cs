using Transazioni.Domain.Account;

namespace Transazioni.Domain.AccountRule;

public interface IAccountRuleRepository
{
    public abstract Task<List<AccountRules>> GetAccountRules(CancellationToken cancellationToken = default);
    public abstract void Add(AccountRules AccountRule);
}
