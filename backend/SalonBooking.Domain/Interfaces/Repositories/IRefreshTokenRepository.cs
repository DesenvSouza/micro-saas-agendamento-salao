using SalonBooking.Domain.Entities;

namespace SalonBooking.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task RevokeAllByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
