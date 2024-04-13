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

        public static bool IsImageFile(string contentType)
        {
            var imageMimeTypes = new List<string> { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/svg+xml" };
            return contentType != null && imageMimeTypes.Contains(contentType);
        }
    }
