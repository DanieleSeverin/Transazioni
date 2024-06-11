using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Movement.CreateMovement;
using Transazioni.Application.Movement.GetMovements;
using Transazioni.Domain.Utilities.Ordering;
using Transazioni.Domain.Utilities.Pagination;

namespace Transazioni.API.Controllers.Movement;

[Authorize]
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
    public async Task<IActionResult> GetMovements(
        [FromQuery] Guid? originAccountId = null,
        [FromQuery] Guid? destinationAccountId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? category = null,
        [FromQuery] decimal? amountGreaterThan = null,
        [FromQuery] decimal? amountLowerThan = null,
        [FromQuery] string? currency = null,
        [FromQuery] bool? imported = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool? ascending = null,
        CancellationToken cancellationToken = default)
    {
        GetMovementFilter filters = new GetMovementFilter(
            originAccountId: originAccountId,
            destinationAccountId: destinationAccountId,
            startDate: startDate,
            endDate: endDate,
            category: category,
            amountGreaterThan: amountGreaterThan,
            amountLowerThan: amountLowerThan,
            currency: currency,
            imported: imported
        );

        PaginationConfigurations paginationConfigurations = new PaginationConfigurations(
            pageNumber: pageNumber, pageSize: pageSize);

        OrderingConfigurations orderingConfigurations = new OrderingConfigurations(
            propertyName: orderBy, ascending: ascending ?? true);

        Guid userId = User.GetUserId();
        var query = new GetMovementsQuery(userId, filters, paginationConfigurations, orderingConfigurations);
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
        Guid userId = User.GetUserId();
        var command = new CreateMovementCommand(Request, userId);
        var createMovementResult = await _sender.Send(command, cancellationToken);

        if (createMovementResult.IsFailure)
        {
            return BadRequest(createMovementResult.Error);
        }

        return Ok();
    }
}
