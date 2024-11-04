using FitSync.Application.Services;
using FitSync.Domain.Dtos.CalendarEvents;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FitSync.Domain.ViewModels.WorkoutPlans;
using FitSync.Domain.ViewModels.Workouts;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace FitSync.UnitTests;

public class CalendarServiceTests
{
    private readonly ICalendarService _calendarService =
        new CalendarService(
            new LoggerFactory().CreateLogger<CalendarService>()
        );

    [Fact]
    public async Task AddEventAsync_ShouldReturnCalendarEvent()
    {
        // Arrange
        var addWorkoutPlanCalendarEventsDto = AddWorkoutPlanCalendarEventDto.Create(
            new Dictionary<int, IReadOnlyCollection<WorkoutPlanViewModel>>()
            {
                // DayOfWeek
                { 1, new List<WorkoutPlanViewModel>()
                    {
                        WorkoutPlanViewModel.Create(
                            id: 1,
                            name: "Workout Plan 1",
                            workoutWithExercisesSetViewModel: new List<WorkoutWithExercisesSetViewModel>()
                            {
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 1,
                                    title: "Workout 1",
                                    description: "Workout 1 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                ),
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 2,
                                    title: "Workout 2",
                                    description: "Workout 2 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                ),
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 3,
                                    title: "Workout 3",
                                    description: "Workout 3 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                ),
                            }
                        ),
                    }
                },
                { 2, new List<WorkoutPlanViewModel>()
                    {
                        WorkoutPlanViewModel.Create(
                            id: 2,
                            name: "Workout Plan 2",
                            workoutWithExercisesSetViewModel: new List<WorkoutWithExercisesSetViewModel>()
                            {
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 44,
                                    title: "Workout 44",
                                    description: "Workout 44 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                )
                            }
                        )
                    }
                },
                { 3, new List<WorkoutPlanViewModel>()
                    {
                        WorkoutPlanViewModel.Create(
                            id: 3,
                            name: "Workout Plan 3",
                            workoutWithExercisesSetViewModel: new List<WorkoutWithExercisesSetViewModel>()
                            {
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 11,
                                    title: "Workout 11",
                                    description: "Workout 11 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                )
                            }
                        ),
                        WorkoutPlanViewModel.Create(
                            id: 4,
                            name: "Workout Plan 4",
                            workoutWithExercisesSetViewModel: new List<WorkoutWithExercisesSetViewModel>()
                            {
                                WorkoutWithExercisesSetViewModel.Create(
                                    id: 11,
                                    title: "Workout 656",
                                    description: "Workout 656 description",
                                    type: WorkoutType.Cardio,
                                    bodyPart: "Body Part Example",
                                    equipment: "Equipment Example",
                                    level: WorkoutLevel.Beginner,
                                    exerciseSet: ExerciseSet.Create(
                                        sets: 3,
                                        repsMin: 8,
                                        repsMax: 12,
                                        weight: 30,
                                        restBetweenSets: 70,
                                        notes: null
                                    )
                                )
                            }
                        )
                    }
                }
            },
            startDate: DateOnly.FromDateTime(DateTime.Now).AddDays(1),
            until: DateOnly.FromDateTime(DateTime.Now).AddDays(30)
        );

        // Act
        var result = await _calendarService.AddEventAsync(addWorkoutPlanCalendarEventsDto);
        result.Should().NotBeNullOrEmpty();
    }
}