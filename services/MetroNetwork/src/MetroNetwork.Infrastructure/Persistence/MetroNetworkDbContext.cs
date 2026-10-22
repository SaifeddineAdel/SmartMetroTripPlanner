using MetroNetwork.Domain.Features.LineStations;
using MetroNetwork.Domain.Features.MetroLines;
using MetroNetwork.Domain.Features.Stations;
using Microsoft.EntityFrameworkCore;

namespace MetroNetwork.Infrastructure.Persistence
{
    public sealed class MetroNetworkDbContext : DbContext
    {
        public MetroNetworkDbContext(
            DbContextOptions<MetroNetworkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Stations => Set<Station>();

        public DbSet<MetroLine> MetroLines => Set<MetroLine>();

        public DbSet<LineStation> LineStations => Set<LineStation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(MetroNetworkDbContext).Assembly);
        }
    }
}
