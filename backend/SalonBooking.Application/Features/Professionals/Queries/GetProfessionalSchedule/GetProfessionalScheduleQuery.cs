using MediatR;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalSchedule;

public record GetProfessionalScheduleQuery(Guid ProfessionalId) : IRequest<IEnumerable<WorkingScheduleDto>>;
