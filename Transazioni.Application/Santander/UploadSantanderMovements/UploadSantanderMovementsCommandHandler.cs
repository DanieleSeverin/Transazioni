using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Santander.uploadSantanderMovements;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Santander;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Santander.UploadSantanderMovements;

public class UploadSantanderMovementsCommandHandler : ICommandHandler<UploadSantanderMovementsCommand>
{
    private readonly ISantanderReader _santanderReader;
    private readonly IAccountRuleRepository _accountRuleRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMovementsRepository _movementsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadSantanderMovementsCommandHandler(ISantanderReader santanderReader, 
                                                  IAccountRuleRepository accountRuleRepository, 
                                                  IAccountRepository accountRepository, 
                                                  IMovementsRepository movementsRepository, 
                                                  IUnitOfWork unitOfWork)
    {
        _santanderReader = santanderReader;
        _accountRuleRepository = accountRuleRepository;
        _accountRepository = accountRepository;
        _movementsRepository = movementsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UploadSantanderMovementsCommand request, CancellationToken cancellationToken)
    {
        List<SantanderMovements> movements = _santanderReader.ReadMovements(request.File);

        if (!movements.Any())
        {
            Error error = new Error("File.Empty", "The file is empty.");
            return Result.Failure(error);
        }

        UserId userId = new(request.UserId);
        List<AccountRules> rules = await _accountRuleRepository.GetAccountRules(userId, cancellationToken);
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

        await RemoveOldMovements(userId: userId, OriginAccount.Id, movements, cancellationToken);

        foreach (var movement in movements)
        {
            AccountName? AccountName = rules.GetAccountName(movement.DescrizioneOperazione);

            if (AccountName is null)
            {
                var NotAvalaibleAccountName = new AccountName(AccountCostants.NotAvalaibleAccountName);

                // L'account è -NA.
                // Cerca l'account -NA tra quelli da db o creati nuovi
                Accounts? NotAvalaible = accounts.FindByName(NotAvalaibleAccountName) ??
                    accountsToCreate.FindByName(NotAvalaibleAccountName);

                // Se lo trovi, aggiungi movimento
                if (NotAvalaible is not null)
                {
                    _movementsRepository.Add(movement.ToMovement(
                                OriginAccountId: OriginAccount.Id,
                                DestinationAccountId: NotAvalaible.Id,
                                UserId: userId));
                    continue;
                }

                // Se non lo trovi, crealo e aggiungi movimento
                AccountType notAvalaibleAccountType = new AccountType(DefaultAccountTypes.None);
                NotAvalaible = new Accounts(NotAvalaibleAccountName, userId, notAvalaibleAccountType);

                _movementsRepository.Add(movement.ToMovement(
                            OriginAccountId: OriginAccount.Id,
                            DestinationAccountId: NotAvalaible.Id, 
                            UserId: userId));

                accountsToCreate.Add(NotAvalaible);
                continue;
            }

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

    private async Task RemoveOldMovements(UserId userId, AccountId id, List<SantanderMovements> movements, CancellationToken cancellationToken)
    {
        movements = movements.OrderBy(x => x.DataMovimento).ToList();
        DateTime first = movements.First().DataMovimento;
        DateTime last = movements.Last().DataMovimento;

        await _movementsRepository.RemoveDateRange(userId, id, first, last, cancellationToken);
    }
}
