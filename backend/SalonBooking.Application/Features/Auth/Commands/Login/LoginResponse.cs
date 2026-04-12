namespace SalonBooking.Application.Features.Auth.Commands.Login;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string Role,
    Guid UserId,
    string Email,
    string Name);
