using MediatR;

namespace SalonBooking.Application.Features.Appointments.Commands.CompleteAppointment;

public record CompleteAppointmentCommand(Guid AppointmentId) : IRequest<AppointmentDto>;
