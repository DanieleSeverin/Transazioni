using Transazioni.Domain.Movement;

namespace Transazioni.Domain.Account;

public class Accounts
{
    public AccountId Id { get; init; }
    public AccountName AccountName { get; init; } = null!;
    public bool IsPatrimonial { get; init; }
    
    private readonly List<Movements> _movements = new List<Movements>();
    public IReadOnlyList<Movements> Movements => _movements.ToList();

    private readonly List<Movements> _destinationMovements = new List<Movements>();
    public IReadOnlyList<Movements> DestinationMovements => _destinationMovements.ToList();

    public Accounts(AccountName accountName, bool isPatrimonial)
    {
        Id = AccountId.New();
        AccountName = accountName;
        IsPatrimonial = isPatrimonial;
    }

    public void AddMovement(Movements movement)
    {
        _movements.Add(movement);
    }

    public void AddDestinationMovement(Movements movement)
    {
        _destinationMovements.Add(movement);
    }
}