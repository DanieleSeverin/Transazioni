using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Domain.Satispay;

public record SatispayMovements
{
    public DateTime Date { get; set; }
    public string Name { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public decimal Amount { get; set; }
   
    public Movements ToMovement(AccountId OriginAccountId, AccountId DestinationAccountId)
    {
        return new Movements(Date,
                             new MovementDescription(Name),
                             new Money(Amount, Shared.Currency.FromCode(Currency)),
                             accountId: OriginAccountId,
                             destinationAccountId: DestinationAccountId,
                             category: null,
                             isImported: true,
                             peridiocity: Peridiocity.None);
    }
}
