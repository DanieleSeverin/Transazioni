using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Controllers.Account;
using Transazioni.Application.Movement.CreateMovement;
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
