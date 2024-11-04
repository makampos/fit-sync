using System.Text;
using FitSync.Domain.ViewModels.WorkoutPlans;

namespace FitSync.Application.Extensions;

public static class WorkoutAggregator
{
    public static string AggregateWorkouts(IReadOnlyCollection<WorkoutPlanViewModel> workoutPlanViewModels)
    {
        if (!workoutPlanViewModels.Any())
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var workoutPlanVm in workoutPlanViewModels)
        {
            foreach (var (workout, index) in workoutPlanVm.WorkoutWithExercisesSetViewModel.WithIndex())
            {
                builder.AppendLine($"Exercise {index} :" + workout.Title);
                builder.AppendLine();
                builder.AppendLine(workout.Description);
                builder.AppendLine();
                builder.AppendLine("Level: " + workout.Level);
                builder.AppendLine("Body Part: " + workout.BodyPart);
                builder.AppendLine("Equipment: " + workout.Equipment); // Determine the unit of measurement on the front end based on user settings
                builder.AppendLine("Type: " + workout.Type);
                builder.AppendLine("Weight: " + workout.ExerciseSet.Weight);
                builder.AppendLine("Sets: " + workout.ExerciseSet.Sets + " * " + $"({workout.ExerciseSet.RepsMin + " ~ " + workout.ExerciseSet.RepsMax}) / {workout.ExerciseSet.RestBetweenSets} seconds");
                builder.AppendLine("Notes: " + workout.ExerciseSet.Notes);
            }
        }

        return builder.ToString();
    }

    private static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index + 1));
    }
}