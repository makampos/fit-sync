namespace FitSync.Domain.ViewModels.Users;

public record UserPreferencesViewModel(string PreferredWeightUnit, string PreferredDistanceUnit)
{
    public static UserPreferencesViewModel Create(string preferredWeightUnit, string preferredDistanceUnit)
    {
        return new UserPreferencesViewModel(preferredWeightUnit, preferredDistanceUnit);
    }
}