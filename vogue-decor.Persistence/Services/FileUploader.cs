using System.Net;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace vogue_decor.Persistence.Services
{
    /// <inheritdoc/>
    public class FileUploader : IFileUploader
    {
        public IFormFile? File { get; set; } = null;
        public string? FileUrl { get; set; } = null;
        public string AbsolutePath { get; set; } = string.Empty;
        public string WebRootPath { get; set; } = string.Empty;

        [Obsolete("Obsolete")]
        public async Task<string[]> UploadFileAsync(bool mutable = true)
        {
            string fileExtension;
            string fileNameHash;
            var fullName = string.Empty;
            string smallFullName;
            string path;
            string smallPath;

            Directory.CreateDirectory(Path.Combine(WebRootPath, AbsolutePath));
            
            if (File is not null)
            {
                fileExtension = Path.GetExtension(File.FileName);
                fileNameHash = Guid.NewGuid().ToString();
                fullName = $"{fileNameHash}_default{fileExtension}";
                smallFullName = $"{fileNameHash}_small{fileExtension}";
                path = Path.Combine(AbsolutePath, fullName);
                smallPath = Path.Combine(AbsolutePath, smallFullName);
                
                using var image = await Image.LoadAsync(File.OpenReadStream());
                await image.SaveAsync(Path.Combine(WebRootPath, path));

                if (!mutable) return new[] { fullName, string.Empty };
                
                image.Mutate(r => r.Resize(0, 350));
                await image.SaveAsync(Path.Combine(WebRootPath, smallPath));

                return new[] { fullName, smallFullName };
            }
            if (FileUrl is not null)
            {
                fileExtension = Path.GetExtension(FileUrl);
                fileNameHash = Guid.NewGuid().ToString();
                fullName = $"{fileNameHash}_default{fileExtension}";
                smallFullName = $"{fileNameHash}_small{fileExtension}";
                path = Path.Combine(AbsolutePath, fullName);
                smallPath = Path.Combine(AbsolutePath, smallFullName);

                using var client = new HttpClient();
                await using var stream = await client.GetStreamAsync(FileUrl);
                
                
                using var image = await Image.LoadAsync(stream);
                await image.SaveAsync(Path.Combine(WebRootPath, path));
                
                if (!mutable) return new[] { fullName, string.Empty };
                
                image.Mutate(r => r.Resize(0, 350));
                await image.SaveAsync(Path.Combine(WebRootPath, smallPath));

                return new[] { fullName, smallFullName };
            }

            if (File is null && FileUrl is null)
                throw new BadRequestException("Пустой контент для загрузки");
            
            return new[] { fullName };
        }
    }
}
