using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Transazioni.API.Costants;
using Transazioni.Application.Users.LogInUser;
using Transazioni.Application.Users.RegisterUser;

namespace Transazioni.API.Controllers.Authentication;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogIn(
        LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        HttpContext.Response.Cookies.Append(
            CookieNames.AccessToken,
            result.Value.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

        HttpContext.Response.Cookies.Append(
            CookieNames.RefreshToken,
            result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Path = "/api/users/refresh",
                SameSite = SameSiteMode.None,
                Secure = true
            });

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public ActionResult<string?> Refresh()
    {
        //TODO
        string? refreshToken = HttpContext.Request.Cookies[CookieNames.RefreshToken];

        if (refreshToken is null)
        {
            return Unauthorized();
        }

        return Ok(refreshToken);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(CookieNames.AccessToken);
        Response.Cookies.Delete(CookieNames.RefreshToken);
        return Ok();
    }
}
