using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Shared;
using Transazioni.Domain.Users;

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
        UserId userId = new(request.UserId);
        DateTime startDate = request.Request.date.Date;
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
                                           peridiocity,
                                           userId);

        _movementsRepository.Add(movement);

        // Create additional movements if peridiocity is not None
        if (peridiocity != Peridiocity.None)
        {
            DateTime maxEndDate = new DateTime(2034, 12, 31, 0, 0, 0, DateTimeKind.Utc);
            DateTime nextDate = startDate.GetNextPeridiocityDate(peridiocity);

            while (nextDate <= maxEndDate)
            {
                // must instantiate new Money object to avoid conflict with Entity Framework
                Money amount2 = new Money(request.Request.amount, Currency.FromCode(request.Request.currency));

                Movements nextMovement = new Movements(nextDate,
                                                       description,
                                                       amount2,
                                                       accountId,
                                                       destinationAccountId,
                                                       category,
                                                       isImported: false,
                                                       peridiocity, 
                                                       userId);
                _movementsRepository.Add(nextMovement);
                nextDate = nextDate.GetNextPeridiocityDate(peridiocity);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(movement);
    }
}
