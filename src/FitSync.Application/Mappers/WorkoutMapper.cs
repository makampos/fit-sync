using FitSync.Domain.Entities;
using FitSync.Domain.Enums;

namespace FitSync.Application.Mappers;

public static class WorkoutMapper
{
    public static WorkoutEntity ToDomainEntity(this Workout workout)
    {
        return new WorkoutEntity
        {
            Title = workout.Title,
            Description = workout.Description,
            Type = workout.Type switch
            {
              "Strength" => WorkoutType.Strength,
              "Stretching" => WorkoutType.Stretching,
              "Plyometrics" => WorkoutType.Plyometrics,
              "Cardio" => WorkoutType.Cardio,
              "Strongman" => WorkoutType.Strongman,
              "Powerlifting" => WorkoutType.Powerlifting,
              "OlympicWeightlifting" => WorkoutType.OlympicWeightlifting,
              _ => WorkoutType.Unkonwn
            },
            BodyPart = workout.BodyPart,
            Equipment = workout.Equipment,
            WorkoutLevel = workout.Level switch
            {
                "Beginner" => WorkoutLevel.Beginner,
                "Intermediate" => WorkoutLevel.Intermediate,
                _ => WorkoutLevel.Unkonwn
            }
        };
    }
}