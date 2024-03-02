using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.Satispay.UploadSatispayMovements;

namespace Transazioni.API.Controllers.Satispay;
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
        var command = new UploadSatispayMovementsCommand(file, accountName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}

