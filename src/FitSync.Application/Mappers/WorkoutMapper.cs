using FitSync.Domain.Dtos;
using FitSync.Domain.Entities;
using FitSync.Domain.Enums;
using FitSync.Domain.ViewModels;

namespace FitSync.Application.Mappers;

public static class WorkoutMapper
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

    public static UserEntity ToDomainEntity(this UserDto userDto)
    {
        return new UserEntity
        {
            Name = userDto.Name,
            Age = userDto.Age,
            Genre = userDto.Genre,
            WorkoutPlans = userDto.WorkoutPlans.Select(wp => wp.ToDomainEntity()).ToList()
        };
    }

    public static UserDto ToDto(this UserEntity userEntity)
    {
        return new UserDto(
            userEntity.Name,
            userEntity.Age,
            userEntity.Genre,
            userEntity.WorkoutPlans.Select(wp => wp.ToDto()).ToList()
        );
    }

    public static WorkoutPlanEntity ToDomainEntity(this AddWorkoutPlanDto addWorkoutPlanDto)
    {
        return new WorkoutPlanEntity
        {
            Name = addWorkoutPlanDto.Name,
            UserId = addWorkoutPlanDto.UserId,
            Workouts = addWorkoutPlanDto.WorkoutsDto.Select(w => w.ToDomainEntity()).ToList()
        };
    }

    public static WorkoutPlanEntity ToDomainEntity(this WorkoutPlanDto workoutPlanDto)
    {
        return new WorkoutPlanEntity
        {
            Name = workoutPlanDto.Name,
            UserId = workoutPlanDto.UserId,
            Workouts = workoutPlanDto.WorkoutPlans.Select(w => w.ToDomainEntity()).ToList()
        };
    }

    public static WorkoutPlanDto ToDto(this WorkoutPlanEntity workoutPlanEntity)
    {
        return new WorkoutPlanDto(
            workoutPlanEntity.Id,
            workoutPlanEntity.Name,
            workoutPlanEntity.UserId,
            workoutPlanEntity.Workouts.Select(w => w.ToDto()).ToList()
        );
    }

    public static WorkoutPlanWorkoutEntity ToDomainEntity(this WorkoutPlanWorkoutDto workoutPlanWorkoutDto)
    {
        return new WorkoutPlanWorkoutEntity
        {
            WorkoutId = workoutPlanWorkoutDto.WorkoutId
        };
    }

    public static WorkoutPlanWorkoutDto ToDto(this WorkoutPlanWorkoutEntity workoutPlanWorkoutEntity)
    {
        return new WorkoutPlanWorkoutDto(
            workoutPlanWorkoutEntity.WorkoutId
        );
    }

    public static WorkoutViewModel ToViewModel(this WorkoutEntity workoutEntity)
    {
        return new WorkoutViewModel(
            workoutEntity.Id,
            workoutEntity.Title,
            workoutEntity.Description,
            workoutEntity.Type,
            workoutEntity.BodyPart,
            workoutEntity.Equipment,
            workoutEntity.WorkoutLevel
        );
    }

    // public static UserViewModel ToViewModel(this UserEntity userEntity)
    // {
    //     return new UserViewModel(
    //         userEntity.Name,
    //         userEntity.Age,
    //         userEntity.Genre,
    //         userEntity.WorkoutPlans.Select(wp => wp.ToViewModel()).ToList()
    //     );
    // }

    // public static WorkoutPlanViewModel ToViewModel(this WorkoutPlanEntity workoutPlanEntity)
    // {
    //     return new WorkoutPlanViewModel(
    //         workoutPlanEntity.Id,
    //         workoutPlanEntity.Name,
    //         workoutPlanEntity.Workouts.Select(w => w.ToViewModel()).ToList()
    //     );
    // }

    // public static WorkoutPlanWorkoutViewModel ToViewModel(this WorkoutPlanWorkoutEntity workoutPlanWorkoutEntity)
    // {
    //     return new WorkoutPlanWorkoutViewModel(
    //         workoutPlanWorkoutEntity.WorkoutPlanId,
    //         workoutPlanWorkoutEntity.WorkoutId);
    // }
}