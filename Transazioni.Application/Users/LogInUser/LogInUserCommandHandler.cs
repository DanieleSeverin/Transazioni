using Transazioni.Application.Abstractions.Authentication;
using Transazioni.Application.Abstractions.Messaging;
using Transazioni.Domain.Abstractions;
using Transazioni.Domain.Users;

namespace Transazioni.Application.Users.LogInUser;

internal sealed class LogInUserCommandHandler : ICommandHandler<LogInUserCommand, LogInResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUnitOfWork _unitOfWork;

    public LogInUserCommandHandler(IUserRepository userRepository,
                                   IJwtProvider jwtProvider,
                                   IPasswordEncrypter passwordEncrypter)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task<Result<LogInResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        Email email = new Email(request.Email);

        User? user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return Result.Failure<LogInResponse>(UserErrors.InvalidCredentials);
        }

        (bool verified, bool needsUpgrade) =
            _passwordEncrypter.Check(user.Password.Value, request.Password);

        if (!verified)
        {
            return Result.Failure<LogInResponse>(UserErrors.InvalidCredentials);
        }

        if (needsUpgrade)
        {
            // TODO
        }

        var accessTokenResult = _jwtProvider.GenerateAccessToken(user);
        var refreshTokenResult = _jwtProvider.GenerateRefreshToken(user);

        user.AddRefreshToken(refreshTokenResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new LogInResponse(accessTokenResult.Value.Value, refreshTokenResult.Value.Value);
    }
}