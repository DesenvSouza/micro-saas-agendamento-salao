using MediatR;

namespace SalonBooking.Application.Features.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid ProfessionalId,
    Guid ServiceId,
    DateTime StartTime,
    string? ClientNotes = null) : IRequest<AppointmentDto>;
