using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Satispay;

namespace Transazioni.Application.Satispay.UploadSatispayMovements;

public class UploadSatispayMovementsCommandHandler : ICommandHandler<UploadSatispayMovementsCommand>
{
    private readonly ISatispayReader _SatispayReader;
    private readonly IAccountRepository _accountRepository;
    private readonly IMovementsRepository _movementsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadSatispayMovementsCommandHandler(ISatispayReader SatispayReader,
                                                 IAccountRepository accountRepository,
                                                 IMovementsRepository movementsRepository,
                                                 IUnitOfWork unitOfWork)
    {
        _SatispayReader = SatispayReader;
        _accountRepository = accountRepository;
        _movementsRepository = movementsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UploadSatispayMovementsCommand request, CancellationToken cancellationToken)
    {
        List<SatispayMovements> movements = _SatispayReader.ReadMovements(request.File);

        if (!movements.Any())
        {
            Error error = new Error("File.Empty", "The file is empty.");
            return Result.Failure(error);
        }

        List<Accounts> accounts = await _accountRepository.GetAccounts(cancellationToken);

        List<Accounts> accountsToCreate = new();

        Accounts? OriginAccount = accounts.FindByName(request.AccountName);
        if (OriginAccount is null)
        {
            AccountName originAccountName = new AccountName(request.AccountName);
            OriginAccount = new Accounts(originAccountName, isPatrimonial: true);
            accountsToCreate.Add(OriginAccount);
        }

        await RemoveOldMovements(OriginAccount.Id, movements, cancellationToken);

        foreach (var movement in movements)
        {
            AccountName AccountName = new AccountName(movement.Name);

            // Cerca l'account tra quelli da db o creati nuovi
            Accounts? Account = accounts.FindByName(AccountName) ??
                accountsToCreate.FindByName(AccountName);

            // Se non lo trovi, crealo
            if (Account is null)
            {
                Account = new Accounts(AccountName, isPatrimonial: false);
                accountsToCreate.Add(Account);
            }

            // In ogni caso, aggiungi movimento
            _movementsRepository.Add(movement.ToMovement(
                    OriginAccountId: OriginAccount.Id,
                    DestinationAccountId: Account.Id));
        }

        _accountRepository.AddRange(accountsToCreate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task RemoveOldMovements(AccountId id, List<SatispayMovements> movements, CancellationToken cancellationToken)
    {
        movements = movements.OrderBy(x => x.Date).ToList();
        DateTime first = movements.First().Date;
        DateTime last = movements.Last().Date;

        await _movementsRepository.RemoveDateRange(id, first, last, cancellationToken);
    }
}