using System.Text;
using FitSync.Domain.ViewModels;

namespace FitSync.Application.Extensions;

public static class WorkoutAggregator
{
    public static string AggregateWorkouts(IEnumerable<WorkoutViewModel> workouts)
    {
        if (workouts == null || !workouts.Any())
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var workout in workouts)
        {
            builder.Append(workout.ToString());
            builder.AppendLine("---");
        }

        return builder.ToString();
    }
}