using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.Account.CreateAccount;
using Transazioni.Application.Account.GetAccounts;
using Transazioni.Domain.Abstractions;

namespace Transazioni.API.Controllers.Account;

[Authorize]
[Route("api/Accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts([FromQuery] bool? onlyPatrimonial, CancellationToken cancellationToken)
    {
        var query = new GetAccountsQuery();

        var accountsResult = await _sender.Send(query, cancellationToken);

        if(accountsResult.IsFailure)
        {
            return StatusCode(500, accountsResult.Error);
        }

        var dtos = accountsResult.Value
            .Where(account => onlyPatrimonial == true ? account.IsPatrimonial : true)
            .Select(account => new AccountDto(account.Id.Value, account.AccountName.Value, account.IsPatrimonial))
            .OrderBy(account => account.AccountName);

        return Ok(Result.Success(dtos));
    }

    [HttpPost()]
    public async Task<IActionResult> CreatePatrimonialAccount([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAccountCommand(request.AccountName, request.IsPatrimonial);
        var createAccountsResult = await _sender.Send(command, cancellationToken);

        if(createAccountsResult.IsFailure)
        {
            return Conflict(createAccountsResult.Error);
        }

        var newAccountDto = new AccountDto(
            createAccountsResult.Value.Id.Value, 
            createAccountsResult.Value.AccountName.Value, 
            createAccountsResult.Value.IsPatrimonial);

        return Ok(Result.Success(newAccountDto));
    }
}
