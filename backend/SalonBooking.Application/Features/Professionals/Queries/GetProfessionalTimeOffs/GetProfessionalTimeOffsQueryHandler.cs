using MediatR;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Professionals.Queries.GetProfessionalTimeOffs;

public class GetProfessionalTimeOffsQueryHandler
    : IRequestHandler<GetProfessionalTimeOffsQuery, IEnumerable<TimeOffDto>>
{
    private readonly ITimeOffRepository _timeOffRepository;

    public GetProfessionalTimeOffsQueryHandler(ITimeOffRepository timeOffRepository) =>
        _timeOffRepository = timeOffRepository;

    public async Task<IEnumerable<TimeOffDto>> Handle(
        GetProfessionalTimeOffsQuery request,
        CancellationToken cancellationToken)
    {
        var timeOffs = await _timeOffRepository.GetByProfessionalAsync(
            request.ProfessionalId, cancellationToken);

        return timeOffs
            .OrderBy(t => t.StartDateTime)
            .Select(t => t.ToDto());
    }
}
