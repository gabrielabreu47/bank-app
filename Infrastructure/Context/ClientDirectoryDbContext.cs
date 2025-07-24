using ClientDirectory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public interface IClientDirectoryDbContext : IDbContext;
    public class ClientDirectoryDbContext(DbContextOptions<ClientDirectoryDbContext> options)
        : BaseDbContext(options), IClientDirectoryDbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasQueryFilter(p => !p.Deleted);
        }
    }
}
