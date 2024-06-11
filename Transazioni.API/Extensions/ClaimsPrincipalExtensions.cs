using System.Security.Claims;
using Transazioni.API.Exceptions;

namespace Transazioni.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null)
        {
            throw new UserIdNotFoundException();
        }

        return Guid.Parse(userId);
    }
}
