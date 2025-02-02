using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Persistance.Configurations.Infrastructure;
using TBC.ROPP.Infrastructure.Persistance.Schemas;

namespace TBC.ROPP.Infrastructure.Persistance.Configurations;

public class FileRecordsConfiguration : EntityConfiguration<FileRecord>
{
    public override void Map(EntityTypeBuilder<FileRecord> builder)
    {
        builder.ToTable("FileRecords", DbSchemas.FileStorage)
            .HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.UId);
    }
}