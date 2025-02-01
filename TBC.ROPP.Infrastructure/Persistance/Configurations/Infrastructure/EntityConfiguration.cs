using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure
{
    public abstract class EntityConfiguration<T> : IEntityMappingConfiguration<T>
        where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> builder);

        public virtual void Map(ModelBuilder builder)
        {
            var entityBuilder = builder.Entity<T>();

            Map(entityBuilder);
        }
    }
}