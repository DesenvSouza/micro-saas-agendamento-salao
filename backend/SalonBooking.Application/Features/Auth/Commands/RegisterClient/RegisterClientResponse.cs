namespace SalonBooking.Application.Features.Auth.Commands.RegisterClient;

public record RegisterClientResponse(Guid Id, string Email, string FullName, string Message);
