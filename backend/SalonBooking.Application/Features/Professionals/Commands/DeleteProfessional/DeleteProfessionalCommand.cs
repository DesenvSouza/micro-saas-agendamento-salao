using MediatR;

namespace SalonBooking.Application.Features.Professionals.Commands.DeleteProfessional;

public record DeleteProfessionalCommand(Guid Id) : IRequest;
