using FitSync.Domain.Enums;

namespace FitSync.Domain.Entities;

public class UserPreferencesEntity : TrackableEntity
{
    private UserPreferencesEntity(int userId, string preferredWeightUnit, string preferredDistanceUnit)
    {
        UserId = userId;

        PreferredWeightUnit = !string.IsNullOrWhiteSpace(preferredWeightUnit)
            ? preferredWeightUnit
            : throw new ArgumentException("Preferred weight unit cannot be null or empty.", nameof(preferredWeightUnit));

        PreferredDistanceUnit = !string.IsNullOrWhiteSpace(preferredDistanceUnit)
            ? preferredDistanceUnit
            : throw new ArgumentException("Preferred distance unit cannot be null or empty.", nameof(preferredDistanceUnit));
    }

    public string PreferredWeightUnit { get; private set; }
    public string PreferredDistanceUnit { get; private set; }

    // Foreign key
    public int UserId { get; private set; }
    public virtual UserEntity User { get; set; }


    public static UserPreferencesEntity Create(int userId, WeightUnit preferredWeightUnit, DistanceUnit preferredDistanceUnit)
    {
        return new UserPreferencesEntity(userId, preferredWeightUnit.ToString(), preferredDistanceUnit.ToString());
    }

    public void Update(WeightUnit preferredWeightUnit, DistanceUnit preferredDistanceUnit)
    {
        PreferredWeightUnit = preferredWeightUnit.ToString();
        PreferredDistanceUnit = preferredDistanceUnit.ToString();
    }
}