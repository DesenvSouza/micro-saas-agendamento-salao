using SalonBooking.Domain.Events.Base;

namespace SalonBooking.Domain.Events;

public record AppointmentConfirmedEvent(
    Guid AppointmentId,
    Guid ClientId,
    Guid EstablishmentId) : DomainEvent;
