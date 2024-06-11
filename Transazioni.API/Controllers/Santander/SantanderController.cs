using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Santander.UploadSantanderMovements;

namespace Transazioni.API.Controllers.Santander;

[Authorize]
[ApiController]
[Route("api/Santander")]
public class SantanderController : ControllerBase
{
    private readonly ISender _sender;

    public SantanderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(
        IFormFile file,
        [FromQuery] string accountName,
        CancellationToken cancellationToken)
    {
        Guid userId = User.GetUserId();
        var command = new UploadSantanderMovementsCommand(file, accountName, userId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
