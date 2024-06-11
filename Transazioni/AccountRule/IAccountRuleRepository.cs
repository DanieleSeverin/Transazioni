using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Domain.AccountRule;

public interface IAccountRuleRepository
{
    public abstract Task<List<AccountRules>> GetAccountRules(UserId UserId, CancellationToken cancellationToken = default);
    public abstract void Add(AccountRules AccountRule);
}
