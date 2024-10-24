using FitSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Data;

public class FitSyncDbContext : DbContext
{
    public FitSyncDbContext(DbContextOptions<FitSyncDbContext> options) : base(options)
    {
    }

    public DbSet<WorkoutEntity> Workouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkoutEntity>()
            .ToTable("Workouts")
            .HasKey(w => w.Id);

        modelBuilder.Entity<WorkoutEntity>()
            .Property(x => x.Title)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<WorkoutEntity>()
            .Property(x => x.Description)
            .HasColumnType("varchar(1000)");

        modelBuilder.Entity<WorkoutEntity>()
            .Property(w => w.Type)
            .HasConversion<string>();

        modelBuilder.Entity<WorkoutEntity>()
            .Property(x => x.BodyPart)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<WorkoutEntity>()
            .Property(x => x.Equipment)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<WorkoutEntity>()
            .Property(w => w.WorkoutLevel)
            .HasConversion<string>();
    }
}