using Transazioni.Domain.Account;
using Transazioni.Domain.Shared;

namespace Transazioni.Domain.Movement;

public class Movements
{
    public MovementId Id { get; init; }
    public DateTime Date { get; init; }
    public MovementDescription Description { get; init; }
    public Money Money { get; init; }
    public AccountId AccountId { get; init; }
    public AccountId DestinationAccountId { get; init; }
    public MovementCategory? Category { get; init; }

    public Accounts Account { get; init; } = null!;
    public Accounts DestinationAccount { get; init; } = null!;

    public Movements(DateTime date,
                     MovementDescription description,
                     Money money, 
                     AccountId accountId,
                     AccountId destinationAccountId,
                     MovementCategory? category)
    {
        Id = MovementId.New();
        Date = date;
        Description = description;
        Money = money;
        AccountId = accountId;
        DestinationAccountId = destinationAccountId;
        Category = category;
    }

    private Movements() { }
}
