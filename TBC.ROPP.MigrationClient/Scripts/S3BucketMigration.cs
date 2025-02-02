using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.MigrationClient.Abstractions;

namespace TBC.ROPP.MigrationClient.Scripts;

public class S3BucketMigration(IS3Bucket s3Bucket) : PostMigrationClientScript
{

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await s3Bucket.EnsureBucketExistsAsync(cancellationToken);
    }
}