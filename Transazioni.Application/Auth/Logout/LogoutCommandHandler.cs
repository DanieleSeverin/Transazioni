using Transazioni.Application.Abstractions.Authentication;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Application.Users.LogInUser;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Auth.Logout;

internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IUserRepository userRepository, 
                                IJwtProvider jwtProvider, 
                                IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        Result<Guid> userIdResult = _jwtProvider.GetUserIdFromJwt(request.Token);

        if (userIdResult.IsFailure)
        {
            return Result.Failure<LogInResponse>(userIdResult.Error!);
        }

        UserId userId = new(userIdResult.Value);
        User? user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LogInResponse>(UserErrors.NotFound);
        }

        foreach (var userToken in user.RefreshTokens.Where(t => t.Valid))
        {
            userToken.Invalidate("Logout");
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
