using FitSync.Domain.ViewModels;

namespace FitSync.Domain.Dtos;

public record AddEventsDto(ICollection<WorkoutPlanViewModel> WorkoutPlansViewModel, DateOnly StartDate, DateOnly EndDate, int FrequencyType, int DayOfWeek);