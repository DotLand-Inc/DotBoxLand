# DotBoxLand.Storage - AWS S3 Document Storage API

A .NET 8 Web API for managing documents with file uploads to AWS S3 and metadata storage in MongoDB. This service provides a pure storage API for document management.

## Features

- **File Upload**: Accept files with metadata via REST API
- **Cloud Storage**: Store files in AWS S3 bucket
- **Metadata Storage**: Store document metadata in MongoDB
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

## Project Structure

```
DotBoxLand.Storage/
├── Controllers/
│   └── DocumentsController.cs      # REST API endpoints
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

## Security Considerations

- Update CORS policy in `Program.cs` for production
- Use environment variables or Azure Key Vault for sensitive configuration
- Implement authentication and authorization
- Add rate limiting for API endpoints
- Validate file types and sizes before upload

## Dependencies

- MongoDB.Driver
- AWSSDK.S3
- Swashbuckle.AspNetCore

## License

MIT License
