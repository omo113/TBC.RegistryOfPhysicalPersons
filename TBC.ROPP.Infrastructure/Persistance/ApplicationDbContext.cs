using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
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
    }
    public DbSet<EventLog> EventLogs { get; init; }
}