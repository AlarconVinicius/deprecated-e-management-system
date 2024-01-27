using EMS.Core.Data;
using EMS.Subscription.API.Model;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EMS.Subscription.API.Data;

public class SubscriptionContext : DbContext, IUnitOfWork
{
    public SubscriptionContext(DbContextOptions<SubscriptionContext> options)
            : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Plan> Plans { get; set; }
    public DbSet<PlanUser> PlanUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Ignore<ValidationResult>();

        //modelBuilder.Entity<PlanUser>()
        //    .HasIndex(c => c.ClientId)
        //    .HasDatabaseName("IDX_Cliente");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var success = await base.SaveChangesAsync() > 0;
        return success;
    }
}