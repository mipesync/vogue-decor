using System.Net;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Interfaces;
using Microsoft.AspNetCore.Http;

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
        public async Task<string> UploadFileAsync()
        {
            string fileExtension;
            string fileNameHash;
            var fullName = string.Empty;
            string path;

            Directory.CreateDirectory(Path.Combine(WebRootPath, AbsolutePath));
            
            if (File is not null)
            {
                fileExtension = Path.GetExtension(File.FileName);
                fileNameHash = Guid.NewGuid().ToString();
                fullName = fileNameHash + fileExtension;
                path = Path.Combine(AbsolutePath, fullName);
          
                await using var fileStream = new FileStream(Path.Combine(WebRootPath, path), FileMode.Create);
                await File.CopyToAsync(fileStream);

                return fullName;
            }
            if (FileUrl is not null)
            {
                fileExtension = ".jpg";
                fileNameHash = Guid.NewGuid().ToString();
                fullName = fileNameHash + fileExtension;
                path = Path.Combine(AbsolutePath, fullName);

                using var client = new WebClient();
                var fileData = client.DownloadData(FileUrl);

                await using var fs = new FileStream(Path.Combine(WebRootPath, path), FileMode.Create);
                await using var w = new BinaryWriter(fs);
                w.Write(fileData);
                
                return fullName;
            }

            if (File is null && FileUrl is null)
                throw new BadRequestException("Пустой контент для загрузки");
            
            return fullName;
        }
    }
}
