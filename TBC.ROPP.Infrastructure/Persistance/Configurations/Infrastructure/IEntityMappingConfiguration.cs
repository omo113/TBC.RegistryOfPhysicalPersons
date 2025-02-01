using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure
{
    public interface IEntityMappingConfiguration<T> : IEntityConfiguration
        where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}