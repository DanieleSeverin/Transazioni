namespace Transazioni.Domain.AccountRule;

public interface IAccountRuleRepository
{
    public abstract Task<List<AccountRules>> GetAccountRules(CancellationToken cancellationToken = default);
}
