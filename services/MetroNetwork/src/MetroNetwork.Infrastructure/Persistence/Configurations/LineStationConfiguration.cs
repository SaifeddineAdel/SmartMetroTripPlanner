using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MetroNetwork.Domain.Features.LineStations;

namespace MetroNetwork.Infrastructure.Persistence.Configurations;

public sealed class LineStationConfiguration
    : IEntityTypeConfiguration<LineStation>
{
    public void Configure(EntityTypeBuilder<LineStation> builder)
    {
        builder.ToTable("LineStations");

        builder.HasKey(x => new
        {
            x.MetroLineId,
            x.StationId
        });

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.MetroLineId,
            x.Sequence
        })
        .IsUnique();

        builder.HasOne(x => x.MetroLine)
            .WithMany(x => x.LineStations)
            .HasForeignKey(x => x.MetroLineId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Station)
            .WithMany(x => x.LineStations)
            .HasForeignKey(x => x.StationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}