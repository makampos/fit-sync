using FitSync.Domain.Enums;

namespace FitSync.Domain.ViewModels.Users;

public record UserViewModel(int Id, string Name, int Age, Genre Genre)
{
    public static UserViewModel Create(int id, string name, int age, Genre genre)
    {
        return new UserViewModel(id, name, age, genre);
    }
}