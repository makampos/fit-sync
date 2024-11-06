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
    public async Task CreateWorkoutPlan_ReturnsWorkoutPlanId()
    {
        // Arrange
        var userId = await CreateUserAsync();
        var workoutId = await CreateWorkoutAsync();

        var request = _fixture
            .Build<AddWorkoutPlanDto>()
            .With(x => x.UserId, userId)
            .With(x => x.WorkoutIdToExerciseSet,
                new Dictionary<int, ExerciseSet>()
                {
                    { workoutId, _fixture.Create<ExerciseSet>() }
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
    public async Task GetWorkoutPlanById_ReturnsWorkoutPlan()
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
}