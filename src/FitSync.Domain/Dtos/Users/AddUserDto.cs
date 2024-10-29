using FitSync.Domain.Enums;

namespace FitSync.Domain.Dtos.Users;

public record AddUserDto(string Name, int Age, Genre Genre);