using MediatR;

namespace SalonBooking.Application.Features.Auth.Commands.RegisterClient;

public record RegisterClientCommand(
    string Email,
    string Password,
    string FullName,
    string? Phone) : IRequest<RegisterClientResponse>;
