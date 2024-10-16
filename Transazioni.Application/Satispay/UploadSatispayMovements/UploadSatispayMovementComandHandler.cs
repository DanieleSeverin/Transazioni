﻿using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Satispay;
using Transazioni.Domain.Users;

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

        UserId userId = new(request.UserId);
        List<Accounts> accounts = await _accountRepository.GetAccounts(userId, cancellationToken);

        List<Accounts> accountsToCreate = new();

        Accounts? OriginAccount = accounts.FindByName(request.AccountName);
        if (OriginAccount is null)
        {
            AccountName originAccountName = new AccountName(request.AccountName);
            AccountType originAccountType = new AccountType(DefaultAccountTypes.Bank);
            OriginAccount = new Accounts(originAccountName, userId, originAccountType);
            accountsToCreate.Add(OriginAccount);
        }

        await RemoveOldMovements(userId, OriginAccount.Id, movements, cancellationToken);

        foreach (var movement in movements)
        {
            AccountName AccountName = new AccountName(movement.Name);

            // Cerca l'account tra quelli da db o creati nuovi
            Accounts? Account = accounts.FindByName(AccountName) ??
                accountsToCreate.FindByName(AccountName);

            // Se non lo trovi, crealo
            if (Account is null)
            {
                AccountType accountType = new AccountType(DefaultAccountTypes.None);
                Account = new Accounts(AccountName, userId, accountType);
                accountsToCreate.Add(Account);
            }

            // In ogni caso, aggiungi movimento
            _movementsRepository.Add(movement.ToMovement(
                    OriginAccountId: OriginAccount.Id,
                    DestinationAccountId: Account.Id,
                    UserId: userId));
        }

        _accountRepository.AddRange(accountsToCreate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task RemoveOldMovements(UserId userId, AccountId id, List<SatispayMovements> movements, CancellationToken cancellationToken)
    {
        movements = movements.OrderBy(x => x.Date).ToList();
        DateTime first = movements.First().Date;
        DateTime last = movements.Last().Date;

        await _movementsRepository.RemoveDateRange(userId, id, first, last, cancellationToken);
    }
}