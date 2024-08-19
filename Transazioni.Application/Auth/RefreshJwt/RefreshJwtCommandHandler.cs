using Transazioni.Application.Abstractions.Authentication;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Users.LogInUser;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Tokens;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Auth.RefreshJwt;

internal sealed class RefreshJwtCommandHandler : ICommandHandler<RefreshJwtCommand, LogInResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshJwtCommandHandler(IUserRepository userRepository, 
                                    IJwtProvider jwtProvider, 
                                    IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LogInResponse>> Handle(RefreshJwtCommand request, CancellationToken cancellationToken)
    {
        Result<Guid> userIdResult = _jwtProvider.GetUserIdFromJwt(request.AccessToken);

        if(userIdResult.IsFailure)
        {
            return Result.Failure<LogInResponse>(userIdResult.Error!);
        }

        UserId userId = new(userIdResult.Value);
        User? user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LogInResponse>(UserErrors.NotFound);
        }

        RefreshToken? token = user.RefreshTokens.FirstOrDefault(t => t.Value == request.RefreshToken);

        if(token is null)
        {
            return Result.Failure<LogInResponse>(JwtErrors.NotFound);
        }

        if(!token.Valid)
        {
            // Someone tried to use an invalid refresh token!
            // Set all user refresh token as invalid
            foreach(var userToken in user.RefreshTokens.Where(t => t.Valid)) 
            {
                userToken.Invalidate("Invalid RefreshToken used.");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Failure<LogInResponse>(JwtErrors.Invalid);
        }

        var accessTokenResult = _jwtProvider.GenerateAccessToken(user);
        if (accessTokenResult.IsFailure)
        {
            return Result.Failure<LogInResponse>(accessTokenResult.Error!);
        }

        var refreshTokenResult = _jwtProvider.GenerateRefreshToken(user);
        if (refreshTokenResult.IsFailure)
        {
            return Result.Failure<LogInResponse>(refreshTokenResult.Error!);
        }

        user.AddRefreshToken(refreshTokenResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LogInResponse(accessTokenResult.Value, refreshTokenResult.Value);
    }
}
