using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DotBoxLand.Storage.Api.Models;

public class DocumentMetadata
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("fileName")]
    public string FileName { get; set; } = string.Empty;

    [BsonElement("originalFileName")]
    public string OriginalFileName { get; set; } = string.Empty;

    [BsonElement("contentType")]
    public string ContentType { get; set; } = string.Empty;

    [BsonElement("fileSize")]
    public long FileSize { get; set; }

    [BsonElement("s3Key")]
    public string S3Key { get; set; } = string.Empty;

    [BsonElement("s3Bucket")]
    public string S3Bucket { get; set; } = string.Empty;

    [BsonElement("uploadedAt")]
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("metadata")]
    public Dictionary<string, string> Metadata { get; set; } = new();

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();

    [BsonElement("description")]
    public string? Description { get; set; }
}
