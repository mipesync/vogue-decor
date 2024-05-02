using System.Net;
using System.Net.Security;
using System.Security.Authentication;
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
                SaveImage(image, path);

                if (!mutable) return new[] { fullName, string.Empty };
                
                image.Mutate(r => r.Resize(0, 350));
                SaveImage(image, smallPath);

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

                var handler = new SocketsHttpHandler
                {
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        EnabledSslProtocols = SslProtocols.Tls12
                    }
                };                
                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(300);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 YaBrowser/24.4.0.0 Safari/537.36");
                client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ru,en;q=0.9");
                
                Console.WriteLine($"\nGetting file from url: {FileUrl}");
                using var response = await client.GetAsync(FileUrl, HttpCompletionOption.ResponseHeadersRead);
                Console.WriteLine($"Success! Response status code: {response.StatusCode}");
                
                using var image = await Image.LoadAsync(await response.Content.ReadAsStreamAsync());
                Console.WriteLine($"File info: \n\tSize: {image.Size}\n\tWidth: {image.Width}\n\tHeight: {image.Height}");

                SaveImage(image, path);
                
                if (!mutable) return new[] { fullName, string.Empty };
                
                image.Mutate(r => r.Resize(0, 350));
                SaveImage(image, smallPath);
                
                return new[] { fullName, smallFullName };
            }

            if (File is null && FileUrl is null)
                throw new BadRequestException("Пустой контент для загрузки");
            
            return new[] { fullName };
        }

        private async void SaveImage(Image image, string path)
        {
            var attempt = 0;
            Console.WriteLine($"Saving file to path: {path}");
            while (true)
            {
                if (!Path.Exists(Path.Combine(WebRootPath, path)))
                {
                    ++attempt;
                    if (attempt > 10)
                        break;
                    Console.WriteLine($"Attempt: {attempt}");
                    
                    await image.SaveAsync(Path.Combine(WebRootPath, path));
                }
                else
                {
                    Console.WriteLine("File successfully has been saved!");
                    break;
                }

                await Task.Delay(1000);
            }
        }
    }
}
