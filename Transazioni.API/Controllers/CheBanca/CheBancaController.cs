﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.CheBanca.UploadCheBancaMovements;

namespace Transazioni.API.Controllers.CheBanca;

[Authorize]
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
        Guid userId = User.GetUserId();
        var command = new UploadCheBancaMovementsCommand(file, accountName, userId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
