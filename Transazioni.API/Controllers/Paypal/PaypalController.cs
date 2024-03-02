using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transazioni.Application.Paypal.UploadPaypalMovements;

namespace Transazioni.API.Controllers.Paypal;

[ApiController]
[Route("api/Paypal")]
public class PaypalController : ControllerBase
{
    private readonly ISender _sender;

    public PaypalController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("import")]
    public async Task<IActionResult> Register(
        IFormFile file,
        [FromQuery] string accountName,
        CancellationToken cancellationToken)
    {
        var command = new UploadPaypalMovementsCommand(file, accountName);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
