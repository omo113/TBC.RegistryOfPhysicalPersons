using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Persistance.Abstractions;
using TBC.ROPP.Infrastructure.Persistance.Configurations;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Entities;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IMigrationDbContext
{
    public ApplicationDbContext()
    {
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        builder.HasDefaultSchema(DbSchemas.Main);

        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", DbSchemas.Identity);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", DbSchemas.Identity);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", DbSchemas.Identity);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", DbSchemas.Identity);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", DbSchemas.Identity);

        builder.AddConfiguration(new MigrationClientScriptConfiguration());
        builder.AddConfiguration(new PhysicalPersonConfiguration());
        builder.AddConfiguration(new RelatedPersonConfiguration());
        builder.AddConfiguration(new PhoneNumberConfiguration());
        builder.AddConfiguration(new FileRecordsConfiguration());
        builder.AddConfiguration(new CountryConfiguration());
    }
    public DbSet<EventLog> EventLogs { get; init; }
    public DbSet<MigrationClientScript> MigrationClientScripts { get; init; }
}