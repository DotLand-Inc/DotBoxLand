# DotBoxLand - Document Management API

A .NET 8 Web API for managing documents with file uploads to AWS S3, metadata storage in MongoDB, and GraphQL/REST APIs for querying and retrieval.

## Features

- **File Upload**: Accept files with metadata via REST API
- **Cloud Storage**: Store files in AWS S3 bucket
- **Metadata Storage**: Store document metadata in MongoDB
- **GraphQL API**: Search and query documents using GraphQL
- **REST API**: Upload, download, and manage documents
- **Presigned URLs**: Generate temporary download URLs for secure file access

## Prerequisites

- .NET 8.0 SDK
- MongoDB (local or cloud instance)
- AWS Account with S3 access
- AWS credentials (Access Key and Secret Key)

## Configuration

Update the `appsettings.json` file with your configuration:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "DotBoxLandDb",
    "CollectionName": "Documents"
  },
  "AwsS3Settings": {
    "AccessKey": "YOUR_AWS_ACCESS_KEY",
    "SecretKey": "YOUR_AWS_SECRET_KEY",
    "BucketName": "your-bucket-name",
    "Region": "us-east-1"
  }
}
```

## Running the Application

```bash
dotnet restore
dotnet run
```

The API will be available at:
- REST API: `https://localhost:7XXX` or `http://localhost:5XXX`
- GraphQL: `https://localhost:7XXX/graphql`
- Swagger UI: `https://localhost:7XXX/swagger`

## REST API Endpoints

### Upload Document
```
POST /api/documents/upload
Content-Type: multipart/form-data

Form fields:
- file: The file to upload
- description: (optional) Description of the document
- tags: (optional) Comma-separated tags
- metadata: (optional) JSON string of key-value pairs
```

### Get Document by ID
```
GET /api/documents/{id}
```

### Download Document
```
GET /api/documents/{id}/download
```

### Get Presigned Download URL
```
GET /api/documents/{id}/download-url?expirationMinutes=60
```

### Delete Document
```
DELETE /api/documents/{id}
```

## GraphQL API

Access the GraphQL playground at `/graphql`

### Queries

**Get All Documents**
```graphql
query {
  documents {
    id
    fileName
    contentType
    fileSize
    uploadedAt
    description
    tags
    metadata
  }
}
```

**Get Document by ID**
```graphql
query {
  document(id: "document_id_here") {
    id
    fileName
    description
    tags
  }
}
```

**Search Documents**
```graphql
query {
  searchDocuments(
    fileName: "example"
    tag: "important"
    fromDate: "2024-01-01T00:00:00Z"
  ) {
    id
    fileName
    uploadedAt
  }
}
```

### Mutations

**Update Document**
```graphql
mutation {
  updateDocument(
    id: "document_id_here"
    description: "Updated description"
    tags: ["tag1", "tag2"]
    metadata: { key1: "value1", key2: "value2" }
  ) {
    id
    description
    tags
  }
}
```

**Delete Document**
```graphql
mutation {
  deleteDocument(id: "document_id_here")
}
```

## Project Structure

```
DotBoxLand/
├── Controllers/
│   └── DocumentsController.cs      # REST API endpoints
├── GraphQL/
│   ├── Query.cs                    # GraphQL queries
│   └── Mutation.cs                 # GraphQL mutations
├── Models/
│   ├── DocumentMetadata.cs         # Document model
│   ├── MongoDbSettings.cs          # MongoDB configuration
│   └── AwsS3Settings.cs            # AWS S3 configuration
├── Services/
│   ├── MongoDbService.cs           # MongoDB operations
│   └── S3Service.cs                # AWS S3 operations
├── Program.cs                      # Application entry point
└── appsettings.json                # Configuration file
```

## Example: Upload a Document using cURL

```bash
curl -X POST "https://localhost:7XXX/api/documents/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/your/document.pdf" \
  -F "description=Important document" \
  -F "tags=legal,contract" \
  -F 'metadata={"department":"HR","year":"2024"}'
```

## Example: Query with GraphQL using cURL

```bash
curl -X POST "https://localhost:7XXX/graphql" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "{ documents { id fileName uploadedAt } }"
  }'
```

## Security Considerations

- Update CORS policy in `Program.cs` for production
- Use environment variables or Azure Key Vault for sensitive configuration
- Implement authentication and authorization
- Add rate limiting for API endpoints
- Validate file types and sizes before upload

## Dependencies

- MongoDB.Driver
- AWSSDK.S3
- HotChocolate.AspNetCore
- HotChocolate.Data
- Swashbuckle.AspNetCore

## License

MIT License
