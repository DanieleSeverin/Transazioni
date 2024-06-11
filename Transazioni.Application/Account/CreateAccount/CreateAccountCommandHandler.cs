using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Account.CreateAccount;

public class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, Accounts>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IAccountRepository accountRepository, 
                                       IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Accounts>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        UserId userId = new(request.UserId);
        AccountName alreadyExistsAccountName = new AccountName(request.AccountName);
        Accounts? alreadyExistsAccount = await _accountRepository.GetByName(userId, alreadyExistsAccountName, cancellationToken);

        if(alreadyExistsAccount is not null)
        {
            return Result.Failure<Accounts>(AccountErrors.AlreadyExists);
        }

        AccountName accountName = new AccountName(request.AccountName);
        Accounts account = new Accounts(accountName, request.IsPatrimonial, userId);
        _accountRepository.Add(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(account);
    }
}
