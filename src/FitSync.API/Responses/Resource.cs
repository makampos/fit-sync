namespace FitSync.API.Responses;

public record Resource(int Id, string? Version = null)
{
    public static Resource Create(int id, string? version = "1.0")
    {
        return new Resource(id, version);
    }

    public object GetRouteValues()
    {
        return new { id = Id };
    }

    public object GetCreatedResource()
    {
        return new { Id, Version };
    }

    public void Deconstruct(out int id, out string? version)
    {
        id = Id;
        version = Version;
    }
}
