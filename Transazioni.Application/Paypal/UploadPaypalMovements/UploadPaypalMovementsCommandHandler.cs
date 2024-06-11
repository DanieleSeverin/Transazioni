using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.CheBanca;
using Transazioni.Domain.Movement;
using Transazioni.Domain.Paypal;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Paypal.UploadPaypalMovements;

public class UploadPaypalMovementsCommandHandler : ICommandHandler<UploadPaypalMovementsCommand>
{
    private readonly IPaypalReader _paypalReader;
    private readonly IAccountRuleRepository _accountRuleRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMovementsRepository _movementsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadPaypalMovementsCommandHandler(IPaypalReader paypalReader,
                                      IAccountRuleRepository accountRuleRepository,
                                      IAccountRepository accountRepository,
                                      IMovementsRepository movementsRepository,
                                      IUnitOfWork unitOfWork)
    {
        _paypalReader = paypalReader;
        _accountRuleRepository = accountRuleRepository;
        _accountRepository = accountRepository;
        _movementsRepository = movementsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UploadPaypalMovementsCommand request, CancellationToken cancellationToken)
    {
        List<PaypalMovements> movements = _paypalReader.ReadMovements(request.File);

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
            OriginAccount = new Accounts(originAccountName, isPatrimonial: true, userId);
            accountsToCreate.Add(OriginAccount);
        }

        await RemoveOldMovements(userId: userId, OriginAccount.Id, movements, cancellationToken);

        foreach (var movement in movements)
        {

            AccountName? AccountName = rules.GetAccountName(
                movement.Nome ?? movement.NomeBanca ?? movement.Descrizione
                ?? throw new InvalidPaypalMovementException("Nome, NomeBanca and Descrizione can't be all null."));

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
                NotAvalaible = new Accounts(NotAvalaibleAccountName, isPatrimonial: false, userId);

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
                Account = new Accounts(AccountName, isPatrimonial: false, userId);
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

    private async Task RemoveOldMovements(UserId userId, AccountId id, List<PaypalMovements> movements, CancellationToken cancellationToken)
    {
        movements = movements.OrderBy(x => x.Data).ToList();
        DateTime first = movements.First().Data;
        DateTime last = movements.Last().Data;

        await _movementsRepository.RemoveDateRange(userId, id, first, last, cancellationToken);
    }
}
