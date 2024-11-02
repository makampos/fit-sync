using FitSync.Application.Extensions;
using FitSync.Application.Services;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Enums;
using FluentAssertions;

namespace FitSync.UnitTests;

public class WorkoutServiceTests : DataBaseTest<WorkoutService>
{
    private readonly WorkoutService _workoutService;

    public WorkoutServiceTests()
    {
        _workoutService = CreateInstance<WorkoutService>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnWorkout()
    {
        // Arrange
        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>())
            .ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutService.GetByIdAsync(workoutEntity.Id);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Id.Should().Be(workoutEntity.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnWorkouts()
    {
        // Arrange
        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>())
            .ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutService.GetAllAsync(1, 10);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetFilteredWorkoutsAsync_ShouldReturnFilteredWorkouts()
    {
        // Arrange
        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>())
            .ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutService.GetFilteredWorkoutsAsync(workoutEntity.Type);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateWorkout()
    {
        // Arrange
        var addWorkoutDto = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>());

        // Act
        var result = await _workoutService.CreateAsync(addWorkoutDto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateWorkout()
    {
        // Arrange
        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>())
            .ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var updateWorkoutDto = UpdateWorkoutDto.Create(workoutEntity.Id, Faker.Random.String(), Faker.Random.String(),
            workoutEntity.Type, Faker.Random.String(), Faker.Random.String(), Faker.PickRandom<WorkoutLevel>());

        // Act
        var result = await _workoutService.UpdateAsync(updateWorkoutDto);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailureResult_WhenWorkoutNotFound()
    {
        // Arrange
        var updateWorkoutDto = UpdateWorkoutDto.Create(Faker.Random.Int(), Faker.Random.String(),
            Faker.Random.String(),
            Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
            Faker.PickRandom<WorkoutLevel>());

        // Act
        var result = await _workoutService.UpdateAsync(updateWorkoutDto);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("Workout not found");
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteWorkout()
    {
        // Arrange
        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(),
                Faker.PickRandom<WorkoutType>(), Faker.Random.String(), Faker.Random.String(),
                Faker.PickRandom<WorkoutLevel>())
            .ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutService.DeleteAsync(workoutEntity.Id);

        result.Success.Should().BeTrue();
    }
}