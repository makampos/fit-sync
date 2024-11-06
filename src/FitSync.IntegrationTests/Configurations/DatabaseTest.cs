using System.Net.Http.Json;
using AutoFixture;
using FitSync.API.Responses;
using FitSync.Domain.Dtos.Users;
using FitSync.Domain.Dtos.WorkoutPlans;
using FitSync.Domain.Dtos.Workouts;
using FitSync.Infrastructure.Data;

namespace FitSync.IntegrationTests.Configurations;

public abstract class DatabaseTest : IAsyncLifetime
{
    protected HttpClient Client;
    protected readonly FitSyncDbContext DbContext;
    private readonly Func<Task> _resetDatabase;
    public Fixture _fixture = new();

    protected DatabaseTest(IntegrationTestFactory factory)
    {
        _resetDatabase = factory.ResetDatabase;
        DbContext = factory.FitSyncDbContext;
        Client = factory.CreateClient();
    }

    /// <summary>
    /// Helper method to create a user
    /// </summary>
    /// <returns>UserId</returns>
    protected virtual async Task<int> CreateUserAsync()
    {
        var userRequest = _fixture.Create<AddUserDto>();
        var createUserResponse = await Client.PostAsJsonAsync("/api/users", userRequest);
        createUserResponse.EnsureSuccessStatusCode();
        return await createUserResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);
    }

    /// <summary>
    ///  Helper method to create a workout
    /// </summary>
    /// <returns>WorkoutId</returns>
    protected virtual async Task<int> CreateWorkoutAsync()
    {
        var workoutRequest = _fixture.Create<AddWorkoutDto>();
        var createWorkoutResponse = await Client.PostAsJsonAsync("/api/workouts", workoutRequest);
        createWorkoutResponse.EnsureSuccessStatusCode();

        return await createWorkoutResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);;
    }

    /// <summary>
    ///  Helper method to create a workout plan
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="workoutId">Workout identifier</param>
    /// <returns>WorkoutPlanId</returns>
    protected virtual async Task<int> CreateWorkoutPlanAsync(int userId, int workoutId)
    {
        var workoutPlanRequest = _fixture
            .Build<AddWorkoutPlanDto>()
            .With(x => x.UserId, userId)
            .With(x => x.WorkoutIdToExerciseSet,
                new Dictionary<int, ExerciseSet>()
                {
                    { workoutId, _fixture.Create<ExerciseSet>() }
                }).Create();

        var createWorkoutPlanResponse = await Client.PostAsJsonAsync("/api/workout-plans", workoutPlanRequest);
        createWorkoutPlanResponse.EnsureSuccessStatusCode();
        return await createWorkoutPlanResponse.Content
            .ReadFromJsonAsync<Resource>()
            .ContinueWith(x => x.Result!.Id);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;
}
