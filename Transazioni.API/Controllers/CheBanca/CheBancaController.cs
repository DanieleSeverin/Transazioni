using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.CheBanca.UploadCheBancaMovements;

namespace Transazioni.API.Controllers.CheBanca;

[ApiController]
[Route("api/CheBanca")]
public class CheBancaController : ControllerBase
{
    private readonly ISender _sender;

    public CheBancaController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("import")]
    public async Task<IActionResult> Register(
        IFormFile file,
        [FromQuery] string accountName,
        CancellationToken cancellationToken)
    {
        var command = new UploadCheBancaMovementsCommand(file, accountName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
