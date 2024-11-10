using FitSync.Domain.Enums;

namespace FitSync.Domain.Entities;

// TODO: Create user Profile entity
public class UserEntity : TrackableEntity
{
    private UserEntity(string name, int age, Genre genre)
    {
        Name = name;
        Age = age;
        Genre = genre;
    }

    public string Name { get; private set; }
    public int Age { get; private set; }
    public Genre Genre { get; private set; }

    public virtual ICollection<WorkoutPlanEntity> WorkoutPlans { get; set; } = new List<WorkoutPlanEntity>();
    public virtual UserPreferencesEntity UserPreferences { get; set; }

    public static UserEntity Create(string name, int age, Genre genre)
    {
        return new UserEntity(name, age, genre);
    }
}