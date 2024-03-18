using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Controllers.Account;
using Transazioni.Application.Movement.CreateMovement;
using Transazioni.Application.Movement.GetMovements;
using Transazioni.Domain.Abstractions;

namespace Transazioni.API.Controllers.Movement;

[Route("api/Movements")]
[ApiController]
public class MovementsController : ControllerBase
{
    private readonly ISender _sender;

    public MovementsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet()]
    public async Task<IActionResult> GetMovements(CancellationToken cancellationToken)
    {
        var query = new GetMovementsQuery();
        var getMovementsResult = await _sender.Send(query, cancellationToken);

        if (getMovementsResult.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, getMovementsResult.Error);
        }

        return Ok(getMovementsResult);
    }

    [HttpPost()]
    public async Task<IActionResult> CreateMovement([FromBody] CreateMovementRequest Request, CancellationToken cancellationToken)
    {
        var command = new CreateMovementCommand(Request);
        var createMovementResult = await _sender.Send(command, cancellationToken);

        if (createMovementResult.IsFailure)
        {
            return BadRequest(createMovementResult.Error);
        }

        return Ok();
    }
}
