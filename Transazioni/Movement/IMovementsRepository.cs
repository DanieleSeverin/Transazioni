using Transazioni.Domain.Account;

namespace Transazioni.Domain.Movement;

public interface IMovementsRepository
{
    public void Add(Movements movement);
    public Task<List<Movements>> Get();
    public Task RemoveDateRange(AccountId AccountId, DateTime StartDate, DateTime EndDate);
}
