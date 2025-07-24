using ClientDirectory.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public abstract class BaseDbContext(DbContextOptions options) : DbContext(options), IDbContext
    {
        private void SetAuditEntities()
        {
            foreach (var entry in ChangeTracker.Entries<IBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        entry.Entity.Deleted = false;
                        break;

                    case EntityState.Deleted:

                        entry.State = EntityState.Modified;
                        entry.Entity.Deleted = true;
                        break;
                }
            }
        }
        public override int SaveChanges()
        {
            SetAuditEntities();
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetAuditEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = type.ClrType;

                if (typeof(IBase).IsAssignableFrom(clrType) && type.BaseType == null)
                {
                    modelBuilder.SetSoftDeleteFilter(clrType);
                }
            }
        }
    }
}
