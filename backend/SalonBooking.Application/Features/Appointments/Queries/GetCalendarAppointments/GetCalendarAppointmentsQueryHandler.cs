using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Appointments.Queries.GetCalendarAppointments;

public class GetCalendarAppointmentsQueryHandler
    : IRequestHandler<GetCalendarAppointmentsQuery, IEnumerable<CalendarEventDto>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCalendarAppointmentsQueryHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CalendarEventDto>> Handle(
        GetCalendarAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var appointments = await _appointmentRepository.GetByEstablishmentAsync(
            establishmentId,
            request.From,
            request.To,
            professionalId: request.ProfessionalId,
            cancellationToken: cancellationToken);

        return appointments.Select(a => new CalendarEventDto(
            a.Id.ToString(),
            BuildTitle(a.Client?.FullName, a.Service?.Name, a.Professional?.Name),
            a.StartTime,
            a.EndTime,
            StatusColor(a.Status),
            a.Status.ToString(),
            a.Client?.FullName,
            a.Professional?.Name,
            a.Service?.Name,
            a.Price));
    }

    private static string BuildTitle(string? client, string? service, string? professional)
    {
        var parts = new[] { client, service, professional }.Where(p => p is not null);
        return string.Join(" · ", parts);
    }

    private static string StatusColor(AppointmentStatus status) => status switch
    {
        AppointmentStatus.Pending   => "#FFA500",
        AppointmentStatus.Confirmed => "#007BFF",
        AppointmentStatus.Completed => "#28A745",
        AppointmentStatus.Cancelled => "#DC3545",
        AppointmentStatus.NoShow    => "#6C757D",
        _ => "#6C757D"
    };
}
