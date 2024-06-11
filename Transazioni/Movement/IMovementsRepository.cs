using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Domain.Movement;

public interface IMovementsRepository
{
    public void Add(Movements movement);
    public Task<List<Movements>> Get(UserId UserId, CancellationToken cancellationToken = default);
    public Task RemoveDateRange(UserId UserId, AccountId AccountId, DateTime StartDate, DateTime EndDate, CancellationToken cancellationToken);
}
