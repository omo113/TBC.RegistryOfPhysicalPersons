using Microsoft.Extensions.DependencyInjection;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.MigrationClient.Abstractions;

namespace TBC.ROPP.MigrationClient.Scripts;

public class S3BucketMigration(IServiceScopeFactory scopeFactory) : PostMigrationClientScript
{
    private const string BucketName = "TBC";

    public override async Task RunAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var s3Bucket = scope.ServiceProvider.GetRequiredService<IS3Bucket>();
        await s3Bucket.EnsureBucketExistsAsync(BucketName, cancellationToken);
    }
}