using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Account.CreateAccount;
using Transazioni.Application.Account.GetAccounts;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Account;

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
        Guid userId = User.GetUserId();
        var query = new GetAccountsQuery(userId);

        var accountsResult = await _sender.Send(query, cancellationToken);

        if(accountsResult.IsFailure)
        {
            return StatusCode(500, accountsResult.Error);
        }

        var dtos = accountsResult.Value
            .Where(account => onlyPatrimonial == true ? account.AccountType.Value == DefaultAccountTypes.Bank : true)
            .Select(account => new AccountDto(account.Id.Value, account.AccountName.Value, account.AccountType.Value))
            .OrderBy(account => account.AccountName);

        return Ok(Result.Success(dtos));
    }

    // TODO: i conti dovrebbero avere un tipo (banca, stipendio, clienti, ecc.)
    // Prima di salvare, si dovrebbe fare una chiamata per vedere se esiste già un conto con lo stesso nome, ma di tipo diverso.
    // Se si, chiedere se si vuole aggiornare. Se esiste già ed è identico dare errore.
    [HttpPost()]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        Guid userId = User.GetUserId();
        var command = new CreateAccountCommand(
            AccountName: request.AccountName,
            AccountType: request.AccountType,
            UserId: userId);

        var createAccountsResult = await _sender.Send(command, cancellationToken);

        if(createAccountsResult.IsFailure)
        {
            return Conflict(createAccountsResult.Error);
        }

        var newAccountDto = new AccountDto(
            createAccountsResult.Value.Id.Value, 
            createAccountsResult.Value.AccountName.Value, 
            createAccountsResult.Value.AccountType.Value);

        return Ok(Result.Success(newAccountDto));
    }
}
