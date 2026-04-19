using MediatR;
using SalonBooking.Application.Common.Interfaces;
using SalonBooking.Application.Features.Appointments;
using SalonBooking.Domain.Enums;
using SalonBooking.Domain.Interfaces.Repositories;

namespace SalonBooking.Application.Features.Dashboard.Queries.GetEstablishmentDashboard;

public class GetEstablishmentDashboardQueryHandler
    : IRequestHandler<GetEstablishmentDashboardQuery, DashboardDto>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetEstablishmentDashboardQueryHandler(
        IAppointmentRepository appointmentRepository,
        ICurrentUserService currentUserService)
    {
        _appointmentRepository = appointmentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DashboardDto> Handle(
        GetEstablishmentDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var establishmentId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var todayEnd = todayStart.AddDays(1);
        var weekStart = todayStart.AddDays(-(int)now.DayOfWeek);
        var weekEnd = weekStart.AddDays(7);

        // Load week appointments (covers today + this week)
        var weekAppointments = (await _appointmentRepository.GetByEstablishmentAsync(
            establishmentId,
            from: weekStart,
            to: weekEnd,
            cancellationToken: cancellationToken)).ToList();

        var todayAppointments = weekAppointments
            .Where(a => a.StartTime >= todayStart && a.StartTime < todayEnd)
            .ToList();

        var revenueThisWeek = weekAppointments
            .Where(a => a.Status == AppointmentStatus.Completed)
            .Sum(a => a.Price);

        // Pending/Confirmed counts (all time, not just this week)
        var activeAppointments = await _appointmentRepository.GetByEstablishmentAsync(
            establishmentId,
            cancellationToken: cancellationToken);

        var pendingCount = activeAppointments.Count(a => a.Status == AppointmentStatus.Pending);
        var confirmedCount = activeAppointments.Count(a => a.Status == AppointmentStatus.Confirmed);

        // 5 most recent appointments
        var recent = activeAppointments
            .OrderByDescending(a => a.StartTime)
            .Take(5)
            .Select(a => a.ToDto());

        return new DashboardDto(
            AppointmentsToday: todayAppointments.Count,
            AppointmentsThisWeek: weekAppointments.Count,
            RevenueThisWeek: revenueThisWeek,
            PendingCount: pendingCount,
            ConfirmedCount: confirmedCount,
            RecentAppointments: recent);
    }
}
