using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class PhoneNumberConfiguration : EntityConfiguration<PhoneNumber>
{
    public override void Map(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.ToTable("PhoneNumbers", DbSchemas.Main).HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
    }
}