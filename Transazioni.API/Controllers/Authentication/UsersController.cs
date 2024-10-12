using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Transazioni.API.Costants;
using Transazioni.Application.Auth.Logout;
using Transazioni.Application.Auth.RefreshJwt;
using Transazioni.Application.Users.LogInUser;
using Transazioni.Application.Users.RegisterUser;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Tokens;

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
            result.Value.AccessToken.Value,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

        HttpContext.Response.Cookies.Append(
            CookieNames.RefreshToken,
            result.Value.RefreshToken.Value,
            new CookieOptions
            {
                HttpOnly = true,
                Path = "/api/users/refresh",
                SameSite = SameSiteMode.None,
                Secure = true
            });

        AuthResponse response = new(result.Value.AccessToken.ExpireAt, result.Value.RefreshToken.ExpireAt);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        string? accessToken = HttpContext.Request.Cookies[CookieNames.AccessToken];
        string? refreshToken = HttpContext.Request.Cookies[CookieNames.RefreshToken];

        if(string.IsNullOrWhiteSpace(accessToken))
        {
            return StatusCode(403, Result.Failure(JwtErrors.MissingJwt));
        }

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return StatusCode(403, Result.Failure(JwtErrors.MissingRefreshToken));
        }

        var command = new RefreshJwtCommand(AccessToken: accessToken, RefreshToken: refreshToken);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(403, result.Error);
        }

        HttpContext.Response.Cookies.Append(
            CookieNames.AccessToken,
            result.Value.AccessToken.Value,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

        HttpContext.Response.Cookies.Append(
            CookieNames.RefreshToken,
            result.Value.RefreshToken.Value,
            new CookieOptions
            {
                HttpOnly = true,
                Path = "/api/users/refresh",
                SameSite = SameSiteMode.None,
                Secure = true
            });

        AuthResponse response = new(result.Value.AccessToken.ExpireAt, result.Value.RefreshToken.ExpireAt);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        string? accessToken = HttpContext.Request.Cookies[CookieNames.AccessToken];

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return BadRequest(Result.Failure(JwtErrors.MissingJwt));
        }

        var command = new LogoutCommand(accessToken);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(500, result.Error);
        }

        Response.Cookies.Delete(CookieNames.AccessToken);
        Response.Cookies.Delete(CookieNames.RefreshToken);
        return Ok();
    }

    [HttpGet("IsLoggedIn")]
    public IActionResult IsLoggedIn()
    {
        return Ok();
    }
}
