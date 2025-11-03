using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using DotBoxLand.Storage.Api.Models;
using Microsoft.Extensions.Options;

namespace DotBoxLand.Storage.Api.Services;

public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IOptions<AwsS3Settings> awsS3Settings)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsS3Settings.Value.Region)
        };

        _s3Client = new AmazonS3Client(
            awsS3Settings.Value.AccessKey,
            awsS3Settings.Value.SecretKey,
            config
        );

        _bucketName = awsS3Settings.Value.BucketName;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var key = $"documents/{Guid.NewGuid()}/{fileName}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = fileStream,
            Key = key,
            BucketName = _bucketName,
            ContentType = contentType,
            CannedACL = S3CannedACL.Private
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest);

        return key;
    }

    public async Task<Stream> DownloadFileAsync(string key)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(request);
        return response.ResponseStream;
    }

    public async Task<string> GetPresignedUrlAsync(string key, int expirationMinutes = 60)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
        };

        return await Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task DeleteFileAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request);
    }
}
