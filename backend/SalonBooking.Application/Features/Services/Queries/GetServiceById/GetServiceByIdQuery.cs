using MediatR;

namespace SalonBooking.Application.Features.Services.Queries.GetServiceById;

public record GetServiceByIdQuery(Guid Id) : IRequest<ServiceDto>;
