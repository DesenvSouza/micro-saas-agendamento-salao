using MediatR;

namespace SalonBooking.Application.Features.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(Guid AppointmentId) : IRequest<AppointmentDto>;
