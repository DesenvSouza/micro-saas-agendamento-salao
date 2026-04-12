using FluentValidation;

namespace SalonBooking.Application.Features.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do serviço é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duração deve ser maior que zero.")
            .LessThanOrEqualTo(480).WithMessage("Duração máxima é de 480 minutos (8 horas).");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo.");

        RuleFor(x => x.SlotIntervalMinutes)
            .GreaterThan(0).WithMessage("Intervalo de slot deve ser maior que zero.")
            .When(x => x.SlotIntervalMinutes.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => x.Description is not null);
    }
}
