﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Extensions;
using Transazioni.Application.Fideuram.UploadFideuramMovements;

namespace Transazioni.API.Controllers.Fideuram;

[Authorize]
[ApiController]
[Route("api/Fideuram")]
public class FideuramController : ControllerBase
{
    private readonly ISender _sender;

    public FideuramController(ISender sender)
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
        var command = new UploadFideuramMovementsCommand(file, accountName, userId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
