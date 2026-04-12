using MediatR;

namespace SalonBooking.Application.Features.Professionals.Commands.DeleteTimeOff;

public record DeleteTimeOffCommand(Guid ProfessionalId, Guid TimeOffId) : IRequest;
