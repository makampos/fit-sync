using FitSync.Domain.Entities;
using FitSync.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitSync.Infrastructure.Data;

public class FitSyncDbContext : DbContext
{
    public FitSyncDbContext(DbContextOptions<FitSyncDbContext> options) : base(options)
    {
    }

    public DbSet<WorkoutEntity> Workouts { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<WorkoutPlanEntity> WorkoutPlans { get; set; }
    public DbSet<WorkoutPlanWorkoutEntity> WorkoutPlanWorkouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<WorkoutEntity>());
        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<UserEntity>());
        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<WorkoutPlanEntity>());
        modelBuilder.ApplyConfiguration(new TrackableEntityConfiguration<WorkoutPlanWorkoutEntity>());

        modelBuilder.Entity<WorkoutEntity>()
            .ToTable("Workouts")
            .HasKey(w => w.Id);

        modelBuilder.Entity<WorkoutEntity>()
            .Property(w => w.Id)
            .ValueGeneratedOnAdd();

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

        modelBuilder.Entity<UserEntity>()
            .ToTable("Users")
            .HasKey(x => x.Id);

        modelBuilder.Entity<UserEntity>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<UserEntity>()
            .Property(x => x.Name)
            .HasColumnType("varchar(200)");

        modelBuilder.Entity<UserEntity>()
            .Property(x => x.Age)
            .HasColumnType("int");


        modelBuilder.Entity<WorkoutPlanEntity>()
            .ToTable("WorkoutPlans")
            .HasKey(wp => wp.Id);

        modelBuilder.Entity<WorkoutPlanEntity>()
            .Property(wp => wp.IsActive)
            .HasColumnType("boolean");

        modelBuilder.Entity<WorkoutPlanEntity>()
            .HasOne(wp => wp.User)
            .WithMany(u => u.WorkoutPlans)
            .HasForeignKey(wp => wp.UserId);

        // Configure many-to-many relationship between WorkoutPlans and Workouts through WorkoutPlanWorkout
       modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .ToTable("WorkoutPlanWorkouts");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .HasKey(wpw => new { wpw.WorkoutPlanId, wpw.WorkoutId });

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .HasOne(wpw => wpw.WorkoutPlan)
            .WithMany(wp => wp.WorkoutPlanWorkoutEntities)
            .HasForeignKey(wpw => wpw.WorkoutPlanId);

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .HasOne(wpw => wpw.Workout)
            .WithMany(w => w.WorkoutPlans)
            .HasForeignKey(wpw => wpw.WorkoutId);

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.Sets)
            .HasColumnType("int");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.RepsMin)
            .HasColumnType("int");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.RepsMax)
            .HasColumnType("int");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.Weight)
            .HasColumnType("int");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.RestBetweenSets)
            .HasColumnType("int");

        modelBuilder.Entity<WorkoutPlanWorkoutEntity>()
            .Property(wpw => wpw.Notes)
            .HasColumnType("varchar(1000)");
    }
}