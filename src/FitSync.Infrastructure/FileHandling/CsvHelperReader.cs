using System.Globalization;
using CsvHelper;
using FitSync.Domain.Entities;
using FitSync.Domain.Interfaces;
using FitSync.Infrastructure.Mappers;

namespace FitSync.Infrastructure.FileHandling;

public class CsvHelperReader : ICsvReader
{
    public async Task<IEnumerable<WorkoutCSV>> ReadWorkoutsAsync(MemoryStream inputStream)
    {
        using var reader = new StreamReader(inputStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<WorkoutMap>();

        var records = new List<WorkoutCSV>();

        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<WorkoutCSV>();
            records.Add(record);
        }

        return records;
    }
}