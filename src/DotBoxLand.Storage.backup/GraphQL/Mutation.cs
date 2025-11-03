using DotBoxLand.Storage.Api.Models;
using DotBoxLand.Storage.Api.Services;

namespace DotBoxLand.Storage.Api.GraphQL;

public class Mutation
{
    public async Task<DocumentMetadata> UpdateDocument(
        string id,
        string? description,
        List<string>? tags,
        Dictionary<string, string>? metadata,
        [Service] MongoDbService mongoDbService)
    {
        var document = await mongoDbService.GetByIdAsync(id);
        if (document == null)
        {
            throw new Exception("Document not found");
        }

        if (description != null)
        {
            document.Description = description;
        }

        if (tags != null)
        {
            document.Tags = tags;
        }

        if (metadata != null)
        {
            document.Metadata = metadata;
        }

        await mongoDbService.UpdateAsync(id, document);
        return document;
    }

    public async Task<bool> DeleteDocument(string id, [Service] MongoDbService mongoDbService, [Service] S3Service s3Service)
    {
        var document = await mongoDbService.GetByIdAsync(id);
        if (document == null)
        {
            return false;
        }

        await s3Service.DeleteFileAsync(document.S3Key);
        await mongoDbService.DeleteAsync(id);
        return true;
    }
}
