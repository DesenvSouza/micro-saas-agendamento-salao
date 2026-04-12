using MediatR;
using Microsoft.Extensions.Configuration;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Application.Features.Auth.Commands.Login;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Exceptions;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(request.Token, cancellationToken)
            ?? throw new BusinessRuleViolationException("Token de atualização inválido.");

        if (!existingToken.IsActive)
            throw new BusinessRuleViolationException("Token de atualização expirado ou revogado.");

        var user = existingToken.User;
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var newRefreshExpiration = _jwtTokenService.GetRefreshTokenExpiration();

        existingToken.Revoke(newRefreshTokenValue);

        var newRefreshToken = Domain.Entities.RefreshToken.Create(
            user.Id, newRefreshTokenValue, newRefreshExpiration);
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationMinutes"] ?? "60");
        var name = GetUserName(user);

        return new LoginResponse(
            newAccessToken,
            newRefreshTokenValue,
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
