using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class RelatedPersonConfiguration : EntityConfiguration<RelatedPerson>
{
    public override void Map(EntityTypeBuilder<RelatedPerson> builder)
    {
        builder.ToTable("RelatedPeople", DbSchemas.Main).HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);

        builder.HasOne(x => x.PhysicalPerson)
            .WithMany(s => s.RelatedPeopleList);

        builder.HasOne(x => x.RelatedPhysicalPerson)
            .WithMany(x => x.RelatedPeopleList);
    }
}