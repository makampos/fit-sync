using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Users.Preferences;

public record UpdateUserPreferencesDto(
    int UserPreferencesId,
    WeightUnit PreferredWeightUnit,
    DistanceUnit PreferredDistanceUnit)
{
    public static UpdateUserPreferencesDto Create(int userPreferencesId, WeightUnit preferredWeightUnit, DistanceUnit preferredDistanceUnit)
    {
        return new UpdateUserPreferencesDto(userPreferencesId, preferredWeightUnit, preferredDistanceUnit);
    }
}