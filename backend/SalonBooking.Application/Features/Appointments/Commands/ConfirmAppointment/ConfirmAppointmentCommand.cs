using MediatR;

namespace SalonBooking.Application.Features.Appointments.Commands.ConfirmAppointment;

public record ConfirmAppointmentCommand(Guid AppointmentId) : IRequest<AppointmentDto>;
