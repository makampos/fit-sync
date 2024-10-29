using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Users;

public record UserViewModel(string Name, int Age, Genre Genre)
{
    public static UserViewModel Create(string name, int age, Genre genre)
    {
        return new UserViewModel(name, age, genre);
    }
}