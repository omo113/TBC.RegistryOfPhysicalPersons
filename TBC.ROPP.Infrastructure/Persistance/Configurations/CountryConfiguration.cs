using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class CountryConfiguration : EntityConfiguration<City>
{
    public override void Map(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities", DbSchemas.Main).HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
    }
}