using MediatR;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Services.Commands.UpdateService;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    ServiceCategory Category,
    int DurationMinutes,
    decimal Price,
    string? Description = null,
    int? SlotIntervalMinutes = null) : IRequest<ServiceDto>;
