using FluentValidation;

namespace SalonBooking.Application.Features.Auth.Commands.RegisterEstablishment;

public class RegisterEstablishmentCommandValidator : AbstractValidator<RegisterEstablishmentCommand>
{
    public RegisterEstablishmentCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.")
            .MaximumLength(254);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Senha deve conter ao menos uma letra maiúscula.")
            .Matches("[0-9]").WithMessage("Senha deve conter ao menos um número.");

        RuleFor(x => x.TradeName)
            .NotEmpty().WithMessage("Nome do estabelecimento é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Endereço é obrigatório.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("Estado é obrigatório.")
            .Length(2).WithMessage("Estado deve ter 2 caracteres (ex: SP).");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{8}$").WithMessage("CEP deve conter 8 dígitos.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude inválida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude inválida.");
    }
}
