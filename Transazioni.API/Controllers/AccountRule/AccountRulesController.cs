using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.AccountRule.CreateAccountRule;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;
using Transazioni.Domain.AccountRule;

namespace Transazioni.API.Controllers.AccountRule;

[Authorize]
[Route("api/AccountRules")]
[ApiController]
public class AccountRulesController : ControllerBase
{
    private readonly ISender _sender;

    public AccountRulesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost()]
    public async Task<IActionResult> CreateAccountRule([FromBody] CreateAccountRuleRequest request, CancellationToken cancellationToken)
    {
        Guid userId = User.GetUserId();

        var command = new CreateAccountRuleCommand(request.AccountId, request.Query, userId);
        var createAccountRuleResult = await _sender.Send(command, cancellationToken);

        if (createAccountRuleResult.IsFailure && createAccountRuleResult.Error == AccountErrors.NotFound)
        {
            return NotFound(createAccountRuleResult.Error);
        }

        if (createAccountRuleResult.IsFailure && createAccountRuleResult.Error == AccountRuleErrors.AlreadyExists)
        {
            return Conflict(createAccountRuleResult.Error);
        }

        return Ok(Result.Success(createAccountRuleResult.Value.Id));
    }
}
