using FitSync.Domain.Entities;

namespace FitSync.Domain.Interfaces;

public interface ICsvReader
{
    Task<IEnumerable<Workout>> ReadWorkoutsAsync(MemoryStream inputStream);
}