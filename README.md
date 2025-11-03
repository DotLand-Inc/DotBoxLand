# DotLandBox

DotLandBox is a collection of microservices built with .NET 8 for cloud-based document management and storage solutions.

## Project Structure

This repository contains the following services:

### DotBoxLand.Storage
A pure AWS S3 storage API service that provides REST endpoints for document management. This service handles:
- File uploads to AWS S3
- Document metadata storage in MongoDB
- Document retrieval and presigned URL generation
- Document deletion and management

**Location**: `src/DotBoxLand.Storage/`
**Documentation**: See [DotBoxLand.Storage README](src/DotBoxLand.Storage/README.md)

## Technology Stack

- **.NET 8.0**: Core framework
- **AWS S3**: Cloud storage for documents
- **MongoDB**: Metadata storage
- **Swagger/OpenAPI**: API documentation
- **REST API**: Standard HTTP endpoints

## Getting Started

Each service has its own README with specific setup instructions. Navigate to the service directory for detailed information:

```bash
cd src/DotBoxLand.Storage
```

## Common Prerequisites

- .NET 8.0 SDK or higher
- MongoDB (local or cloud instance)
- AWS Account with S3 access

## Architecture

DotLandBox follows a microservices architecture where each service is independently deployable and focused on a specific domain:

- **Storage Service**: Handles all document storage operations with AWS S3
- Future services can be added as needed (e.g., Authentication, Analytics, etc.)

## Contributing

1. Each service should be self-contained with its own dependencies
2. Follow .NET coding standards and conventions
3. Update service-specific README files when making changes
4. Ensure all tests pass before committing

## License

MIT License
