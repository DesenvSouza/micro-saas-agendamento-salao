using MediatR;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Application.Features.Appointments.Queries.GetEstablishmentAppointments;

public record GetEstablishmentAppointmentsQuery(
    DateTime? From = null,
    DateTime? To = null,
    AppointmentStatus? Status = null,
    Guid? ProfessionalId = null) : IRequest<IEnumerable<AppointmentDto>>;
