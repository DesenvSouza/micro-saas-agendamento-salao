using MediatR;
using SalonBooking.Application.Features.Professionals;

namespace SalonBooking.Application.Features.Professionals.Commands.AddTimeOff;

public record AddTimeOffCommand(
    Guid ProfessionalId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string? Reason) : IRequest<TimeOffDto>;
