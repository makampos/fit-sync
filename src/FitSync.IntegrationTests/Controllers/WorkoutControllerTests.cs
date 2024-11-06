using System.Net.Http.Json;
using AutoFixture;
using FitSync.API.Responses;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Domain.Results;
using FitSync.Domain.ViewModels.Workouts;
using FitSync.IntegrationTests.Configurations;
using FluentAssertions;

namespace FitSync.IntegrationTests.Controllers;

[CollectionDefinition(nameof(WorkoutControllerTestsCollection))]
public class WorkoutControllerTestsCollection : ICollectionFixture<IntegrationTestFactory>;


[Collection(nameof(WorkoutControllerTestsCollection))]
public class WorkoutControllerTests : DatabaseTest
{
    private readonly Fixture _fixture = new();

    public WorkoutControllerTests(IntegrationTestFactory factory) : base(factory){ }

    [Fact]
    public async Task CreateWorkout_ReturnsWorkout()
    {
        // Arrange
        var request = _fixture.Create<AddWorkoutDto>();

        // Act
        var response = await Client.PostAsJsonAsync("/api/workouts", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var id = await response.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.Should().BeOfType<Uri>();
        response.Headers.Location!.Should().Be($"http://localhost/api/workouts/{id}"); // Make it better somehow
    }

    [Fact]
    public async Task GetWorkoutById_ReturnsWorkout()
    {
        // Arrange
        var request = _fixture.Create<AddWorkoutDto>();
        var createResponse = await Client.PostAsJsonAsync("/api/workouts", request);
        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        // Act
        var response = await Client.GetAsync($"/api/workouts/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var workout = await response.Content.ReadFromJsonAsync<WorkoutViewModel>();

        workout.Should().NotBeNull();
        workout!.Title.Should().Be(request.Title);
        workout.Description.Should().Be(request.Description);
        workout.Type.Should().Be(request.Type);
        workout.BodyPart.Should().Be(request.BodyPart);
        workout.Equipment.Should().Be(request.Equipment);
        workout.Level.Should().Be(request.Level);
    }

    [Fact]
    public async Task GetAllWorkouts_ReturnsWorkouts()
    {
        // Arrange
        var request = _fixture.Create<AddWorkoutDto>();
        var createResponse = await Client.PostAsJsonAsync("/api/workouts", request);
        createResponse.EnsureSuccessStatusCode();

        // Act
        var response = await Client.GetAsync("/api/workouts");

        // Assert
        response.EnsureSuccessStatusCode();
        var workouts = await response.Content.ReadFromJsonAsync<PagedResult<WorkoutViewModel>>();

        workouts.Should().NotBeNull();
        workouts!.CurrentPage.Should().Be(1);
        workouts.PageSize.Should().Be(10);
        workouts.TotalPages.Should().Be(1);
        workouts.TotalCount.Should().BeGreaterOrEqualTo(1);
        workouts.Items.Should().NotBeEmpty();
    }
}