using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class PhysicalPersonConfiguration : EntityConfiguration<PhysicalPerson>
{
    public override void Map(EntityTypeBuilder<PhysicalPerson> builder)
    {
        builder.ToTable("PhysicalPeople").HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);

        builder.Property(x => x.Name).HasMaxLength(64);
        builder.Property(x => x.LastName).HasMaxLength(64);
        builder.Property(x => x.PersonalNumber).HasMaxLength(32);

        builder.HasMany(x => x.PhoneNumbers)
               .WithOne();

    }
}