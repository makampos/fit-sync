using CsvHelper.Configuration;
using FitSync.Domain.Entities;

namespace FitSync.Infrastructure.Mappers;

public class WorkoutMap : ClassMap<WorkoutCSV>
{
    public WorkoutMap()
    {
        Map(x => x.Title).Name("Title");
        Map(x => x.Description).Name("Desc");
        Map(x => x.Type).Name("Type");
        Map(x => x.BodyPart).Name("BodyPart");
        Map(x => x.Equipment).Name("Equipment");
        Map(x => x.Level).Name("Level");
        Map(x => x.Rating).Name("Rating").Ignore();
        Map(x => x.RatingDescription).Name("RatingDesc").Ignore();
    }
}