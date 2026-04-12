using MediatR;
using SalonBooking.Domain.Interfaces;

namespace SalonBooking.Application.Common.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        // Only commit for commands (not queries); queries don't modify state
        if (IsCommand(request))
            await _unitOfWork.CommitAsync(cancellationToken);

        return response;
    }

    private static bool IsCommand(TRequest request)
    {
        var requestType = typeof(TRequest).Name;
        return requestType.EndsWith("Command", StringComparison.OrdinalIgnoreCase);
    }
}
