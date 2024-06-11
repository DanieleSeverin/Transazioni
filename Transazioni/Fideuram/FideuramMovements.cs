using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;
using Transazioni.Domain.Users;

namespace Transazioni.Domain.Fideuram;

public record FideuramMovements
{
    public DateTime Data { get; set; }
    public string Operazione { get; set; } = null!;
    public string Dettagli { get; set; } = null!;
    public string Conto_o_Carta { get; set; } = null!;
    public string Contabilizzazione { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Valuta { get; set; } = null!;
    public decimal Importo { get; set; }

    public Movements ToMovement(AccountId OriginAccountId, AccountId DestinationAccountId, UserId UserId)
    {
        return new Movements(Data,
                             new MovementDescription(Dettagli),
                             new Money(Importo, Currency.FromCode(Valuta)),
                             accountId: OriginAccountId,
                             destinationAccountId: DestinationAccountId,
                             new MovementCategory(Categoria),
                             isImported: true,
                             peridiocity: Peridiocity.None,
                             userId: UserId);
    }

}
