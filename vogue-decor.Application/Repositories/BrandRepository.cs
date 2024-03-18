using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs.BrandDTOs;
using vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using File = System.IO.File;

namespace vogue_decor.Application.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IDBContext _dbContext;
    private readonly IFileUploader _uploader;
    private readonly IMapper _mapper;

    public BrandRepository(IDBContext dbContext, IFileUploader uploader, IMapper mapper)
    {
        _dbContext = dbContext;
        _uploader = uploader;
        _mapper = mapper;
    }

    public async Task<CreateBrandResponseDto> CreateAsync(CreateBrandDto dto, string webRootPath, string hostUrl)
    {
        var brand = new Brand { Id = Guid.NewGuid(), Name = dto.Name, Url = string.Empty };

        var fileName = await UploadImageAsync(webRootPath, dto.Url, dto.File, brand.Id);
        
        brand.Url = fileName;
        
        await _dbContext.Brands.AddAsync(brand, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        return new CreateBrandResponseDto { BrandId = brand.Id};
    }

    public async Task UpdateAsync(UpdateBrandDto dto, string webRootPath, string hostUrl)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == dto.BrandId, CancellationToken.None);
        if (brand is null)
            throw new NotFoundException(brand);
        
        if (!string.IsNullOrEmpty(dto.Name))
            brand.Name = dto.Name;

        if (dto.File is not null || !string.IsNullOrEmpty(dto.Url))
        {
            if (!string.IsNullOrEmpty(brand.Url))
                RemoveImage(brand, webRootPath);
            brand.Url = await UploadImageAsync(webRootPath, dto.Url, dto.File, brand.Id);
        }

        _dbContext.Brands.Update(brand);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteAsync(Guid brandId, string webRootPath)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId, CancellationToken.None);
        if (brand is null)
            throw new NotFoundException(brand);
        
        RemoveImage(brand, webRootPath);
        
        _dbContext.Brands.Remove(brand);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteImageAsync(DeleteBrandImageDto dto, string webRootPath)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == dto.BrandId, CancellationToken.None);
        if (brand is null)
            throw new NotFoundException(brand);
        
        RemoveImage(brand, webRootPath);
        brand.Url = string.Empty;
        
        _dbContext.Brands.Update(brand);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<GetBrandsResponseDto> GetAllAsync(string hostUrl, int skip, int take)
    {
        var brands = await _dbContext.Brands
            .AsNoTracking()
            .Skip(skip).Take(take == 0 ? 10 : take)
            .ToListAsync(CancellationToken.None);

        var result = new GetBrandsResponseDto();

        if (brands.Count <= 0) return result;
        
        foreach (var brand in brands)
        {
            result.Brands.Add(new BrandResponseDto
            {
                Id = brand.Id,
                Name = brand.Name,
                File = new FileDto
                {
                    Url = UrlParse(hostUrl, brand.Id.ToString(), brand.Url)!,
                    Name = brand.Url
                }
            });
        }

        result.Total = await _dbContext.Brands.CountAsync();

        return result;
    }

    public async Task<BrandResponseDto> GetByIdAsync(Guid brandId, string hostUrl)
    {
        var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId, CancellationToken.None);
        if (brand is null)
            throw new NotFoundException(brand);

        var result = new BrandResponseDto
        {
            Id = brand.Id,
            Name = brand.Name,
            File = new FileDto
            {
                Url = UrlParse(hostUrl, brand.Id.ToString(), brand.Url)!,
                Name = brand.Url
            }
        };

        return result;
    }

    public async Task<GetCollectionsResponseDto> GetCollectionsAsync(GetCollectionsByBrandIdDto dto, string hostUrl)
    {
        await GetByIdAsync(dto.BrandId, hostUrl);
            
        var collections = await _dbContext.Collections
            .AsNoTracking()
            .Where(c => c.BrandId == dto.BrandId)
            .OrderBy(u => u.Index)
            .Skip(dto.From)
            .Take(dto.Count)
            .ProjectTo<CollectionLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
                
        var totalCount = await _dbContext.Collections.ToListAsync();

        var result = new GetCollectionsResponseDto
        {
            Collections = collections,
            TotalCount = totalCount.Count()
        };

        result = UrlParse(result, hostUrl);

        return result;
    }


    private async Task<string> UploadImageAsync(string webRootPath, string? url, IFormFile? file, Guid brandId)
    {
        _uploader.WebRootPath = webRootPath is null
            ? throw new ArgumentException("Корневой путь проекта " +
                                          "не может быть пустым")
            : webRootPath;
        _uploader.AbsolutePath = brandId.ToString();
        _uploader.FileUrl = url;
        _uploader.File = file;

        var imageName = await _uploader.UploadFileAsync(mutable: false);
        
        return imageName.First();
    }
    
    public void RemoveImage(Brand brand, string webRootPath)
    {
        if (brand.Url.Contains("http")) return;
        
        var extension = Path.GetExtension(brand.Url);
        var fileId = brand.Url.Substring(0, brand.Url.IndexOf('_'));
                
        File.Delete(Path.Combine(
            webRootPath is null
                ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                : webRootPath, 
            brand.Id.ToString(), fileId + "_default" + extension));
    }
    
    private static string? UrlParse(string hostUrl, string baseDir, string fileName)
    {
        var uri = UrlParser.Parse(hostUrl, baseDir, fileName);
        return uri;
    }
    
    private static GetCollectionsResponseDto UrlParse(GetCollectionsResponseDto dto,
        string hostUrl)
    {
        for (int i = 0; i < dto.Collections.Count; i++)
        {
            dto.Collections[i].Preview = UrlParser.Parse(hostUrl,
                dto.Collections[i].Id.ToString(),
                dto.Collections[i].Preview!);
        }

        return dto;
    }
}