using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Queries.GetEstablishmentAppointments;

public class GetEstablishmentAppointmentsQueryHandler
    : IRequestHandler<GetEstablishmentAppointmentsQuery, IEnumerable<AppointmentDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEstablishmentAppointmentsQueryHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<AppointmentDto>> Handle(
        GetEstablishmentAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var appointments = await _appointmentRepository.GetByEstablishmentAsync(
            establishmentId,
            request.From,
            request.To,
            request.Status,
            request.ProfessionalId,
            cancellationToken);

        return appointments.Select(a => a.ToDto());
    }
}
