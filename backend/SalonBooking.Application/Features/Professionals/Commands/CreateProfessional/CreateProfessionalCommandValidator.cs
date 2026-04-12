using FluentValidation;

namespace SalonBooking.Application.Features.Professionals.Commands.CreateProfessional;

public class CreateProfessionalCommandValidator : AbstractValidator<CreateProfessionalCommand>
{
    public CreateProfessionalCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do profissional é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .When(x => x.Bio is not null);
    }
}
