using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Utilities;

    public static class RequestHelper
    {

        public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }

        public static bool IsImageMimeType(string contentType)

        {
            var imageMimeTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/svg+xml" };
            return contentType != null && imageMimeTypes.Contains(contentType);
        }

        public static bool IsImageFileExtension(string fileName)
        {
            var imageExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" };
            return fileName != null && imageExtensions.Contains(Path.GetExtension(fileName));
        }

    }
