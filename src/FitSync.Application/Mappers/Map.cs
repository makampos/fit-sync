using FitSync.Domain.Dtos;
using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.Features.Users;
using FitSync.Domain.Features.WorkoutPlans;
using FitSync.Domain.ViewModels;
using FitSync.Domain.ViewModels.Users;

namespace FitSync.Application.Mappers;

public static class Map
{
    public static WorkoutEntity ToDomainEntity(this WorkoutCSV workoutCsv)
    {
        return new WorkoutEntity
        {
            Title = workoutCsv.Title,
            Description = workoutCsv.Description,
            Type = workoutCsv.Type switch
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
            BodyPart = workoutCsv.BodyPart,
            Equipment = workoutCsv.Equipment,
            WorkoutLevel = workoutCsv.Level switch
            {
                "Beginner" => WorkoutLevel.Beginner,
                "Intermediate" => WorkoutLevel.Intermediate,
                _ => WorkoutLevel.Unkonwn
            }
        };
    }

    public static WorkoutEntity ToDomainEntity(this WorkoutDto workoutEntity)
    {
        return new WorkoutEntity
        {
            Title = workoutEntity.Title,
            Description = workoutEntity.Description,
            Type = workoutEntity.Type,
            BodyPart = workoutEntity.BodyPart,
            Equipment = workoutEntity.Equipment,
            WorkoutLevel = workoutEntity.Level
        };
    }

    public static WorkoutDto ToDto(this WorkoutEntity workoutEntity)
    {
        return new WorkoutDto(
            workoutEntity.Id,
            workoutEntity.Title,
            workoutEntity.Description,
            workoutEntity.Type,
            workoutEntity.BodyPart,
            workoutEntity.Equipment,
            workoutEntity.WorkoutLevel
        );
    }

    public static UserEntity ToDomainEntity(this AddUser addUser)
    {
       return UserEntity.Create(addUser.Name, addUser.Age, addUser.Genre);
    }

    public static UserViewModel ToViewModel(this UserEntity userEntity)
    {
        return UserViewModel.Create(
            userEntity.Name,
            userEntity.Age,
            userEntity.Genre);
    }

    public static UserViewModelIncluded ToViewModelIncluded(this UserEntity userEntity)
    {
        return UserViewModelIncluded.Create(
            userEntity.Name,
            userEntity.Age,
            userEntity.Genre,
            userEntity.WorkoutPlans
                .Select(w => w.ToViewModel()).ToList());
    }

    public static WorkoutViewModel ToViewModel(this WorkoutEntity workoutEntity, ExerciseSet? exerciseSet = null)
    {
        return new WorkoutViewModel(
            workoutEntity.Id,
            workoutEntity.Title,
            workoutEntity.Description,
            workoutEntity.Type,
            workoutEntity.BodyPart,
            workoutEntity.Equipment,
            workoutEntity.WorkoutLevel,
            exerciseSet
        );
    }

    public static WorkoutPlanEntity ToDomainEntity(this AddWorkoutPlan addWorkoutPlan)
    {
        return new WorkoutPlanEntity
        {
            Name = addWorkoutPlan.Name,
            UserId = addWorkoutPlan.UserId,
            WorkoutPlanWorkoutEntities = addWorkoutPlan.Workouts
                .Select(w =>
                    WorkoutPlanWorkoutEntity.Create(
                    w.Key,
                    w.Value.Sets,
                    w.Value.RepsMin,
                    w.Value.RepsMax,
                    w.Value.RestBetweenSets,
                    w.Value.Notes))
                .ToList()
        };
    }

    public static WorkoutPlanViewModel ToViewModel(this WorkoutPlanEntity workoutPlanEntity)
    {
        return WorkoutPlanViewModel.Create(
            workoutPlanEntity.Id,
            workoutPlanEntity.Name,
            workoutPlanEntity.WorkoutPlanWorkoutEntities
                .Select(w => w.Workout
                    .ToViewModel(ExerciseSet.Create(
                        w.Sets,
                        w.RepsMin,
                        w.RepsMax,
                        w.RestBetweenSets,
                        w.Notes)))
                .ToList());
    }
}