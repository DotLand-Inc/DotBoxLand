using DotBoxLand.Storage.Api.Models;
using DotBoxLand.Storage.Api.Services;

namespace DotBoxLand.Storage.Api.GraphQL;

public class Query
{
    public async Task<List<DocumentMetadata>> GetDocuments([Service] MongoDbService mongoDbService)
        => await mongoDbService.GetAllAsync();

    public async Task<DocumentMetadata?> GetDocument(string id, [Service] MongoDbService mongoDbService)
        => await mongoDbService.GetByIdAsync(id);

    public async Task<List<DocumentMetadata>> SearchDocuments(
        string? fileName,
        string? tag,
        DateTime? fromDate,
        [Service] MongoDbService mongoDbService)
        => await mongoDbService.SearchAsync(fileName, tag, fromDate);
}
