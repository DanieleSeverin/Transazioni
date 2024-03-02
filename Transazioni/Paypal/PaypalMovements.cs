using Transazioni.Domain.Account;
using Transazioni.Domain.CheBanca;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Domain.Paypal;

public record PaypalMovements
{
    public DateTime Data { get; set; }
    public string Descrizione { get; set; } = null!;
    public string Valuta { get; set; } = null!;
    public decimal Lordo { get; set; }
    public string? Nome { get; set; }
    public string? NomeBanca { get; set; }

    public Movements ToMovement(AccountId OriginAccountId, AccountId DestinationAccountId)
    {
        if (Nome is null && NomeBanca is null && Descrizione is null)
        {
            throw new InvalidCheBancaMovementException("Nome and NomeBanca and Descrizione can't be null.");
        }

        return new Movements(Data,
                             new MovementDescription(Descrizione),
                             new Money(Lordo, Currency.FromCode(Valuta)),
                             accountId: OriginAccountId,
                             destinationAccountId: DestinationAccountId,
                             category: null,
                             isImported: true,
                             peridiocity: Peridiocity.None);
    }
}
