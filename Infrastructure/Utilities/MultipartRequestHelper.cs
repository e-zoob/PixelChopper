using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;

namespace Infrastructure.Utilities
{
    public static class MultipartRequestHelper
    {
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
        }

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }

        public static bool IsImageFile(ContentDispositionHeaderValue contentDisposition)
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
    }
}