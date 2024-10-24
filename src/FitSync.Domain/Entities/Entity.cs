using System.ComponentModel.DataAnnotations;

namespace FitSync.Domain.Entities;

public abstract class Entity
{
    [Key]
    public int Id { get; set; }
}