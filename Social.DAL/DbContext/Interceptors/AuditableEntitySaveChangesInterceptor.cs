using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Social.DAL.DbContext.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ILogger<AuditableEntitySaveChangesInterceptor> _logger;

        public AuditableEntitySaveChangesInterceptor(ILogger<AuditableEntitySaveChangesInterceptor> logger)
        {
            _logger = logger;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditableEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditableEntities(Microsoft.EntityFrameworkCore.DbContext context)
        {
            if (context == null) return;

            var entries = context.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity =entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    _logger.LogInformation($"Added {entry.Entity.GetType().Name} at {DateTime.UtcNow}");
                }
                else if (entry.State == EntityState.Modified)
                {
                    _logger.LogInformation($"Modified {entry.Entity.GetType().Name} at {DateTime.UtcNow}");
                }
            }
        }
    }
}
