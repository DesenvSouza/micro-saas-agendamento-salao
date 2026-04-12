using MediatR;
using SalonBooking.Application.Features.Auth.Commands.Login;

namespace SalonBooking.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token) : IRequest<LoginResponse>;
