using SalonBooking.Domain.Entities.Base;

namespace SalonBooking.Domain.Entities;

public class WorkingSchedule : Entity
{
    public Guid ProfessionalId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public TimeOnly? LunchBreakStart { get; private set; }
    public TimeOnly? LunchBreakEnd { get; private set; }
    public bool IsWorkingDay { get; private set; } = true;

    public Professional Professional { get; private set; } = default!;

    private WorkingSchedule() { }

    public static WorkingSchedule Create(
        Guid professionalId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        bool isWorkingDay = true,
        TimeOnly? lunchBreakStart = null,
        TimeOnly? lunchBreakEnd = null)
    {
        return new WorkingSchedule
        {
            ProfessionalId = professionalId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            IsWorkingDay = isWorkingDay,
            LunchBreakStart = lunchBreakStart,
            LunchBreakEnd = lunchBreakEnd
        };
    }

    public void Update(
        TimeOnly startTime,
        TimeOnly endTime,
        bool isWorkingDay,
        TimeOnly? lunchBreakStart = null,
        TimeOnly? lunchBreakEnd = null)
    {
        StartTime = startTime;
        EndTime = endTime;
        IsWorkingDay = isWorkingDay;
        LunchBreakStart = lunchBreakStart;
        LunchBreakEnd = lunchBreakEnd;
        SetUpdatedAt();
    }
}
