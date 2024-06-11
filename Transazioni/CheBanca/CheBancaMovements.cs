using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;
using Transazioni.Domain.Users;

namespace Transazioni.Domain.CheBanca;

public record CheBancaMovements
{
    public DateTime DataContabile { get; init; }
    public DateTime DataValuta { get; init; }
    public string Tipologia { get; init; } = null!;
    public decimal? Entrate { get; init; }
    public decimal? Uscite { get; init; }
    public string Divisa { get; init; } = null!;

    public Movements ToMovement(AccountId OriginAccountId, AccountId DestinationAccountId, UserId UserId)
    {
        if(Entrate is null && Uscite is null)
        {
            throw new InvalidCheBancaMovementException("Entrate e Uscite can't be both null.");
        }

        decimal Importo = (Entrate ?? Uscite)!.Value;

        return new Movements(DataContabile, 
                             new MovementDescription(Tipologia), 
                             new Money(Importo, Currency.FromCode(Divisa)),
                             accountId: OriginAccountId,
                             destinationAccountId: DestinationAccountId, 
                             category: null,
                             isImported: true,
                             peridiocity: Peridiocity.None,
                             userId: UserId);
    }

    public bool HasAmount()
    {
        return Entrate is not null || Uscite is not null;
    }
}
