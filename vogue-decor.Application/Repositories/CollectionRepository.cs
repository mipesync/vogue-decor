﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;
using File = System.IO.File;

namespace vogue_decor.Application.Repositories
{
    /// <summary>
    /// Репозиторий коллекций
    /// </summary>
    /// <inheritdoc/>
    public class CollectionRepository : ICollectionRepository
    {
        private readonly IDBContext _dbContext;
        private readonly IFileUploader _uploader;
        private readonly IMapper _mapper;

        public CollectionRepository(IDBContext dbContext, IFileUploader uploader, IMapper mapper)
        {
            _dbContext = dbContext;
            _uploader = uploader;
            _mapper = mapper;
        }

        public async Task<GetCollectionsResponseDto> GetAll(GetColectionsDto dto, string hostUrl)
        {
            var collections = await _dbContext.Collections
                .AsNoTracking()
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

        public async Task<CollectionLookupDto> GetById(GetCollectionByIdDto dto,
            string hostUrl)
        {
            var collection = await _dbContext.Collections
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                    CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            var result = _mapper.Map<CollectionLookupDto>(collection);

            result.Preview = UrlParse(hostUrl, result);

            return result;
        }

        public async Task<CollectionLookupDto> GetByName(GetCollectionByNameDto dto,
            string hostUrl)
        {
            var collection = await _dbContext.Collections
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == dto.Name,
                    CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            var result = _mapper.Map<CollectionLookupDto>(collection);

            result.Preview = UrlParse(hostUrl, result);

            return result;
        }

        public async Task<AddCollectionResponseDto> Add(AddCollectionDto dto, string webRootPath)
        {
            var brand = await _dbContext.Brands.AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == dto.BrandId, CancellationToken.None);

            if (brand is null)
                throw new NotFoundException(brand);
            
            var collection = new Collection
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Url = string.Empty,
                BrandId = dto.BrandId
            };
            
            var fileName = await UploadImageAsync(webRootPath, dto.Url, dto.File, collection.Id);
        
            collection.Url = fileName;

            await _dbContext.Collections.AddAsync(collection, CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return new AddCollectionResponseDto
            {
                CollectionId = collection.Id
            };
        }

        public async Task Update(UpdateCollectionDto dto)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            collection.Name = dto.Name;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task Delete(DeleteCollectionDto dto)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            _dbContext.Collections.Remove(collection);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<UploadPreviewResponseDto> UploadPreview(UploadPreviewDto dto,
            string webRootPath, string hostUrl)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            var result = new UploadPreviewResponseDto();

            if (dto.Url is not null)
            {
                collection.Url = dto.Url;
                result.PreviewUrl = dto.Url;
            }
            else if (dto.File is not null)
            {
                _uploader.File = dto.File;
                _uploader.WebRootPath = webRootPath is null
                    ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                    : webRootPath;
                _uploader.AbsolutePath = collection.Id.ToString();

                var imageName = await _uploader.UploadFileAsync();

                collection.Url = imageName.FirstOrDefault()!;

                var imagePath = UrlParse(hostUrl, collection.Id.ToString(), imageName.FirstOrDefault()!);

                result.PreviewUrl = imagePath!;
            }
            else
            {
                throw new BadRequestException("Хотя бы одно из значений должно " +
                    "быть заполнено: ссылки или файлы");
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return result;
        }

        public async Task RemovePreview(RemovePreviewDto dto, string webRootPath)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            if (webRootPath is null)
                throw new ArgumentException("Корневой путь проекта не может быть пустым");
            
            File.Delete(Path.Combine(webRootPath, collection.Id.ToString()));
                
            collection.Url.Remove(0);

            collection.Url = string.Empty;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task AddProduct(AddProductToCollectionDto dto)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            collection.Products.Add(product);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteProduct(DeleteProductFromCollectionDto dto)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            collection.Products.Remove(product);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task SetIndex(SetCollectionIndexDto dto)
        {
            var collection = await _dbContext.Collections
                .FirstOrDefaultAsync(u => u.Id == dto.CollectionId,
                CancellationToken.None);

            if (collection is null)
                throw new NotFoundException(collection);

            collection.Index = dto.Index;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        private static string? UrlParse(string hostUrl, string baseDir, string fileName)
        {
            var uri = UrlParser.Parse(hostUrl, baseDir, fileName);
            return uri;
        }

        private static string? UrlParse(string hostUrl, CollectionLookupDto dto)
        {
            var uri = UrlParser.Parse(hostUrl, dto.Id.ToString(), dto.Preview);
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
    }
}
