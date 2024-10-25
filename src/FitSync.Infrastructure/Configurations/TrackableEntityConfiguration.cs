using FitSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitSync.Infrastructure.Configurations;

public class TrackableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : TrackableEntity
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => DateTime.SpecifyKind(v.Value, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        builder.Property(e => e.DeletedAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => DateTime.SpecifyKind(v.Value, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );
    }
}