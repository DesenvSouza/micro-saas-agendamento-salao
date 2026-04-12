using SalonBooking.Domain.Events.Base;

namespace SalonBooking.Domain.Events;

public record AppointmentCreatedEvent(
    Guid AppointmentId,
    Guid ClientId,
    Guid EstablishmentId,
    Guid ProfessionalId,
    DateTime StartTime) : DomainEvent;
