using SalonBooking.Domain.Entities.Base;

namespace SalonBooking.Domain.Entities;

public class EstablishmentPhoto : Entity
{
    public Guid EstablishmentId { get; private set; }
    public string Url { get; private set; } = default!;
    public int DisplayOrder { get; private set; }

    public Establishment Establishment { get; private set; } = default!;

    private EstablishmentPhoto() { }

    public static EstablishmentPhoto Create(Guid establishmentId, string url, int displayOrder = 0)
    {
        return new EstablishmentPhoto
        {
            EstablishmentId = establishmentId,
            Url = url,
            DisplayOrder = displayOrder
        };
    }

    public void UpdateOrder(int order)
    {
        DisplayOrder = order;
        SetUpdatedAt();
    }
}
