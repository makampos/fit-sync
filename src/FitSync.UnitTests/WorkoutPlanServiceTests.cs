using Bogus;
using FitSync.Application.Extensions;
using FitSync.Application.Services;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Enums;
using FluentAssertions;

namespace FitSync.UnitTests;

public class WorkoutPlanServiceTests : DataBaseTest<WorkoutPlanService>
{
    private readonly WorkoutPlanService _workoutPlanService;

    public WorkoutPlanServiceTests()
    {
        _workoutPlanService = CreateInstance<WorkoutPlanService>();
    }

    [Fact]
    public async Task GetWorkoutPlansByUserIdAsync_ShouldReturnWorkoutPlans()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();


        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                {
                    workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,Faker.Random.Int(),
                        Faker.Random.String())
                }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutPlanService.GetWorkoutPlansByUserIdAsync(userEntity.Id);

        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetWorkoutPlansByUserIdAsync_ShouldReturnOnlyActiveWorkoutPlans()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();


        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntityA = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                {
                    workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,Faker.Random.Int(),
                        Faker.Random.String())
                }
            }).ToDomainEntity();

        workoutPlanEntityA.ToggleIsActive(true);

        var workoutPlanEntityB = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                {
                    workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,Faker.Random.Int(),
                        Faker.Random.String())
                }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntityA);
        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntityB);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutPlanService.GetWorkoutPlansByUserIdAsync(userEntity.Id,
            true);

        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetWorkPlanByIdAsync_ShouldReturnWorkPlan()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();


        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                {
                    workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,Faker.Random.Int(),
                        Faker.Random.String())
                }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutPlanService.GetWorkoutPlanByIdAsync(workoutPlanEntity.Id);
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.WorkoutPlanId.Should().Be(workoutPlanEntity.Id);
        result.Data.Name.Should().Be(workoutPlanEntity.Name);
        result.Data.WorkoutWithExercisesSetViewModel.Count.Should().Be(1);
    }

    [Fact]
    public async Task CreateWorkPlanAsync_ShouldCreateWorkPlan()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();


        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var addWorkoutPlanDto = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
        {
            { workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                Faker.Random.Int(),Faker.Random.String()) }
        });

        // Act
        var result = await _workoutPlanService.CreateWorkPlanAsync(addWorkoutPlanDto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateWorkPlanAsync_ShouldUpdateWorkPlan()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();

        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                { workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                        Faker.Random.Int(),Faker.Random.String() ) }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var updateWorkoutPlanDto = UpdateWorkoutPlanDto.Create(workoutPlanEntity.Id, userEntity.Id, Faker.Random
            .String(),
            new Dictionary<int, ExerciseSet>()
            {
                { workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                    Faker.Random.Int(),
                    Faker.Random.String()) }
            });

        // Act
        var result = await _workoutPlanService.UpdateWorkPlanAsync(updateWorkoutPlanDto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();

        var workoutPlan = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetByIdAsync(workoutPlanEntity.Id);
        workoutPlan!.Name.Should().Be(updateWorkoutPlanDto.Name);
        workoutPlan!.WorkoutPlanWorkoutEntities.Count.Should().Be(1);
    }

    [Fact]
    public async Task DeleteWorkPlanAsync_ShouldDeleteWorkPlan()
    {
        // Arrange
        var userEntity = AddUserDto.Create(Faker.Random.String(), Faker.Random.Int(), Faker.PickRandom<Genre>())
            .ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();

        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                {
                    workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                        Faker.Random.Int(),Faker.Random.String())
                }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);

        await _fitSyncUnitOfWork.SaveChangesAsync();

        // Act
        var result = await _workoutPlanService.DeleteWorkPlanAsync(workoutPlanEntity.Id);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();

        var workoutPlan = await _fitSyncUnitOfWork.WorkoutPlanRepository.GetByIdAsync(workoutPlanEntity.Id);
        workoutPlan.Should().BeNull();
    }


    [Fact]
    public async Task UpdateWorkoutPlanIsActiveProperty_ShouldUpdateWorkoutPlan()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();

        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                { workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                    Faker.Random.Int(),Faker.Random.String() ) }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        workoutPlanEntity.IsActive.Should().BeFalse();

        var updateWorkoutPlanActiveOrInactiveDto = UpdateWorkoutPlanActiveOrInactiveDto.Create(workoutPlanEntity.Id,
            true);

        // Act
        var result = await _workoutPlanService.ToggleWorkoutPlanActiveAsync
            (updateWorkoutPlanActiveOrInactiveDto);


        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();

        var workoutPlanViewModel = await _workoutPlanService.GetWorkoutPlanByIdAsync(workoutPlanEntity.Id)
            .ContinueWith(x => x.Result.Data!);

        workoutPlanViewModel.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task
        UpdateWorkoutPlanIsActiveProperty_WhenTheInputIsTheSameAsTheExistingProperty_ShouldReturnErrorMessage()
    {
        // Arrange
        var userEntity = AddUserDto.Create("Test", 25, Genre.Male).ToDomainEntity();

        var workoutEntity = new Faker<AddWorkoutDto>()
            .CustomInstantiator(w => new AddWorkoutDto(
                Title: w.Random.String(),
                Description: w.Random.String(),
                Type: w.PickRandom<WorkoutType>(),
                BodyPart: w.Random.String(),
                Equipment: w.Random.String(),
                Level: w.PickRandom<WorkoutLevel>()))
            .Generate()
            .ToDomainEntity();

        await Task.WhenAll(
            _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity),
            _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity)
        );

        await _fitSyncUnitOfWork.SaveChangesAsync();

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
            new Dictionary<int, ExerciseSet>()
            {
                { workoutEntity.Id, new ExerciseSet(3, 10, 15, 60,
                    Faker.Random.Int(),Faker.Random.String() ) }
            }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        workoutPlanEntity.IsActive.Should().BeFalse();

        var updateWorkoutPlanActiveOrInactiveDto = UpdateWorkoutPlanActiveOrInactiveDto.Create(workoutPlanEntity.Id,
            false);

        // Act
        var result = await _workoutPlanService.ToggleWorkoutPlanActiveAsync
            (updateWorkoutPlanActiveOrInactiveDto);

        // Assert
        result.Success.Should().BeFalse();
        result.Data.Should().BeFalse();
        result.ErrorMessage.Should().Be("Can not update workout plan status");
    }
}
