using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.ViewModels.Users;
using FitSync.Domain.ViewModels.WorkoutPlans;
using FitSync.Domain.ViewModels.Workouts;

namespace FitSync.Application.Extensions;

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

    public static WorkoutEntity ToDomainEntity(this AddWorkoutDto addWorkoutDtoEntity)
    {
        return new WorkoutEntity
        {
            Title = addWorkoutDtoEntity.Title,
            Description = addWorkoutDtoEntity.Description,
            Type = addWorkoutDtoEntity.Type,
            BodyPart = addWorkoutDtoEntity.BodyPart,
            Equipment = addWorkoutDtoEntity.Equipment,
            WorkoutLevel = addWorkoutDtoEntity.Level
        };
    }

    public static UserEntity ToDomainEntity(this AddUserDto addUserDto)
    {
       return UserEntity.Create(addUserDto.Name, addUserDto.Age, addUserDto.Genre);
    }

    public static UserViewModel ToViewModel(this UserEntity userEntity)
    {
        return UserViewModel.Create(
            userEntity.Id,
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

    public static WorkoutWithExercisesSetViewModel ToViewModel(this WorkoutEntity workoutEntity, ExerciseSet? exerciseSet = null)
    {
        return new WorkoutWithExercisesSetViewModel(
            workoutEntity.Id,
            workoutEntity.Title,
            workoutEntity.Description,
            workoutEntity.Type,
            workoutEntity.BodyPart,
            workoutEntity.Equipment,
            workoutEntity.WorkoutLevel,
            exerciseSet // TODO: Make it better
        );
    }

    public static WorkoutViewModel ToViewModel(this WorkoutEntity workoutEntity)
    {
        return WorkoutViewModel.Create(
            workoutEntity.Id,
            workoutEntity.Title,
            workoutEntity.Description,
            workoutEntity.Type,
            workoutEntity.BodyPart,
            workoutEntity.Equipment,
            workoutEntity.WorkoutLevel);
    }

    public static WorkoutPlanEntity ToDomainEntity(this AddWorkoutPlanDto addWorkoutPlanDto)
    {
        return new WorkoutPlanEntity
        {
            Name = addWorkoutPlanDto.Name,
            UserId = addWorkoutPlanDto.UserId,
            WorkoutPlanWorkoutEntities = addWorkoutPlanDto.WorkoutIdToExerciseSet
                .Select(w =>
                    WorkoutPlanWorkoutEntity.Create(
                    w.Key,
                    w.Value.Sets,
                    w.Value.RepsMin,
                    w.Value.RepsMax,
                    w.Value.Weight,
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
                        w.Weight,
                        w.RestBetweenSets,
                        w.Notes)))
                .ToList());
    }
}