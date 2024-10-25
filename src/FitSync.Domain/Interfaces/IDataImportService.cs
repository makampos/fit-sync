namespace FitSync.Domain.Interfaces;

public interface IDataImportService
{
    Task ImportWorkoutsAsync(MemoryStream inputStream);
}