using FluentValidation;

namespace SalonBooking.Application.Features.Professionals.Commands.UpdateProfessional;

public class UpdateProfessionalCommandValidator : AbstractValidator<UpdateProfessionalCommand>
{
    public UpdateProfessionalCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do profissional é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .When(x => x.Bio is not null);

        RuleFor(x => x.PhotoUrl)
            .MaximumLength(500)
            .When(x => x.PhotoUrl is not null);
    }
}
