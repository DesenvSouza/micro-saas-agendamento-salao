using FluentValidation;

namespace SalonBooking.Application.Features.Services.Commands.UpdateService;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do serviço é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duração deve ser maior que zero.")
            .LessThanOrEqualTo(480);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Preço não pode ser negativo.");

        RuleFor(x => x.SlotIntervalMinutes)
            .GreaterThan(0).When(x => x.SlotIntervalMinutes.HasValue);
    }
}
