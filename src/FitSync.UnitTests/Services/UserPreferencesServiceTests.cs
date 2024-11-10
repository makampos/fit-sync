using FitSync.Application.Extensions;
using FitSync.Application.Services;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.Users.Preferences;
using FitSync.Domain.Enums;
using FitSync.Domain.Interfaces;
using FluentAssertions;

namespace FitSync.UnitTests.Services;

public class UserPreferencesServiceTests : DataBaseTest<UserPreferencesService>
{
    private readonly IUserPreferencesService _userPreferencesService;

    public UserPreferencesServiceTests()
    {
        _userPreferencesService = CreateInstance<UserPreferencesService>();
    }

    [Fact]
    public async Task CreateUserPreferencesAsync_ShouldReturnId_WhenUserPreferencesCreated()
    {
        var userEntity = AddUserDto.Create(Faker.Name.FullName(), Faker.Random.Int(18, 65),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();


        var addUserPreferencesDto = AddUserPreferencesDto.Create(userEntity.Id,
            Faker.PickRandom<WeightUnit>(),
            Faker.PickRandom<DistanceUnit>());

        var result = await _userPreferencesService.CreateUserPreferencesAsync(addUserPreferencesDto);

        result.Should().NotBeNull();
        result.Data.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetUserPreferencesAsync_ShouldReturnUserPreferences_WhenUserPreferencesExist()
    {
        var userEntity = AddUserDto.Create(Faker.Name.FullName(), Faker.Random.Int(18, 65),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var addUserPreferencesDto = AddUserPreferencesDto.Create(userEntity.Id,
            Faker.PickRandom<WeightUnit>(),
            Faker.PickRandom<DistanceUnit>());

        var id = await _userPreferencesService.CreateUserPreferencesAsync(addUserPreferencesDto)
            .ContinueWith(x => x.Result.Data);

        var result = await _userPreferencesService.GetUserPreferencesAsync(id);

        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data!.PreferredWeightUnit.Should().Be((addUserPreferencesDto.PreferredWeightUnit).ToString());
        result.Data!.PreferredDistanceUnit.Should().Be((addUserPreferencesDto.PreferredDistanceUnit).ToString());
    }

    [Fact]
    public async Task UpdateUserPreferencesAsync_ShouldReturnTrue_WhenUserPreferencesUpdated()
    {
        var userEntity = AddUserDto.Create(Faker.Name.FullName(), Faker.Random.Int(18, 65),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var addUserPreferencesDto = AddUserPreferencesDto.Create(userEntity.Id,
            Faker.PickRandom<WeightUnit>(),
            Faker.PickRandom<DistanceUnit>());

        var id = await _userPreferencesService.CreateUserPreferencesAsync(addUserPreferencesDto)
            .ContinueWith(x => x.Result.Data);

        var updateUserPreferencesDto = UpdateUserPreferencesDto.Create(id,
            addUserPreferencesDto.PreferredWeightUnit,
            addUserPreferencesDto.PreferredDistanceUnit);

        var result = await _userPreferencesService.UpdateUserPreferencesAsync(updateUserPreferencesDto);

        result.Should().NotBeNull();
        result.Data.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateUserPreferencesAsync_ShouldReturnFalse_WhenUserPreferencesNotFound()
    {
        var userEntity = AddUserDto.Create(Faker.Name.FullName(), Faker.Random.Int(18, 65),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var updateUserPreferencesDto = UpdateUserPreferencesDto.Create(userEntity.Id,
            Faker.PickRandom<WeightUnit>(),
            Faker.PickRandom<DistanceUnit>());

        var result = await _userPreferencesService.UpdateUserPreferencesAsync(updateUserPreferencesDto);

        result.Should().NotBeNull();
        result.Data.Should().BeFalse();
        result.ErrorMessage.Should().Be("User preferences not found");
    }

    [Fact]
    public async Task GetUserPreferencesAsync_ShouldReturnFailureResult_WhenUserPreferencesNotFound()
    {
        var userEntity = AddUserDto.Create(Faker.Name.FullName(), Faker.Random.Int(18, 65),
            Faker.PickRandom<Genre>()).ToDomainEntity();

        await _fitSyncUnitOfWork.UserRepository.AddAsync(userEntity);
        await _fitSyncUnitOfWork.SaveChangesAsync();

        var result = await _userPreferencesService.GetUserPreferencesAsync(userEntity.Id);
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Be("User preferences not found");
    }
}