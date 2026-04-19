using MediatR;

namespace SalonBooking.Application.Features.Appointments.Queries.GetClientAppointments;

public record GetClientAppointmentsQuery(bool OnlyUpcoming = false) : IRequest<IEnumerable<AppointmentDto>>;
