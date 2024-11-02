using FitSync.Application.Extensions;
using FitSync.Application.Services;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FluentAssertions;

namespace FitSync.UnitTests;

public class UserServiceTests : DataBaseTest<UserService>
{
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = CreateInstance<UserService>();
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnId_WhenUserCreated()
    {
        var addUserDto = AddUserDto.Create(Faker.Random.String(), Faker.Random.Int(1, 100),
            Faker.PickRandom<Genre>());

        var result = await _userService.CreateUserAsync(addUserDto);

        result.Should().NotBeNull();
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var addUserDto = AddUserDto.Create(Faker.Random.String(), Faker.Random.Int(1, 100),
            Faker.PickRandom<Genre>());

        var result = await _userService.CreateUserAsync(addUserDto);

        var user = await _userService.GetUserByIdAsync(result.Data);

        user.Should().NotBeNull();
        user.Data.Id.Should().Be(result.Data);
        user.Data.Name.Should().Be(addUserDto.Name);
        user.Data.Age.Should().Be(addUserDto.Age);
        user.Data.Genre.Should().Be(addUserDto.Genre);
    }

    [Fact]
    public async Task GetUserByIdIncludeAllAsync_ShouldReturnUser_WhenUserExists()
    {
        var userEntity = AddUserDto.Create(Faker.Random.String(), Faker.Random.Int(1, 100),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        var workoutEntity = AddWorkoutDto.Create(Faker.Random.String(), Faker.Random.String(), WorkoutType.Strength,
            Faker.Random.String(), Faker.Random.String(), WorkoutLevel.Beginner).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutEntity);

        var workoutPlanEntity = AddWorkoutPlanDto.Create(userEntity.Id, Faker.Random.String(),
                new Dictionary<int, ExerciseSet>()
                {
                    {
                        workoutEntity.Id, ExerciseSet.Create(
                            Faker.Random.Int(),
                        Faker.Random.Int(),
                        Faker.Random.Int(),
                        Faker.Random.Int(),
                        Faker.Random.String())
                    }
                }).ToDomainEntity();

        await _fitSyncUnitOfWork.WorkoutPlanRepository.AddAsync(workoutPlanEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var result = await _userService.GetUserByIdIncludeAllAsync(userEntity.Id);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.WorkoutPlans.Should().HaveCountGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        var result = await _userService.GetUserByIdAsync(Faker.Random.Int());

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("User not found");
    }

    [Fact]
    public async Task GetUserByIdIncludeAllAsync_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        var result = await _userService.GetUserByIdIncludeAllAsync(Faker.Random.Int());

        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("User not found");
    }
}