using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Domain.Santander;

public record SantanderMovements
{
    public DateTime DataMovimento { get; set; }
    public string DescrizioneOperazione { get; set; } = null!;
    public string Causale { get; set; } = null!;
    public decimal Importo { get; set; }
    public string Divisa { get; set; } = null!;

    public Movements ToMovement(AccountId OriginAccountId, AccountId DestinationAccountId)
    {
        return new Movements(DataMovimento,
                             new MovementDescription(DescrizioneOperazione),
                             new Money(Importo, Currency.FromCode(Divisa)),
                             accountId: OriginAccountId,
                             destinationAccountId: DestinationAccountId,
                             new MovementCategory(Causale));
    }
}
