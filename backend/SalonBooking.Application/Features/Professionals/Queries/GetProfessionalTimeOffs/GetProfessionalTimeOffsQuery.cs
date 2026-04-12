using MediatR;
using SalonBooking.Application.Features.Professionals;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalTimeOffs;

public record GetProfessionalTimeOffsQuery(Guid ProfessionalId) : IRequest<IEnumerable<TimeOffDto>>;
