using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Services;

public record ServiceDto(
    Guid Id,
    Guid EstablishmentId,
    string Name,
    string? Description,
    ServiceCategory Category,
    string CategoryName,
    int DurationMinutes,
    int SlotIntervalMinutes,
    decimal Price,
    string Currency,
    bool IsActive,
    DateTime CreatedAt);
