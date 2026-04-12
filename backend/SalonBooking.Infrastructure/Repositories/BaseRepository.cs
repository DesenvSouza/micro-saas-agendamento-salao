using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities.Base;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : Entity
{
    protected readonly SalonBookingDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(SalonBookingDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbSet.FindAsync([id], cancellationToken);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.ToListAsync(cancellationToken);

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public virtual void Update(T entity) => _dbSet.Update(entity);

    public virtual void Remove(T entity) => _dbSet.Remove(entity);
}
