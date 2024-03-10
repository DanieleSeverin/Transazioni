using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;

namespace Transazioni.Application.Movement.CreateMovement;

public class CreateMovementCommandHandler : ICommandHandler<CreateMovementCommand, Movements>
{
    private readonly IMovementsRepository _movementsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMovementCommandHandler(IMovementsRepository movementsRepository, 
                                        IUnitOfWork unitOfWork)
    {
        _movementsRepository = movementsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Movements>> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
    {
        //TODO: add validations 

        DateTime startDate = request.Request.date;
        MovementDescription description = new MovementDescription(request.Request.description);
        Money amount = new Money(request.Request.amount, Currency.FromCode(request.Request.currency));
        AccountId accountId = new AccountId(request.Request.accountId);
        AccountId destinationAccountId = new AccountId(request.Request.destinationAccountId);
        MovementCategory? category = string.IsNullOrWhiteSpace(request.Request.currency) ? null 
            : new MovementCategory(request.Request.category!);
        Peridiocity peridiocity = PeridiocityUtility.GetFromString(request.Request.periodicity);

        Movements movement = new Movements(startDate,
                                           description,
                                           amount,
                                           accountId,
                                           destinationAccountId,
                                           category,
                                           isImported : false,
                                           peridiocity);

        _movementsRepository.Add(movement);

        // Create additional movements if peridiocity is not None
        if (peridiocity != Peridiocity.None)
        {
            DateTime maxEndDate = new DateTime(2049, 12, 31);
            DateTime nextDate = startDate.GetNextPeridiocityDate(peridiocity);

            while (nextDate <= maxEndDate)
            {
                Movements nextMovement = new Movements(nextDate,
                                                       description,
                                                       amount,
                                                       accountId,
                                                       destinationAccountId,
                                                       category,
                                                       isImported: false,
                                                       peridiocity);
                _movementsRepository.Add(nextMovement);
                nextDate = nextDate.GetNextPeridiocityDate(peridiocity);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return Result.Success(movement);
    }
}
