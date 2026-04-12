using SalonBooking.Application.Common.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace SalonBooking.Infrastructure.Identity;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) =>
        BCryptNet.HashPassword(password, workFactor: 12);

    public bool Verify(string password, string hash) =>
        BCryptNet.Verify(password, hash);
}
