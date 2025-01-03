using Bogus;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Data;
using FitSync.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitSync.UnitTests;

public abstract class DataBaseTest<T> : IDisposable, IAsyncDisposable where T : class
{
    protected Faker Faker => new();
    private readonly ILogger<T> _logger;
    protected readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;
    private readonly FitSyncDbContext _fitSyncDbContext;

    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IWorkoutPlanWorkoutRepository _workoutPlanWorkoutRepository;
    private readonly IUserPreferencesRepository _userPreferencesRepository;

    protected DataBaseTest()
    {
        var options = new DbContextOptionsBuilder<FitSyncDbContext>()
            .UseInMemoryDatabase(databaseName: "FitSyncDatabase")
            .Options;

        _fitSyncDbContext = new FitSyncDbContext(options);

        _logger = new LoggerFactory().CreateLogger<T>();

        _workoutRepository = new WorkoutRepository(_fitSyncDbContext);
        _userRepository = new UserRepository(_fitSyncDbContext);
        _workoutPlanRepository = new WorkoutPlanRepository(_fitSyncDbContext);
        _workoutPlanWorkoutRepository = new WorkoutPlanWorkoutRepository(_fitSyncDbContext);
        _userPreferencesRepository = new UserPreferencesRepository(_fitSyncDbContext);

        _fitSyncUnitOfWork = new FitSyncUnitOfWork(
            _fitSyncDbContext,
            _workoutRepository,
            _userRepository,
            _workoutPlanRepository,
            _workoutPlanWorkoutRepository,
            _userPreferencesRepository);
    }

    protected TInstance CreateInstance<TInstance>()
    {
        return (TInstance)Activator.CreateInstance(typeof(TInstance), _logger, _fitSyncUnitOfWork)!;
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here

    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _fitSyncDbContext.Dispose();
        }
    }

    private async ValueTask DisposeAsyncCore()
    {
        ReleaseUnmanagedResources();

        await _fitSyncDbContext.DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }

    ~DataBaseTest()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
