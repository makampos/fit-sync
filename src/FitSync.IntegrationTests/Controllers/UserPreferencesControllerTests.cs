using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FitSync.API.Responses;
using FitSync.Domain.Dtos.Users.Preferences;
using FitSync.Domain.ViewModels.Users;
using FitSync.IntegrationTests.Configurations;
using FluentAssertions;

namespace FitSync.IntegrationTests.Controllers;

[CollectionDefinition(nameof(UserPreferencesControllerTestsCollection))]
public class UserPreferencesControllerTestsCollection : ICollectionFixture<IntegrationTestFactory>;

[Collection(nameof(UserPreferencesControllerTestsCollection))]
public class UserPreferencesControllerTests : DatabaseTest
{
    public UserPreferencesControllerTests(IntegrationTestFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateUserPreferences_ReturnsCreated()
    {
        // Arrange
        var userId = await CreateUserAsync();

        var request = Fixture.Build<AddUserPreferencesDto>()
            .With(x => x.UserId, userId)
            .Create();

        // Act
        var response = await Client.PostAsJsonAsync("/api/user-preferences", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var id = await response.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.Should().BeOfType<Uri>();
        response.Headers.Location!.Should().Be($"http://localhost/api/user-preferences/{id}"); // Make it better somehow
    }

    [Fact]
    public async Task GetUserPreferencesById_ReturnsOk()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var request = Fixture.Build<AddUserPreferencesDto>()
            .With(x => x.UserId, userId)
            .Create();
        var createResponse = await Client.PostAsJsonAsync("/api/user-preferences", request);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        // Act
        var response = await Client.GetAsync($"/api/user-preferences/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var userPreferences = await response.Content.ReadFromJsonAsync<UserPreferencesViewModel>();
        userPreferences.Should().NotBeNull();
        userPreferences!.PreferredWeightUnit.Should().Be(request.PreferredWeightUnit.ToString());
        userPreferences.PreferredDistanceUnit.Should().Be(request.PreferredDistanceUnit.ToString());
    }

    [Fact]
    public async Task GetUserPreferencesById_ReturnsNotFound()
    {
        // Arrange
        var id = 0;

        // Act
        var response = await Client.GetAsync($"/api/user-preferences/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateUserPreferences_ReturnsNoContent()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var request = Fixture.Build<AddUserPreferencesDto>()
            .With(x => x.UserId, userId)
            .Create();
        var createResponse = await Client.PostAsJsonAsync("/api/user-preferences", request);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        var updateRequest = Fixture.Build<UpdateUserPreferencesDto>()
            .Without(x => x.Id)
            .Create();

        // Act
        var response = await Client.PutAsJsonAsync($"/api/user-preferences/{id}", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateUserPreferences_ReturnsNotFound()
    {
        // Arrange
        var id = 0;
        var updateRequest = Fixture.Build<UpdateUserPreferencesDto>()
            .Without(x => x.Id)
            .Create();

        // Act
        var response = await Client.PutAsJsonAsync($"/api/user-preferences/{id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // TODO: add BadRequest test for CreateUserPreferences/UpdateUserPreferences once FluentValidation is added
}