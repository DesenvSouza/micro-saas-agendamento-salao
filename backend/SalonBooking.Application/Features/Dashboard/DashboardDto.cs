using SalonBooking.Application.Features.Appointments;

namespace SalonBooking.Application.Features.Dashboard;

public record DashboardDto(
    int AppointmentsToday,
    int AppointmentsThisWeek,
    decimal RevenueThisWeek,
    int PendingCount,
    int ConfirmedCount,
    IEnumerable<AppointmentDto> RecentAppointments);
