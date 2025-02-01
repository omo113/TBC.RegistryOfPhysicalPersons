using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Entities;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class MigrationClientScriptConfiguration : EntityConfiguration<MigrationClientScript>
{
    public override void Map(EntityTypeBuilder<MigrationClientScript> builder)
    {
        builder.ToTable("__MigrationClientScripts")
               .HasKey(x => x.Script);
    }
}