using EMS.Core.Data;
using EMS.Users.API.Models;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace EMS.Users.API.Data;

public sealed class UsersContext : DbContext, IUnitOfWork
{

    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Subscriber> Subscribers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Adresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ValidationResult>();
        //modelBuilder.Ignore<Event>();
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
            e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersContext).Assembly);
    }

    public async Task<bool> Commit()
    {
        var success = await base.SaveChangesAsync() > 0;
        return success;
    }
}