using SalonBooking.Domain.Events.Base;

namespace SalonBooking.Domain.Events;

public record AppointmentCancelledEvent(
    Guid AppointmentId,
    Guid ClientId,
    Guid EstablishmentId,
    bool CancelledByClient) : DomainEvent;
