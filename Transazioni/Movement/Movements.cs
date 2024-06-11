using Transazioni.Domain.Account;
using Transazioni.Domain.Shared;
using Transazioni.Domain.Users;

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
    public bool IsImported { get; init; } = false;
    public Peridiocity Peridiocity { get; init; } = Peridiocity.None;
    public UserId UserId { get; init; }


    public Accounts Account { get; init; } = null!;
    public Accounts DestinationAccount { get; init; } = null!;
    public User User { get; init; }


    public Movements(DateTime date,
                     MovementDescription description,
                     Money money, 
                     AccountId accountId,
                     AccountId destinationAccountId,
                     MovementCategory? category,
                     bool isImported,
                     Peridiocity peridiocity)
    {
        Id = MovementId.New();
        Date = date;
        Description = description;
        Money = money;
        AccountId = accountId;
        DestinationAccountId = destinationAccountId;
        Category = category;
        IsImported = isImported;
        Peridiocity = peridiocity;
    }

    private Movements() { }
}
