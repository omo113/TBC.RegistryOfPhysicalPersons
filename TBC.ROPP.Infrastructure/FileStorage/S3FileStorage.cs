using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.Shared.Settings;

namespace TBC.ROPP.Infrastructure.FileStorage;

public class S3FileStorage : IFileStorage, IS3Bucket
{
    private const string BucketName = "tbc";
    private const string Path = "person-files";
    private readonly ILogger<S3FileStorage> _logger;
    private readonly BasicAWSCredentials _credentials;
    private readonly AmazonS3Config _config;

    public S3FileStorage(ILogger<S3FileStorage> logger, IOptions<AWSSettings> options)
    {
        _logger = logger;
        var awsSettings = options.Value;
        _credentials = new BasicAWSCredentials(awsSettings.AccessKey, awsSettings.SecretKey);
        _config = new AmazonS3Config
        {
            ServiceURL = "http://localhost:9000", // Your local MinIO endpoint
            ForcePathStyle = true,              // Required for MinIO
            UseHttp = true,
            SignatureVersion = "v4",            // Use AWS Signature Version 4
        };
    }

    public async Task UploadS3FileAsync(IFormFile file, Guid uId, CancellationToken cancellationToken)
    {
        try
        {
            var s3FileName = string.Concat(Path, $"/{uId}-", file.FileName);

            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                Key = s3FileName,
                BucketName = BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            using var client = new AmazonS3Client(_credentials, _config);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest, cancellationToken);

            uploadRequest.UploadProgressEvent += (_, e) =>
            {
                _logger.LogInformation("Uploaded {TransferredBytes}/{TotalBytes} bytes ({PercentDone}% done)...",
                    e.TransferredBytes, e.TotalBytes, e.PercentDone);
            };
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error encountered on server. Message: '{@Message}'", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unknown error encountered. Message:'{@Message}'", ex.Message);
            throw;
        }
    }

    public async Task<MemoryStream> DownloadS3FileAsync(FileRecord fileRecord, CancellationToken cancellationToken = default)
    {
        var key = $"{Path}/{fileRecord.UId}-{fileRecord.Name}{fileRecord.Extension}";
        try
        {
            var downloadRequest = new GetObjectRequest()
            {
                BucketName = BucketName,
                Key = key,
            };
            using var client = new AmazonS3Client(_credentials, _config);

            using var responseStream = await client.GetObjectAsync(downloadRequest, cancellationToken);
            responseStream.WriteObjectProgressEvent += (_, e) =>
            {
                _logger.LogInformation("Uploaded {TransferredBytes}/{TotalBytes} bytes ({PercentDone}% done)...",
                                       e.TransferredBytes, e.TotalBytes, e.PercentDone);
            };
            var memoryStream = new MemoryStream();

            if (responseStream.HttpStatusCode is HttpStatusCode.OK)
            {
                await responseStream.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
            }

            if (memoryStream.ToArray().Length < 1)
                throw new FileNotFoundException($"The document '{key}' is not found");

            return memoryStream;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error encountered on server. File Key:{@Key}, Message: '{@Message}'", key, ex.Message);
            throw;
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError("Document not found. File Key:{@Key}, Message: '{@Message}'", key, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unknown error encountered. File Key:{@Key}, Message:'{@Message}'", key, ex.Message);
            throw;
        }
    }
    public async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
    {
        using var client = new AmazonS3Client(_credentials, _config);
        try
        {
            var bucketsResponse = await client.ListBucketsAsync(cancellationToken);
            if (!bucketsResponse.Buckets.Any(b =>
                    b.BucketName.Equals(BucketName, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogInformation("Bucket '{BucketName}' does not exist. Creating bucket...", BucketName);

                var request = new PutBucketRequest
                {
                    BucketName = BucketName,
                    UseClientRegion = true,
                };
                var response = await client.PutBucketAsync(request, cancellationToken);
                _logger.LogInformation("Bucket '{BucketName}' created successfully.", BucketName);
            }
            else
            {
                _logger.LogInformation("Bucket '{BucketName}' already exists.", BucketName);
            }
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError("Error encountered on server when checking/creating bucket '{BucketName}'. Message: '{Message}'", BucketName, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unknown error encountered when checking/creating bucket '{BucketName}'. Message: '{Message}'", BucketName, ex.Message);
            throw;
        }
    }
}