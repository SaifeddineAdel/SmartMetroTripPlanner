using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MetroNetwork.Domain.Features.MetroLines;

namespace MetroNetwork.Infrastructure.Persistence.Configurations;

public sealed class MetroLineConfiguration
    : IEntityTypeConfiguration<MetroLine>
{
    public void Configure(EntityTypeBuilder<MetroLine> builder)
    {
        builder.ToTable("MetroLines");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
