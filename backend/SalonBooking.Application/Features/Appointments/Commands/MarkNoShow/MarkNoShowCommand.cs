using MediatR;

namespace SalonBooking.Application.Features.Appointments.Commands.MarkNoShow;

public record MarkNoShowCommand(Guid AppointmentId) : IRequest<AppointmentDto>;
