namespace FitSync.Domain.Entities;

public class WorkoutCSV
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string BodyPart { get; set; }
    public string Equipment { get; set; }
    public string Level { get; set; }
    public decimal Rating { get; set; }
    public string RatingDescription { get; set; }

    public WorkoutCSV()
    {

    }
}