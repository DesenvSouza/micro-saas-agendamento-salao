using FluentValidation;

namespace SalonBooking.Application.Features.Professionals.Commands.AddTimeOff;

public class AddTimeOffCommandValidator : AbstractValidator<AddTimeOffCommand>
{
    public AddTimeOffCommandValidator()
    {
        RuleFor(x => x.ProfessionalId)
            .NotEmpty().WithMessage("ProfessionalId é obrigatório.");

        RuleFor(x => x.StartDateTime)
            .NotEmpty().WithMessage("Data de início é obrigatória.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Data de início deve ser futura.");

        RuleFor(x => x.EndDateTime)
            .NotEmpty().WithMessage("Data de fim é obrigatória.")
            .GreaterThan(x => x.StartDateTime).WithMessage("Data de fim deve ser posterior à data de início.");

        RuleFor(x => x.Reason)
            .MaximumLength(500).WithMessage("Motivo não pode ter mais de 500 caracteres.")
            .When(x => x.Reason is not null);
    }
}
