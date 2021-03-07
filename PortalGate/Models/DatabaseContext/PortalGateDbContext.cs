using Microsoft.EntityFrameworkCore;

namespace PortalGate.Models.DatabaseContext
{
    public class PortalGateDbContext : DbContext
    {
        public DbSet<Railroad> RailroadList { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<UnitStartPageURI> UnitStartPageUries { get; set; }

        public PortalGateDbContext(DbContextOptions<PortalGateDbContext> options) : base(options)
        { }
    }
}
