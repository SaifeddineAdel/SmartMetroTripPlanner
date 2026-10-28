using MetroNetwork.Domain.Features.LineStations;
using MetroNetwork.Domain.Features.MetroLines;
using MetroNetwork.Domain.Features.Stations;
using MetroNetwork.Domain.Features.Users;
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

        public DbSet<User> Users => Set<User>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(MetroNetworkDbContext).Assembly);
        }
    }
}
