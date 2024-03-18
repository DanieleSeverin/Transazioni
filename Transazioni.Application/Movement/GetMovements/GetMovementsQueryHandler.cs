using DocumentFormat.OpenXml.Office2010.Excel;
using System.Linq;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Application.Movement.GetMovements;

public class GetMovementsQueryHandler : IQueryHandler<GetMovementsQuery, List<GetMovementsResponse>>
{
    public readonly IMovementsRepository _movementsRepository;

    public GetMovementsQueryHandler(IMovementsRepository movementsRepository)
    {
        _movementsRepository = movementsRepository;
    }

    public async Task<Result<List<GetMovementsResponse>>> Handle(GetMovementsQuery request, CancellationToken cancellationToken)
    {
        List<Movements> movements = await _movementsRepository.Get(cancellationToken);

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
