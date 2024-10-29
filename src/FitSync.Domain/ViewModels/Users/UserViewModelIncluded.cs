using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Users;

public record UserViewModelIncluded(string Name, int Age, Genre Genre, IReadOnlyCollection<WorkoutPlanViewModel>
        WorkoutPlans)
{
    public static UserViewModelIncluded Create(string name, int age, Genre genre, IReadOnlyCollection<WorkoutPlanViewModel>
        workoutPlans)
    {
        return new UserViewModelIncluded(name, age, genre, workoutPlans);
    }
}