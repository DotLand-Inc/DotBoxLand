# DotBoxLand API Examples

## REST API Examples

### 1. Upload a Document

**cURL:**
```bash
curl -X POST "http://localhost:5000/api/documents/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/path/to/document.pdf" \
  -F "description=Project proposal document" \
  -F "tags=proposal,2024,important" \
  -F 'metadata={"department":"Sales","project":"Alpha"}'
```

**Response:**
```json
{
  "id": "507f1f77bcf86cd799439011",
  "fileName": "document.pdf",
  "originalFileName": "document.pdf",
  "contentType": "application/pdf",
  "fileSize": 1048576,
  "s3Key": "documents/uuid/document.pdf",
  "s3Bucket": "documents",
  "uploadedAt": "2024-11-02T14:30:00Z",
  "description": "Project proposal document",
  "tags": ["proposal", "2024", "important"],
  "metadata": {
    "department": "Sales",
    "project": "Alpha"
  }
}
```

### 2. Get Document by ID

**cURL:**
```bash
curl -X GET "http://localhost:5000/api/documents/507f1f77bcf86cd799439011"
```

### 3. Download Document

**cURL:**
```bash
curl -X GET "http://localhost:5000/api/documents/507f1f77bcf86cd799439011/download" \
  --output downloaded_document.pdf
```

### 4. Get Presigned Download URL

**cURL:**
```bash
curl -X GET "http://localhost:5000/api/documents/507f1f77bcf86cd799439011/download-url?expirationMinutes=30"
```

**Response:**
```json
{
  "url": "https://your-bucket.s3.amazonaws.com/documents/uuid/document.pdf?AWSAccessKeyId=...&Expires=...&Signature=...",
  "expiresIn": 30
}
```

### 5. Delete Document

**cURL:**
```bash
curl -X DELETE "http://localhost:5000/api/documents/507f1f77bcf86cd799439011"
```

## GraphQL API Examples

Access GraphQL Playground at: `http://localhost:5000/graphql`

### Query Examples

#### 1. Get All Documents

```graphql
query GetAllDocuments {
  documents {
    id
    fileName
    originalFileName
    contentType
    fileSize
    uploadedAt
    description
    tags
    metadata
    s3Key
    s3Bucket
  }
}
```

#### 2. Get Single Document

```graphql
query GetDocument {
  document(id: "507f1f77bcf86cd799439011") {
    id
    fileName
    description
    tags
    uploadedAt
    fileSize
  }
}
```

#### 3. Search Documents by Filename

```graphql
query SearchByFilename {
  searchDocuments(fileName: "proposal") {
    id
    fileName
    description
    uploadedAt
  }
}
```

#### 4. Search Documents by Tag

```graphql
query SearchByTag {
  searchDocuments(tag: "important") {
    id
    fileName
    tags
    uploadedAt
  }
}
```

#### 5. Search Documents by Date Range

```graphql
query SearchByDate {
  searchDocuments(fromDate: "2024-01-01T00:00:00Z") {
    id
    fileName
    uploadedAt
  }
}
```

#### 6. Combined Search

```graphql
query CombinedSearch {
  searchDocuments(
    fileName: "report"
    tag: "2024"
    fromDate: "2024-01-01T00:00:00Z"
  ) {
    id
    fileName
    description
    tags
    uploadedAt
  }
}
```

### Mutation Examples

#### 1. Update Document Metadata

```graphql
mutation UpdateDocument {
  updateDocument(
    id: "507f1f77bcf86cd799439011"
    description: "Updated project proposal - Final version"
    tags: ["proposal", "2024", "approved", "final"]
    metadata: {
      department: "Sales"
      project: "Alpha"
      status: "Approved"
      reviewer: "John Doe"
    }
  ) {
    id
    fileName
    description
    tags
    metadata
  }
}
```

#### 2. Update Only Description

```graphql
mutation UpdateDescription {
  updateDocument(
    id: "507f1f77bcf86cd799439011"
    description: "New description"
  ) {
    id
    description
  }
}
```

#### 3. Update Only Tags

```graphql
mutation UpdateTags {
  updateDocument(
    id: "507f1f77bcf86cd799439011"
    tags: ["urgent", "review-needed"]
  ) {
    id
    tags
  }
}
```

#### 4. Delete Document via GraphQL

```graphql
mutation DeleteDocument {
  deleteDocument(id: "507f1f77bcf86cd799439011")
}
```

## Using cURL with GraphQL

```bash
curl -X POST "http://localhost:5000/graphql" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "query { documents { id fileName uploadedAt } }"
  }'
```

```bash
curl -X POST "http://localhost:5000/graphql" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "query SearchDocs($fileName: String) { searchDocuments(fileName: $fileName) { id fileName } }",
    "variables": {
      "fileName": "proposal"
    }
  }'
```

## PowerShell Examples

### Upload Document

```powershell
$filePath = "C:\path\to\document.pdf"
$uri = "http://localhost:5000/api/documents/upload"

$form = @{
    file = Get-Item -Path $filePath
    description = "Test document"
    tags = "test,sample"
    metadata = '{"key":"value"}'
}

Invoke-RestMethod -Uri $uri -Method Post -Form $form
```

### Get Document

```powershell
$uri = "http://localhost:5000/api/documents/507f1f77bcf86cd799439011"
Invoke-RestMethod -Uri $uri -Method Get
```

### GraphQL Query

```powershell
$uri = "http://localhost:5000/graphql"
$body = @{
    query = "{ documents { id fileName } }"
} | ConvertTo-Json

Invoke-RestMethod -Uri $uri -Method Post -Body $body -ContentType "application/json"
```

## JavaScript/Fetch Examples

### Upload Document

```javascript
const formData = new FormData();
formData.append('file', fileInput.files[0]);
formData.append('description', 'My document');
formData.append('tags', 'important,2024');
formData.append('metadata', JSON.stringify({
  department: 'IT',
  project: 'Beta'
}));

fetch('http://localhost:5000/api/documents/upload', {
  method: 'POST',
  body: formData
})
.then(response => response.json())
.then(data => console.log(data));
```

### GraphQL Query

```javascript
fetch('http://localhost:5000/graphql', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    query: `
      query {
        documents {
          id
          fileName
          uploadedAt
        }
      }
    `
  })
})
.then(response => response.json())
.then(data => console.log(data));
```

## Testing Checklist

- [ ] MongoDB is running and accessible
- [ ] AWS credentials are configured in appsettings.json
- [ ] S3 bucket exists and is accessible
- [ ] Application runs without errors (`dotnet run`)
- [ ] Swagger UI is accessible at `/swagger`
- [ ] GraphQL Playground is accessible at `/graphql`
- [ ] Can upload a file via REST API
- [ ] Can retrieve document metadata
- [ ] Can download a file
- [ ] Can get presigned URL
- [ ] Can query documents via GraphQL
- [ ] Can search documents with filters
- [ ] Can update metadata via GraphQL
- [ ] Can delete documents
