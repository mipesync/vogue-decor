using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs.FilesDTOs;
using vogue_decor.Application.DTOs.FilesDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using File = vogue_decor.Domain.File;

namespace vogue_decor.Application.Repositories;

/// <inheritdoc />
public class FilesRepository : IFilesRepository
{
    private readonly IDBContext _dbContext;
    private readonly IFileUploader _uploader;

    public FilesRepository(IFileUploader uploader, IDBContext dbContext)
    {
        _uploader = uploader;
        _dbContext = dbContext;
    }

    public async Task<UploadFileResponseDto> Upload(UploadFileDto dto, string webRootPath, string hostUrl)
    {
        var result = new UploadFileResponseDto();

        var files = new List<File>();

        _uploader.WebRootPath = webRootPath is null
            ? throw new ArgumentException("Корневой путь проекта " +
                                          "не может быть пустым")
            : webRootPath;
        _uploader.AbsolutePath = "storage";

        if (dto.Urls.Count > 0)
        {
            foreach (var url in dto.Urls)
            {
                _uploader.FileUrl = url;

                var imageName = await _uploader.UploadFileAsync();
                var imagePath = UrlParse(imageName, hostUrl);

                result.Urls.Add(new FileDto
                {
                    Name = imageName,
                    Url = imagePath
                });
                
                files.Add(new File
                {
                    Name = imageName
                });
            }
        }
        if (dto.Files.Count > 0)
        {
            foreach (var image in dto.Files)
            {
                _uploader.File = image;

                var imageName = await _uploader.UploadFileAsync();
                var imagePath = UrlParse(imageName, hostUrl);

                result.Urls.Add(new FileDto
                {
                    Name = imageName,
                    Url = imagePath
                });
                
                files.Add(new File
                {
                    Name = imageName
                });
            }
        }
        else
        {
            throw new BadRequestException("Хотя бы одно из значений должно " +
                                          "быть заполнено: ссылки и/или файлы");
        }

        await _dbContext.Files.AddRangeAsync(files, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return result;
    }

    public async Task Delete(string fileName, string webRootPath)
    {
        var file = await _dbContext.Files
            .FirstOrDefaultAsync(u => u.Name == fileName, CancellationToken.None);

        if (file is null)
            throw new NotFoundException("Файл не был найден");
        
        System.IO.File.Delete(Path.Combine(
            webRootPath is null
                ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                : webRootPath, 
            "storage", fileName));

        _dbContext.Files.Remove(file);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<GetAllFilesResponseDto> GetAll(GetAllFilesDto dto, string hostUrl)
    {
        var files = await _dbContext.Files
            .AsNoTracking()
            .Skip(dto.From)
            .Take(dto.Count)
            .ToListAsync();

        var result = new GetAllFilesResponseDto();
        
        if (files.Count == 0)
            return result;

        result.Files.AddRange(files.Select(file => new FileDto { Name = file.Name, Url = UrlParse(file.Name, hostUrl)! }));
        
        return result;
    }
    
    private static string? UrlParse(string fileName, string hostUrl)
    {
        var uri = UrlParser.Parse(hostUrl, "storage", fileName);

        return uri;
    }
}