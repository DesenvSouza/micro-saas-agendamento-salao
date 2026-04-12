namespace SalonBooking.Application.Features.Auth.Queries.GetCurrentUser;

public record CurrentUserDto(
    Guid Id,
    string Email,
    string Role,
    string Name,
    bool EmailVerified,
    bool IsActive);
