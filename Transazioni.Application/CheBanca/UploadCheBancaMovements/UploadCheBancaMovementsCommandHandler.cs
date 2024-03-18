using DocumentFormat.OpenXml.Bibliography;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;
using Transazioni.Domain.CheBanca;
using Transazioni.Domain.Movement;

namespace Transazioni.Application.CheBanca.UploadCheBancaMovements;

public class UploadCheBancaMovementsCommandHandler : ICommandHandler<UploadCheBancaMovementsCommand>
{
    private readonly ICheBancaReader _cheBancaReader;
    private readonly IAccountRuleRepository _accountRuleRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMovementsRepository _movementsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadCheBancaMovementsCommandHandler(ICheBancaReader cheBancaReader,
                                                 IAccountRuleRepository accountRuleRepository,
                                                 IAccountRepository accountRepository,
                                                 IMovementsRepository movementsRepository,
                                                 IUnitOfWork unitOfWork)
    {
        _cheBancaReader = cheBancaReader;
        _accountRuleRepository = accountRuleRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _movementsRepository = movementsRepository;
    }

    public async Task<Result> Handle(UploadCheBancaMovementsCommand request, CancellationToken cancellationToken)
    {
        List<CheBancaMovements> movements = _cheBancaReader.ReadMovements(request.File);

        if (!movements.Any())
        {
            Error error = new Error("File.Empty", "The file is empty.");
            return Result.Failure(error);
        }

        List<AccountRules> rules = await _accountRuleRepository.GetAccountRules(cancellationToken);
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
            if (!movement.HasAmount())
            {
                continue;
            }

            AccountName? AccountName = rules.GetAccountName(movement.Tipologia);

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
                            DestinationAccountId: NotAvalaible.Id));

                    continue;
                }

                // Se non lo trovi, crealo e aggiungi movimento
                NotAvalaible = new Accounts(NotAvalaibleAccountName, isPatrimonial: false);

                _movementsRepository.Add(movement.ToMovement(
                            OriginAccountId: OriginAccount.Id,
                            DestinationAccountId: NotAvalaible.Id));

                accountsToCreate.Add(NotAvalaible);
                continue;
            }

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

    private async Task RemoveOldMovements(AccountId id, List<CheBancaMovements> movements, CancellationToken cancellationToken)
    {
        movements = movements.OrderBy(x => x.DataContabile).ToList();
        DateTime first = movements.First().DataContabile;
        DateTime last = movements.Last().DataContabile;

        await _movementsRepository.RemoveDateRange(id, first, last, cancellationToken);
    }
}
