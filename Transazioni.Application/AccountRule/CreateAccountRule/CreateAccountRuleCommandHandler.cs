using System.Collections.Generic;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;

namespace Transazioni.Application.AccountRule.CreateAccountRule;

public class CreateAccountRuleCommandHandler : ICommandHandler<CreateAccountRuleCommand, AccountRules>
{
    private readonly IAccountRuleRepository _accountRuleRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountRuleCommandHandler(IAccountRuleRepository accountRuleRepository,
                                           IAccountRepository accountRepository,
                                           IUnitOfWork unitOfWork)
    {
        _accountRuleRepository = accountRuleRepository;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AccountRules>> Handle(CreateAccountRuleCommand request, CancellationToken cancellationToken)
    {
        // Check if account exists
        AccountId accountId = new(request.AccountId);
        Accounts? account = await _accountRepository.GetById(accountId, cancellationToken);

        if (account is null)
        {
            return Result.Failure<AccountRules>(AccountErrors.NotFound);
        }

        // Check if rule already exists
        List<AccountRules> accountRules = await _accountRuleRepository.GetAccountRules(cancellationToken);
        AccountRules? accountRule = accountRules.Find(
            rule => rule.RuleContains.Value == request.query && 
                    rule.AccountName.Value == account.AccountName.Value);

        if(accountRule is not null)
        {
            return Result.Failure<AccountRules>(AccountRuleErrors.AlreadyExists);
        }

        // Create rule
        RuleContains ruleContains = new(request.query);
        AccountRules newRule = new(ruleContains, account.AccountName);

        _accountRuleRepository.Add(newRule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(newRule);
    }
}
