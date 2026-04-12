using MediatR;
using Microsoft.Extensions.Configuration;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new BusinessRuleViolationException("E-mail ou senha inválidos.");

        if (!user.IsActive)
            throw new BusinessRuleViolationException("Conta inativa. Entre em contato com o suporte.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new BusinessRuleViolationException("E-mail ou senha inválidos.");

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var refreshExpiration = _jwtTokenService.GetRefreshTokenExpiration();

        var refreshToken = SalonBooking.Domain.Entities.RefreshToken.Create(user.Id, refreshTokenValue, refreshExpiration);
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
        var name = GetUserName(user);

        return new LoginResponse(
            accessToken,
            refreshTokenValue,
            expirationMinutes * 60,
            user.Role.ToString(),
            user.Id,
            user.Email,
            name);
    }

    private static string GetUserName(User user) => user switch
    {
        Client c => c.FullName,
        Establishment e => e.TradeName,
        _ => user.Email
    };
}
