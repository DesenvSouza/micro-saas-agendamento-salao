using FluentValidation;

namespace SalonBooking.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.ProfessionalId)
            .NotEmpty().WithMessage("ProfessionalId é obrigatório.");

        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("ServiceId é obrigatório.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Horário de início é obrigatório.")
            .GreaterThan(DateTime.UtcNow).WithMessage("O horário deve ser no futuro.");

        RuleFor(x => x.ClientNotes)
            .MaximumLength(500).When(x => x.ClientNotes is not null);
    }
}
