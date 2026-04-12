namespace SalonBooking.Domain.Entities;

public class ProfessionalService
{
    public Guid ProfessionalId { get; private set; }
    public Guid ServiceId { get; private set; }

    public Professional Professional { get; private set; } = default!;
    public Service Service { get; private set; } = default!;

    private ProfessionalService() { }

    public static ProfessionalService Create(Guid professionalId, Guid serviceId)
    {
        return new ProfessionalService
        {
            ProfessionalId = professionalId,
            ServiceId = serviceId
        };
    }
}
