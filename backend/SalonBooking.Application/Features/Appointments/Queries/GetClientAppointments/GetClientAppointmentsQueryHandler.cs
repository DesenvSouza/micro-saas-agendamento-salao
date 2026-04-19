using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Queries.GetClientAppointments;

public class GetClientAppointmentsQueryHandler
    : IRequestHandler<GetClientAppointmentsQuery, IEnumerable<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetClientAppointmentsQueryHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(
        GetClientAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var clientId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var appointments = await _appointmentRepository.GetByClientAsync(
            clientId, request.OnlyUpcoming, cancellationToken);

        return appointments.Select(a => a.ToDto());
    }
}
