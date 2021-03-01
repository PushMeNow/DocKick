using System.Collections.Generic;
using System.IO;
using DocKick.Exceptions;
using Microsoft.AspNetCore.Http;

namespace DocKick.Categorizable.Extensions
{
    public static class FormFileExtensions
    {
        private static readonly Dictionary<string, string> _supportedImageTypes = new()
                                                                                 {
                                                                                     {
                                                                                         ".jpeg", "image/jpeg"
                                                                                     },
                                                                                     {
                                                                                         ".jpg", "image/jpeg"
                                                                                     },
                                                                                     {
                                                                                         ".png", "image/png"
                                                                                     }
                                                                                 };
        
        public static bool IsSupportedImageType(this IFormFile formFile)
        {
            ExceptionHelper.ThrowParameterNullIfEmpty(formFile.ContentType, nameof(formFile.ContentType));

            var fileExtension = Path.GetExtension(formFile.FileName);
            var isValidExtension = _supportedImageTypes.ContainsKey(fileExtension);
            var isValidContentType = isValidExtension && _supportedImageTypes[fileExtension] == formFile.ContentType;

            return isValidExtension && isValidContentType;
        }
    }
}