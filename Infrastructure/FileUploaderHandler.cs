using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.StaticFiles;
using PixelChopper.Application;

namespace Infrastructure;

public class FileUploadHandler: IUploadHandler
{
    private const long MaxFileSize = 10L * 1024L * 1024L; // 10MB

    public async Task HandleUploadAsync(HttpContext context)
    {
        if (!MultipartRequestHelper.IsMultipartContentType(context.Request.ContentType))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Not a multipart request");
            return;
        }

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(context.Request.ContentType), int.MaxValue);
        var reader = new MultipartReader(boundary, context.Request.Body);

        var section = await reader.ReadNextSectionAsync();

        if (section == null)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("No file uploaded.");
            return;
        }

        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

        if (hasContentDispositionHeader)
        {
            if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
            {
                using var targetStream = section.Body;

                if (!IsValidSize(targetStream))
                {
                    await RespondWithBadRequest(context, "File size exceeds the limit.");
                    return;
                }

                if (!IsImageFile(contentDisposition))
                {
                    await RespondWithBadRequest(context, "Invalid file type. Only image files are allowed.");
                    return;
                }
            }
        }
        else
        {
            await RespondWithBadRequest(context, "Invalid content disposition.");
            return;
        }

        await context.Response.WriteAsync("File uploaded successfully!");
    }

    private static bool IsImageFile(ContentDispositionHeaderValue contentDisposition)
    {
        var provider = new FileExtensionContentTypeProvider();
        var filename = contentDisposition.FileNameStar.Value ?? contentDisposition.FileName.Value;
        var extension = Path.GetExtension(filename);

        if (!provider.TryGetContentType(extension, out var mimeType))
        {
            return false;
        }

        return mimeType.StartsWith("image/");
    }

    private static bool IsValidSize(Stream stream)
    {
        return stream.Length <= MaxFileSize;
    }

    private static async Task RespondWithBadRequest(HttpContext context, string message)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync(message);
    }

    public Task HandleUploadAsync(HttpContent context)
    {
        throw new NotImplementedException();
    }
}