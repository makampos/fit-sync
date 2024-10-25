using FitSync.Application.Mappers;
using FitSync.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitSync.Application.Services;

public class DataImportService : IDataImportService
{
    private readonly ICsvReader _csvReader;
    private readonly IFitSyncUnitOfWork _fitSyncUnitOfWork;
    private readonly ILogger<DataImportService> _logger;

    public DataImportService(ICsvReader csvReader, IFitSyncUnitOfWork fitSyncUnitOfWork, ILogger<DataImportService> logger)
    {
        _csvReader = csvReader;
        _fitSyncUnitOfWork = fitSyncUnitOfWork;
        _logger = logger;
    }

    public async Task ImportWorkoutsAsync(MemoryStream inputStream)
    {
        _logger.LogInformation("Importing workouts from CSV file");

        var workouts = await _csvReader.ReadWorkoutsAsync(inputStream);
        var workoutsEntities = workouts.Select(workout => workout.ToDomainEntity()).GetEnumerator();

        _logger.LogInformation("Adding workouts to the database");

        //TODO: Update to bulk insert
        await _fitSyncUnitOfWork.WorkoutRepository.AddAsync(workoutsEntities);
        await _fitSyncUnitOfWork.SaveChangesAsync();
    }
}