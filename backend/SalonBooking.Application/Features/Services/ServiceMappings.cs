using SalonBooking.Domain.Entities;

namespace SalonBooking.Application.Features.Services;

public static class ServiceMappings
{
    public static ServiceDto ToDto(this Service service) => new(
        service.Id,
        service.EstablishmentId,
        service.Name,
        service.Description,
        service.Category,
        service.Category.ToString(),
        service.DurationMinutes,
        service.SlotIntervalMinutes,
        service.Price,
        service.Currency,
        service.IsActive,
        service.CreatedAt);
}
