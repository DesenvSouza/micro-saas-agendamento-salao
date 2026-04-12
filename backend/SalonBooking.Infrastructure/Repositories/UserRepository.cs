using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SalonBookingDbContext _context;

    public UserRepository(SalonBookingDbContext context) => _context = context;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Set<User>().FindAsync([id], cancellationToken);

    public async Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _context.Set<User>()
            .AnyAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
}
