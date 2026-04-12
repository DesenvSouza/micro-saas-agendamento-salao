using SalonBooking.Domain.Interfaces;

namespace SalonBooking.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly SalonBookingDbContext _context;

    public UnitOfWork(SalonBookingDbContext context) => _context = context;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
