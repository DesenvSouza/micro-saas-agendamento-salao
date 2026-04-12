namespace SalonBooking.Domain.Entities.Base;

public abstract class AuditableEntity : Entity
{
    public Guid? CreatedById { get; protected set; }
    public Guid? UpdatedById { get; protected set; }

    public void SetCreatedBy(Guid userId)
    {
        CreatedById = userId;
        UpdatedById = userId;
    }

    public void SetUpdatedBy(Guid userId)
    {
        UpdatedById = userId;
        SetUpdatedAt();
    }
}
