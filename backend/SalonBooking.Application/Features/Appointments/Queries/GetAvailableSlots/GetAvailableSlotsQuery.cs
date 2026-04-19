using MediatR;

namespace SalonBooking.Application.Features.Appointments.Queries.GetAvailableSlots;

public record GetAvailableSlotsQuery(
    Guid ProfessionalId,
    Guid ServiceId,
    DateOnly Date) : IRequest<IEnumerable<DateTime>>;
