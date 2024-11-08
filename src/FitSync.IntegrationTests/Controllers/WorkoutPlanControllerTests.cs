using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FitSync.API.Responses;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.ViewModels.WorkoutPlans;
using FitSync.IntegrationTests.Configurations;
using FluentAssertions;

namespace FitSync.IntegrationTests.Controllers;

[CollectionDefinition(nameof(WorkoutPlanControllerTestsCollection))]
public class WorkoutPlanControllerTestsCollection : ICollectionFixture<IntegrationTestFactory>;

[Collection(nameof(WorkoutPlanControllerTestsCollection))]
public class WorkoutPlanControllerTests : DatabaseTest
{
    public WorkoutPlanControllerTests(IntegrationTestFactory factory) : base(factory){ }

    [Fact]
    public async Task CreateWorkoutPlan_ReturnsCreated()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var request = Fixture
            .Build<AddWorkoutPlanDto>()
            .With(x => x.UserId, userId)
            .With(x => x.WorkoutIdToExerciseSet,
                new Dictionary<int, ExerciseSet>()
                {
                    { workoutId, Fixture.Create<ExerciseSet>() }
                }).Create();

        // Act
        var response = await Client.PostAsJsonAsync("/api/workout-plans", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var id = await response.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location.Should().BeOfType<Uri>();
        response.Headers.Location!.Should().Be($"http://localhost/api/workout-plans/{id}"); // Make it better somehow
    }

    [Fact]
    public async Task GetWorkoutPlanById_ReturnsOk()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);

        // Act
        var getWorkoutPlanResponse = await Client.GetAsync($"/api/workout-plans/{workoutPlanId}");

        // Assert
        getWorkoutPlanResponse.EnsureSuccessStatusCode();
        var workoutPlan = await getWorkoutPlanResponse.Content.ReadFromJsonAsync<WorkoutPlanViewModel>();

        workoutPlan.Should().NotBeNull();
        workoutPlan!.WorkoutWithExercisesSetViewModel.Should()
            .Contain(x => x.WorkoutId == workoutId);
        workoutPlan.WorkoutPlanId.Should().Be(workoutPlanId);
    }

    [Fact]
    public async Task GetWorkoutPlanById_ReturnsNotFound()
    {
        // Arrange
        var id = 0;

        // Act
        var getWorkoutPlanResponse = await Client.GetAsync($"/api/workout-plans/{id}");

        // Assert
        getWorkoutPlanResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetWorkoutPlansByUserIdAsync_ReturnsOk()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);

        // Act
        var getWorkoutPlansResponse = await Client.GetAsync($"/api/workout-plans/users/{userId}");

        // Assert
        getWorkoutPlansResponse.EnsureSuccessStatusCode();
        var workoutPlans = await getWorkoutPlansResponse.Content.ReadFromJsonAsync<IEnumerable<WorkoutPlanViewModel>>();

        workoutPlans.Should().NotBeEmpty();
        workoutPlans.Should().Contain(x => x.WorkoutPlanId == workoutPlanId);
    }

    [Fact]
    public async Task GetWorkoutPlansByUserIdAsync_ReturnsNotFound()
    {
        // Arrange
        var id = 0;

        // Act
        var getWorkoutPlansResponse = await Client.GetAsync($"/api/workout-plans/users/{id}");

        // Assert
        getWorkoutPlansResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateWorkoutPlan_ReturnsNoContent()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);

        var updateRequest = Fixture.Build<UpdateWorkoutPlanDto>()
            .With(x => x.Id, workoutPlanId)
            .With(x => x.UserId, userId)
            .With(x => x.Workouts,
                new Dictionary<int, ExerciseSet>()
                {
                    { workoutId, Fixture.Create<ExerciseSet>() }
                })
            .Create();

        // Act
        var response = await Client.PutAsJsonAsync("/api/workout-plans", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateWorkoutPlan_ReturnsNotFound()
    {
        // Arrange
        var workoutPlanId = 0;

        var updateRequest = Fixture.Build<UpdateWorkoutPlanDto>()
            .With(x => x.Id, workoutPlanId)
            .Create();

        // Act
        var response = await Client.PutAsJsonAsync("/api/workout-plans", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWorkoutPlan_ReturnsNoContent()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);

        // Act
        var response = await Client.DeleteAsync($"/api/workout-plans/{workoutPlanId}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteWorkoutPlan_ReturnsNotFound()
    {
        // Arrange
        var workoutPlanId = 0;

        // Act
        var response = await Client.DeleteAsync($"/api/workout-plans/{workoutPlanId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ToggleWorkoutPlanActiveAsync_ShouldReturnNoContent()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);

        // Act
        var response = await Client.PatchAsJsonAsync($"api/workout-plans/{workoutPlanId}/toggle-active", true);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ToggleWorkoutPlanActiveAsync_ShouldReturnNotFound()
    {
        // Arrange
        // Act
        var response = await Client.PatchAsJsonAsync($"api/workout-plans/{0}/toggle-active", true);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ToggleWorkoutPlanActiveAsync_ShouldReturnBadRequest()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var workoutPlanId = await CreateWorkoutPlanAsync(userId, workoutId);
        var status = false;

        // Act
        var response = await Client.PatchAsJsonAsync($"api/workout-plans/{workoutPlanId}/toggle-active", status);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var errorMessage = await response.Content.ReadAsStringAsync();
        errorMessage.Should().Be("Can not update workout plan status",
            because: "Because it is already {IsActive}", status);
    }
}
