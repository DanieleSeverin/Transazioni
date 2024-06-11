using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Satispay.UploadSatispayMovements;

namespace Transazioni.API.Controllers.Satispay;

[Authorize]
[ApiController]
[Route("api/Satispay")]

public class SatispayController : ControllerBase
{ 
     private readonly ISender _sender;

    public SatispayController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("import")]
    public async Task<IActionResult> Register(
        IFormFile file,
        [FromQuery] string accountName,
        CancellationToken cancellationToken)
    {
        Guid userId = User.GetUserId();
        var command = new UploadSatispayMovementsCommand(file, accountName, userId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}

