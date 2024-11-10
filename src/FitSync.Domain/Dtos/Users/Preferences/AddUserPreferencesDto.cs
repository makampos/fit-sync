using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Users.Preferences;

public record AddUserPreferencesDto(
    int UserId,
    WeightUnit PreferredWeightUnit,
    DistanceUnit PreferredDistanceUnit)
{
    public static AddUserPreferencesDto Create(int userId, WeightUnit preferredWeightUnit, DistanceUnit preferredDistanceUnit)
    {
        return new AddUserPreferencesDto(userId, preferredWeightUnit, preferredDistanceUnit);
    }
}