using Microsoft.EntityFrameworkCore;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure
{
    public interface IEntityConfiguration
    {
        void Map(ModelBuilder builder);
    }
}