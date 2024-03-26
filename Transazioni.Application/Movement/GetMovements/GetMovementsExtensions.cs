using Transazioni.Domain.Movement;

namespace Transazioni.Application.Movement.GetMovements;

public static class GetMovementsExtensions
{
    public static List<GetMovementsResponse> ToDto(this IEnumerable<Movements> movements)
    {
        return movements.Select(mov => new GetMovementsResponse(
            Id: mov.Id.Value,
            Date: mov.Date,
            Description: mov.Description.Value,
            Amount: mov.Money.Amount,
            Currency: mov.Money.Currency.Code,
            OriginAccount: new MovementAccount(
                mov.AccountId.Value,
                mov.Account.AccountName.Value,
                mov.Account.IsPatrimonial),
            DestinationAccount: new MovementAccount(
                mov.DestinationAccountId.Value,
                mov.DestinationAccount.AccountName.Value,
                mov.DestinationAccount.IsPatrimonial),
            Category: mov.Category?.Value,
            IsImported: mov.IsImported,
            Peridiocity: mov.Peridiocity.ToString()
            )).ToList();
    }
}
