using System.Security.Claims;
using SalonBooking.Domain.Entities;

namespace SalonBooking.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpiration();
    ClaimsPrincipal? ValidateToken(string token);
}
