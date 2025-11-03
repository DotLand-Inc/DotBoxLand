using DotBoxLand.Storage.Api.Models;
using DotBoxLand.Storage.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotBoxLand.Storage.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController(
    MongoDbService mongoDbService,
    S3Service s3Service,
    ILogger<DocumentsController> logger)
    : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult<DocumentMetadata>> UploadDocument(
        [FromForm] IFormFile file,
        [FromForm] string? description,
        [FromForm] string? tags,
        [FromForm] string? metadata)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            // Upload to S3
            string s3Key;
            using (var stream = file.OpenReadStream())
            {
                s3Key = await s3Service.UploadFileAsync(stream, file.FileName, file.ContentType);
            }

            // Parse tags
            var tagsList = string.IsNullOrEmpty(tags)
                ? new List<string>()
                : tags.Split(',').Select(t => t.Trim()).ToList();

            // Parse metadata
            var metadataDict = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(metadata))
            {
                try
                {
                    metadataDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(metadata) ?? new();
                }
                catch
                {
                    logger.LogWarning("Failed to parse metadata JSON");
                }
            }

            // Create document metadata
            var document = new DocumentMetadata
            {
                FileName = file.FileName,
                OriginalFileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                S3Key = s3Key,
                S3Bucket = "documents",
                Description = description,
                Tags = tagsList,
                Metadata = metadataDict,
                UploadedAt = DateTime.UtcNow
            };

            // Save to MongoDB
            await mongoDbService.CreateAsync(document);

            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading document");
            return StatusCode(500, "Error uploading document");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DocumentMetadata>> GetDocument(string id)
    {
        var document = await mongoDbService.GetByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        return document;
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadDocument(string id)
    {
        try
        {
            var document = await mongoDbService.GetByIdAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            var stream = await s3Service.DownloadFileAsync(document.S3Key);

            return File(stream, document.ContentType, document.FileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading document");
            return StatusCode(500, "Error downloading document");
        }
    }

    [HttpGet("{id}/download-url")]
    public async Task<ActionResult<object>> GetDownloadUrl(string id, [FromQuery] int expirationMinutes = 60)
    {
        try
        {
            var document = await mongoDbService.GetByIdAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            var url = await s3Service.GetPresignedUrlAsync(document.S3Key, expirationMinutes);

            return Ok(new { url, expiresIn = expirationMinutes });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating download URL");
            return StatusCode(500, "Error generating download URL");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(string id)
    {
        try
        {
            var document = await mongoDbService.GetByIdAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            // Delete from S3
            await s3Service.DeleteFileAsync(document.S3Key);

            // Delete from MongoDB
            await mongoDbService.DeleteAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting document");
            return StatusCode(500, "Error deleting document");
        }
    }
}
