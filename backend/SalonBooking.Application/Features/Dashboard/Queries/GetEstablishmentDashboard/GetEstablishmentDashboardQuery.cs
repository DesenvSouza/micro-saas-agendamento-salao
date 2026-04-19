using MediatR;

namespace SalonBooking.Application.Features.Dashboard.Queries.GetEstablishmentDashboard;

public record GetEstablishmentDashboardQuery : IRequest<DashboardDto>;
