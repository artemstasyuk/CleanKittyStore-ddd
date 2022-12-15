﻿using CatStore.Application.Common.Interfaces.Auth;
using CatStore.Application.Common.Interfaces.Persistence;
using CatStore.Application.MediaR.Authentication.Common;
using CatStore.Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace CatStore.Application.MediaR.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(query.Email) is not { } user)
            return Errors.Authentication.InvalidCredentials;

        if (user.Password != query.Password)
            return new[] { Errors.Authentication.InvalidCredentials };

        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthenticationResult(){User = user,Token = token};
    }
}