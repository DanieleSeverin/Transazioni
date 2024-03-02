using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.Reporting.GetAccountsBalance;

namespace Transazioni.API.Controllers.Reporting;

[Route("api/reporting")]
[ApiController]
public class ReportingController : ControllerBase
{
    private readonly ISender _sender;

    public ReportingController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("accounts-balance")]
    public async Task<IActionResult> GetAccountsBalance(CancellationToken cancellationToken)
    {
        var query = new GetAccountsBalanceQuery();

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
