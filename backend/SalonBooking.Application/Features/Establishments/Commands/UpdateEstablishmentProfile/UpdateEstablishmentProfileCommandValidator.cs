using FluentValidation;

namespace SalonBooking.Application.Features.Establishments.Commands.UpdateEstablishmentProfile;

public class UpdateEstablishmentProfileCommandValidator
    : AbstractValidator<UpdateEstablishmentProfileCommand>
{
    public UpdateEstablishmentProfileCommandValidator()
    {
        RuleFor(x => x.TradeName)
            .NotEmpty().WithMessage("Nome fantasia é obrigatório.")
            .MaximumLength(200).WithMessage("Nome fantasia não pode ter mais de 200 caracteres.");

        RuleFor(x => x.LegalName)
            .MaximumLength(200).When(x => x.LegalName is not null);

        RuleFor(x => x.Cnpj)
            .Matches(@"^\d{14}$").WithMessage("CNPJ deve conter 14 dígitos numéricos.")
            .When(x => x.Cnpj is not null);

        RuleFor(x => x.Phone)
            .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter 10 ou 11 dígitos.")
            .When(x => x.Phone is not null);

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => x.Description is not null);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Rua é obrigatória.")
            .MaximumLength(300);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Cidade é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("Estado é obrigatório.")
            .Length(2).WithMessage("Estado deve ter 2 caracteres (UF).");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Matches(@"^\d{8}$").WithMessage("CEP deve conter 8 dígitos numéricos.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude inválida.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude inválida.");
    }
}
