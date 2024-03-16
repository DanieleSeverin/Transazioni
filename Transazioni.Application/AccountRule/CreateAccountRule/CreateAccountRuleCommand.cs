using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.AccountRule;

namespace Transazioni.Application.AccountRule.CreateAccountRule;

public sealed record CreateAccountRuleCommand(Guid AccountId, string query) : ICommand<AccountRules>;
