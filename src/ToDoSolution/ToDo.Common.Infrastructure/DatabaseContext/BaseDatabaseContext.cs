using Microsoft.EntityFrameworkCore;
using ToDo.Common.Domain.Entities;

namespace ToDo.Common.Infrastructure.DatabaseContext
{
    public abstract class BaseDatabaseContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Entity.SetCreatedAt(DateTime.Now);
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Entity.SetUpdatedAt(DateTime.Now);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
