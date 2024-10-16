﻿using Transazioni.Application.Abstractions.Authentication;
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
                                   IPasswordEncrypter passwordEncrypter,
                                   IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _passwordEncrypter = passwordEncrypter;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LogInResponse>> Handle(
        LogInUserCommand request,
        CancellationToken cancellationToken)
    {
        Email email = new Email(request.Email);

        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LogInResponse>(UserErrors.InvalidCredentials);
        }

        (bool verified, _) = _passwordEncrypter.Check(user.Password.Value, request.Password);

        if (!verified)
        {
            return Result.Failure<LogInResponse>(UserErrors.InvalidCredentials);
        }

        var accessTokenResult = _jwtProvider.GenerateAccessToken(user);
        if(accessTokenResult.IsFailure)
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