using DotBoxLand.Storage.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotBoxLand.Storage.Api.Services;

public class MongoDbService
{
    private readonly IMongoCollection<DocumentMetadata> _documentsCollection;

    public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _documentsCollection = mongoDatabase.GetCollection<DocumentMetadata>(mongoDbSettings.Value.CollectionName);
    }

    public async Task<List<DocumentMetadata>> GetAllAsync() =>
        await _documentsCollection.Find(_ => true).ToListAsync();

    public async Task<DocumentMetadata?> GetByIdAsync(string id) =>
        await _documentsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<List<DocumentMetadata>> SearchAsync(string? fileName = null, string? tag = null, DateTime? fromDate = null)
    {
        var filterBuilder = Builders<DocumentMetadata>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(fileName))
        {
            filter &= filterBuilder.Regex(x => x.FileName, new MongoDB.Bson.BsonRegularExpression(fileName, "i"));
        }

        if (!string.IsNullOrEmpty(tag))
        {
            filter &= filterBuilder.AnyEq(x => x.Tags, tag);
        }

        if (fromDate.HasValue)
        {
            filter &= filterBuilder.Gte(x => x.UploadedAt, fromDate.Value);
        }

        return await _documentsCollection.Find(filter).ToListAsync();
    }

    public async Task CreateAsync(DocumentMetadata document) =>
        await _documentsCollection.InsertOneAsync(document);

    public async Task UpdateAsync(string id, DocumentMetadata document) =>
        await _documentsCollection.ReplaceOneAsync(x => x.Id == id, document);

    public async Task DeleteAsync(string id) =>
        await _documentsCollection.DeleteOneAsync(x => x.Id == id);
}
