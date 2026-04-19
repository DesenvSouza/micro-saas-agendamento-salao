using MediatR;

namespace SalonBooking.Application.Features.Appointments.Queries.GetCalendarAppointments;

public record GetCalendarAppointmentsQuery(
    DateTime From,
    DateTime To,
    Guid? ProfessionalId = null) : IRequest<IEnumerable<CalendarEventDto>>;
