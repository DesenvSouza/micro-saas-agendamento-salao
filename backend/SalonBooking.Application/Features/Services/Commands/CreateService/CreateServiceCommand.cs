using MediatR;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Services.Commands.CreateService;

public record CreateServiceCommand(
    string Name,
    ServiceCategory Category,
    int DurationMinutes,
    decimal Price,
    string? Description = null,
    int? SlotIntervalMinutes = null) : IRequest<ServiceDto>;
