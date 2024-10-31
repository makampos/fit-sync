using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Users;

public record AddUserDto(string Name, int Age, Genre Genre)
{
    public static AddUserDto Create(string name, int age, Genre genre)
    {
        return new AddUserDto(name, age, genre);
    }
}