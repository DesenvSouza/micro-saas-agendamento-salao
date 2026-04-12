using FluentValidation;

namespace SalonBooking.Application.Features.Professionals.Commands.SetWorkingSchedule;

public class SetWorkingScheduleCommandValidator : AbstractValidator<SetWorkingScheduleCommand>
{
    public SetWorkingScheduleCommandValidator()
    {
        RuleFor(x => x.ProfessionalId).NotEmpty();

        RuleFor(x => x.Schedules)
            .NotEmpty().WithMessage("É necessário fornecer ao menos um dia na grade de horários.")
            .Must(s => s.Count() <= 7).WithMessage("Máximo de 7 dias na grade semanal.");

        RuleForEach(x => x.Schedules).ChildRules(s =>
        {
            s.RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .When(x => x.IsWorkingDay)
                .WithMessage("Horário de início deve ser anterior ao de término.");

            s.RuleFor(x => x.LunchBreakStart)
                .LessThan(x => x.LunchBreakEnd)
                .When(x => x.LunchBreakStart.HasValue && x.LunchBreakEnd.HasValue)
                .WithMessage("Início do almoço deve ser anterior ao término.");

            s.RuleFor(x => x.LunchBreakStart)
                .Must((input, start) => !start.HasValue || (start > input.StartTime && start < input.EndTime))
                .When(x => x.IsWorkingDay && x.LunchBreakStart.HasValue)
                .WithMessage("Intervalo de almoço deve estar dentro do horário de trabalho.");
        });
    }
}
