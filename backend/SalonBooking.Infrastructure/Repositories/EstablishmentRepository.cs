using Microsoft.EntityFrameworkCore;
using SalonBooking.Domain.Entities;
using SalonBooking.Domain.Interfaces.Repositories;
using SalonBooking.Infrastructure.Persistence;

namespace SalonBooking.Infrastructure.Repositories;

public class EstablishmentRepository : BaseRepository<Establishment>, IEstablishmentRepository
{
    public EstablishmentRepository(SalonBookingDbContext context) : base(context) { }

    public async Task<Establishment?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _dbSet.FirstOrDefaultAsync(
            e => e.Email == email.ToLowerInvariant(), cancellationToken);

    public async Task<IEnumerable<Establishment>> GetNearbyAsync(
        double latitude,
        double longitude,
        double radiusKm,
        int skip,
        int take,
        CancellationToken cancellationToken = default)
    {
        // Bounding box pre-filter for performance (avoids full table scan)
        var latDelta = radiusKm / 111.0;
        var lngDelta = radiusKm / (111.0 * Math.Cos(latitude * Math.PI / 180.0));
        var minLat = latitude - latDelta;
        var maxLat = latitude + latDelta;
        var minLng = longitude - lngDelta;
        var maxLng = longitude + lngDelta;

        // Load candidates from bounding box
        var candidates = await _dbSet
            .Where(e => e.IsActive
                     && e.IsApproved
                     && (double)e.Latitude >= minLat && (double)e.Latitude <= maxLat
                     && (double)e.Longitude >= minLng && (double)e.Longitude <= maxLng)
            .ToListAsync(cancellationToken);

        // Apply Haversine filter and sort in memory
        return candidates
            .Select(e => new
            {
                Establishment = e,
                Distance = HaversineDistance(latitude, longitude, (double)e.Latitude, (double)e.Longitude)
            })
            .Where(x => x.Distance <= radiusKm)
            .OrderBy(x => x.Distance)
            .Skip(skip)
            .Take(take)
            .Select(x => x.Establishment);
    }

    private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2))
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRadians(double deg) => deg * Math.PI / 180;
}
