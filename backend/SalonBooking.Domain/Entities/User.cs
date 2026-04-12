using SalonBooking.Domain.Entities.Base;
using SalonBooking.Domain.Enums;

namespace SalonBooking.Domain.Entities;

public abstract class User : Entity
{
    public string Email { get; protected set; } = default!;
    public string PasswordHash { get; protected set; } = default!;
    public UserRole Role { get; protected set; }
    public bool IsActive { get; protected set; } = true;
    public bool EmailVerified { get; protected set; } = false;
    public string? EmailVerificationToken { get; protected set; }
    public string? PasswordResetToken { get; protected set; }
    public DateTime? PasswordResetTokenExpiresAt { get; protected set; }

    public ICollection<RefreshToken> RefreshTokens { get; protected set; } = [];
    public ICollection<Notification> Notifications { get; protected set; } = [];

    public void SetPasswordHash(string hash)
    {
        PasswordHash = hash;
        SetUpdatedAt();
    }

    public void VerifyEmail()
    {
        EmailVerified = true;
        EmailVerificationToken = null;
        SetUpdatedAt();
    }

    public void SetEmailVerificationToken(string token)
    {
        EmailVerificationToken = token;
        SetUpdatedAt();
    }

    public void SetPasswordResetToken(string token, DateTime expiresAt)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiresAt = expiresAt;
        SetUpdatedAt();
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = null;
        PasswordResetTokenExpiresAt = null;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }
}
