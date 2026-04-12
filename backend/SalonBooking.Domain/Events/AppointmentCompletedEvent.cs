using SalonBooking.Domain.Events.Base;
using SalonBooking.Domain.ValueObjects;

namespace SalonBooking.Domain.Events;

public record AppointmentCompletedEvent(
    Guid AppointmentId,
    Guid ClientId,
    Guid EstablishmentId,
    Money Price) : DomainEvent;
