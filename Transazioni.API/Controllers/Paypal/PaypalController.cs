using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Paypal.UploadPaypalMovements;

namespace Transazioni.API.Controllers.Paypal;

[Authorize]
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
        Guid userId = User.GetUserId();
        var command = new UploadPaypalMovementsCommand(file, accountName, userId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
