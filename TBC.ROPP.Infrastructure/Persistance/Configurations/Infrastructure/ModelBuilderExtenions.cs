using Microsoft.EntityFrameworkCore;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure
{
    public static class ModelBuilderExtenions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration.Map(modelBuilder);
        }
    }
}