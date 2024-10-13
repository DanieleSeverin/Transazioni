using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.Reporting.AccountsBalance.GetAccountsBalance;
using Transazioni.Application.Reporting.GetCosts;
using Transazioni.Application.Reporting.GetRevenue;

namespace Transazioni.API.Controllers.Reporting;

[Authorize]
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
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        return Ok(result);
    }

    [HttpGet("monthly-accounts-balance")]
    public async Task<IActionResult> GetMonthlyAccountsBalance(CancellationToken cancellationToken)
    {
        var query = new GetMonthlyCumulativeBalanceQuery();

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        return Ok(result);
    }

    [HttpGet("costs")]
    public async Task<IActionResult> GetCosts(CancellationToken cancellationToken)
    {
        var query = new GetCostsQuery();

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        return Ok(result);
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenue(CancellationToken cancellationToken)
    {
        var query = new GetRevenueQuery();

        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result.Error);
        }

        return Ok(result);
    }
}
