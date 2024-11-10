using System.Text.Json.Serialization;
using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Users.Preferences;

public record UpdateUserPreferencesDto(
    [property: JsonIgnore] int Id,
    WeightUnit PreferredWeightUnit,
    DistanceUnit PreferredDistanceUnit)
{
    public static UpdateUserPreferencesDto Create(int id, WeightUnit preferredWeightUnit, DistanceUnit preferredDistanceUnit)
    {
        return new UpdateUserPreferencesDto(id, preferredWeightUnit, preferredDistanceUnit);
    }

    public UpdateUserPreferencesDto WithId(int id)
    {
        return this with { Id = id };
    }
}