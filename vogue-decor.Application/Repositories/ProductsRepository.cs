﻿using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Common.Services;
using vogue_decor.Application.DTOs.BrandDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using vogue_decor.Domain.Enums;
using vogue_decor.Domain.Interfaces;
using File = System.IO.File;

namespace vogue_decor.Application.Repositories
{
    /// <summary>
    /// Репозиторий товаров
    /// </summary>
    /// <inheritdoc/>
    public class ProductsRepository : IProductsRepository
    {
        private readonly IDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IFileParser _fileParser;
        private readonly UserManager<User> _userManager;
        private readonly IFileUploader _uploader;
        private readonly IProductCodeGenerator _codeGenerator;

        public ProductsRepository(IDBContext dbContext, IMapper mapper,
            IFileParser fileParser, UserManager<User> userManager, IFileUploader uploader, IProductCodeGenerator codeGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileParser = fileParser;
            _userManager = userManager;
            _uploader = uploader;
            _codeGenerator = codeGenerator;
        }

        public async Task<CreateProductResponseDto> CreateAsync(CreateProductDto dto, string webRootPath, string hostUrl)
        {
            var brand = GetBrandsByIdAsync(new [] { dto.BrandId }).Result.FirstOrDefault();
            if (brand is null)
                throw new NotFoundException(brand);

            var collection = await _dbContext.Collections.FirstOrDefaultAsync(i => i.Id == dto.CollectionId, CancellationToken.None);
            if (collection is null)
                throw new NotFoundException(collection);
            if (collection.BrandId != brand.Id)
                throw new BadRequestException($"Brand '{brand.Id}' doesn't contains collection {collection.Id}");
            
            var colors = await GetColorsByIdAsync(dto.Colors);

            await GetCategoriesByIdAsync(dto.Categories);
            
            var product = _mapper.Map<Product>(dto);
            product.PublicationDate = DateTime.UtcNow;
            product.SearchVector = NpgsqlTsVector.Parse(product.Name);
            product.Code = _codeGenerator.GenerateCode(product.Name, brand.Name, 
                product.PublicationDate.ToString(CultureInfo.CurrentCulture), product.ProductType.ToString(), 
                product.Article, ParseFilterNameToString(colors.Select(c => c.Name)));
            product.Brand = brand;
            
            var urls = product.Urls.ToArray();
            product.Urls.Clear();
            
            await _dbContext.Products.AddAsync(product, CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
            
            if (urls.Length > 0)
                await UploadImageAsync(new UploadImageDto { Urls = urls.ToList(), ProductId = product.Id}, webRootPath, hostUrl);

            return new CreateProductResponseDto { ProductId = product.Id };
        }

        public async Task<ImportProductsResponseDto> ImportAsync(ImportProductsDto dto, string webRootPath, string hostUrl)
        {
            var products = await _fileParser.ParseAsync(dto.File);
            var ids = new List<Guid>();

            foreach (var product in products.ProductList)
            {
                var result = await CreateAsync(product, webRootPath, hostUrl);
                ids.Add(result.ProductId);
            }
            
            return new ImportProductsResponseDto
            {
                ProductIds = ids
            };
        }

        public async Task UpdateAsync(UpdateProductDto dto)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            product = _mapper.Map(dto, product);
            
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task DeleteAsync(DeleteProductDto dto)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<GetProductsResponseDto> GetAllAsync(GetAllProductsDto dto, string hostUrl)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .ToListAsync(CancellationToken.None);

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products
                .Skip(dto.From)
                .Take(dto.Count)
                .ToList(), 
                dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            return new GetProductsResponseDto 
            { 
                Products = productsShort, 
                TotalCount = products.Count(),
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
        }

        public async Task<ProductResponseDto> GetByIdAsync(GetProductByIdDto dto, string hostUrl)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId,
                    CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);
            
            product.Brand = await _dbContext.Brands.FirstOrDefaultAsync(u => u.Id == product.BrandId, CancellationToken.None);

            var counts = GetCounts(dto.UserId);

            var result = _mapper.Map<ProductResponseDto>(product, opt =>
            {
                opt.Items["userId"] = dto.UserId;
            });

            if (product.CollectionId != null)
            {
                var collection = await _dbContext.Collections.FirstOrDefaultAsync(u => u.Id == product.CollectionId, CancellationToken.None);
                
                result.Collection = collection is null ? null : new ShortCollectionDto
                {
                    Id = collection.Id,
                    Name = collection.Name
                };
            }
            
            var brand = await _dbContext.Brands.FirstOrDefaultAsync(u => u.Id == product.BrandId, CancellationToken.None);
                
            result.Brand = brand is null ? null : new ShortBrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };

            result = UrlParse(result, hostUrl);
            result.CartCount = counts.cart;
            result.FavouritesCount = counts.favourites;

            return result;
        }
        
        public async Task<GetProductsResponseDto> GetByCriteriaAsync(GetProductByCriteriaDto dto, 
            string hostUrl)
        {
            var filterExpression = BuildLinqExpression(dto);
            
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, expression: filterExpression);

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products.Skip(dto.From).Take(dto.Count).ToList(), 
                dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            string? collectionName = null;
            if (dto.CollectionsId is not null && dto.CollectionsId.Length == 1)
                collectionName = await GetCollectionName(dto.CollectionsId.First());
                
            return new GetProductsResponseDto 
            { 
                Products = productsShort,
                CollectionName = collectionName,
                TotalCount = products.Count,
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
        }

        private async Task<string?> GetCollectionName(Guid collectionId)
        {
            var collection = await _dbContext.Collections.FirstOrDefaultAsync(u => u.Id == collectionId, CancellationToken.None);

            return collection?.Name;
        }

        public async Task<GetFiltersCountResponseDto> GetFiltersCountAsync(GetProductByCriteriaDto dto)
        {
            var filterExpression = BuildLinqExpression(dto);
    
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, expression: filterExpression);

            var result = new GetFiltersCountResponseDto 
            { 
                TotalCount = products.Count,
                Colors = GetFiltersByColor(products),
                ProductTypes = GetFiltersByProductType(products),
                Categories = GetFiltersByCategory(products, dto.ProductTypes),
                Styles = GetFiltersByStyle(products),
                MinPrice = products.Select(p => p.Price).Min(),
                MaxPrice = products.Select(p => p.Price).Max(),
                MinLength = products
                    .Where(p => p.Length != null && p.Length.Any())
                    .Select(p => p.Length!.Min())
                    .DefaultIfEmpty(0)
                    .Min(),
                MaxLength = products
                    .Where(p => p.Length != null && p.Length.Any())
                    .Select(p => p.Length!.Length == 2 ? p.Length[1] : p.Length[0])
                    .DefaultIfEmpty(0)
                    .Max(),
                MinHeight = products
                    .Where(p => p.Height != null && p.Height.Any())
                    .Select(p => p.Height![0])
                    .DefaultIfEmpty(0)
                    .Min(),
                MaxHeight = products
                    .Where(p => p.Height is { Length: 2 })
                    .Select(p => p.Height![1])
                    .DefaultIfEmpty(0)
                    .Max(),
                MinWidth = products
                    .Where(p => p.Width != null && p.Width.Any())
                    .Select(p => p.Width![0])
                    .DefaultIfEmpty(0)
                    .Min(),
                MaxWidth = products
                    .Where(p => p.Width is { Length: 2 })
                    .Select(p => p.Width![1])
                    .DefaultIfEmpty(0)
                    .Max(),
                MinDiameter = products.Select(p => p.Diameter).Min(),
                MaxDiameter = products.Select(p => p.Diameter).Max(),
                MinIndent = products.Select(p => p.Indent).Min(),
                MaxIndent = products.Select(p => p.Indent).Max(),
                MinLampCount = products.Select(p => p.LampCount).Min(),
                MaxLampCount = products.Select(p => p.LampCount).Max(),
                ChandelierTypes = GetFiltersByChandelierTypes(products),
                Materials = GetFiltersByMaterials(products),
                PictureMaterial = GetFiltersByPictureMaterials(products),
                Brands = GetFiltersByBrands(products),
                Collections = GetFiltersByCollections(products)
            };

            return result;
        }

        public async Task SetIndexAsync(Guid productId, int index)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            product.Index = index;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<ProductsCompareResponseDto> CompareAsync(Guid firstId, Guid secondId)
        {
            var firstProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == firstId, CancellationToken.None);

            if (firstProduct is null)
                throw new NotFoundException($"Товар с идентификатором \"{firstId}\" не найден");
            
            var secondProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == secondId, CancellationToken.None);

            if (secondProduct is null)
                throw new NotFoundException($"Товар с идентификатором \"{secondId}\" не найден");

            var result = new ProductsCompareResponseDto
            {
                Name = firstProduct.Name != secondProduct.Name,
                ProductType = firstProduct.ProductType != secondProduct.ProductType,
                Article = firstProduct.Article != secondProduct.Article,
                Price = decimal.Compare(firstProduct.Price, secondProduct.Price) != 0,
                Colors = !firstProduct.Colors.SequenceEqual(secondProduct.Colors),
                Diameter = decimal.Compare(firstProduct.Diameter ?? 0, secondProduct.Diameter ?? 0) != 0,
                Height = CompareNullableArrays(firstProduct.Height, secondProduct.Height),
                Length = CompareNullableArrays(firstProduct.Length, secondProduct.Length),
                Width = CompareNullableArrays(firstProduct.Width, secondProduct.Width)
            };

            if (secondProduct.PictureMaterial != null)
                result.PictureMaterial = firstProduct.PictureMaterial != null 
                    && !firstProduct.PictureMaterial.SequenceEqual(secondProduct.PictureMaterial);
            result.Indent = decimal.Compare(firstProduct.Indent ?? 0, secondProduct.Indent ?? 0) != 0;
            result.Plinth = firstProduct.Plinth != secondProduct.Plinth;
            result.LampCount = firstProduct.LampCount != secondProduct.LampCount;
            if (secondProduct.ChandelierTypes != null)
                result.ChandelierTypes = firstProduct.ChandelierTypes != null 
                    && !firstProduct.ChandelierTypes.SequenceEqual(secondProduct.ChandelierTypes);
            if (secondProduct.Styles != null)
                result.Styles = firstProduct.Styles != null 
                    && !firstProduct.Styles.SequenceEqual(secondProduct.Styles);
            if (secondProduct.Materials != null)
                result.Materials = firstProduct.Materials != null 
                    && !firstProduct.Materials.SequenceEqual(secondProduct.Materials);

            return result;
        }

        public async Task<GetProductsResponseDto> GetByArticleAsync(GetByArticleDto dto, string hostUrl)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .Where(u => u.Article.Contains(dto.Article)).ToListAsync(CancellationToken.None);

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products                
                    .Skip(dto.From)
                    .Take(dto.Count)
                    .ToList(), dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            return new GetProductsResponseDto
            {
                Products = productsShort,
                TotalCount = products.Count(),
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
        }

        public async Task<GetProductsResponseDto> GetByCodeAsync(GetByCodeDto dto, string hostUrl)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .Where(u => u.Code == dto.Code).ToListAsync(CancellationToken.None);

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products                
                    .Skip(dto.From)
                    .Take(dto.Count)
                    .ToList(), dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            return new GetProductsResponseDto
            {
                Products = productsShort,
                TotalCount = products.Count(),
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
        }

        public async Task<GetProductsResponseDto> GetByCollectionIdAsync(GetProductByCollectionIdDto dto,
            string hostUrl)
        {
            var products = await GetOrderedProductsAsync(sortType: dto.SortType);
            products = products
                .Where(u => u.CollectionId == dto.CollectionId)
                .ToList();

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products                
                .Skip(dto.From)
                .Take(dto.Count)
                .ToList(), dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            return new GetProductsResponseDto
            {
                Products = productsShort,
                TotalCount = products.Count(),
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
        }
        
        public async Task<GetProductsResponseDto> SearchAsync(SearchProductDto dto, string hostUrl)
        {
            var searchQuery = dto.SearchQuery.Trim();
            var article = "";
            
            if (searchQuery.Length > 6 && long.TryParse(searchQuery, out var code))
            {
                if (code.ToString().Length == 8)
                {
                    return await GetByCodeAsync(new GetByCodeDto
                    {
                        Code = searchQuery,
                        UserId = dto.UserId
                    }, hostUrl);
                }
                
                article = await _dbContext.Products.Select(p => p.Article)
                    .FirstOrDefaultAsync(p => p.Contains(code.ToString()), CancellationToken.None);
            }
            
            if (article == string.Empty)
                article = FindArticle(dto.SearchQuery);
            
            var strQueryWithoutArticle = article != string.Empty ? string.Join("", searchQuery.Replace($" {article}", "")).Trim() : searchQuery;
            
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, searchQuery: strQueryWithoutArticle, article: article, take: dto.Count, skip: dto.From);
            
            if (products.Count == 0 && article!.Trim() == searchQuery.Trim())
            {
                if (article != string.Empty)
                {
                    var articleProducts = await GetByArticleAsync(new GetByArticleDto
                    {
                        Article = article,
                        UserId = dto.UserId,
                        From = dto.From,
                        Count = dto.Count
                    }, hostUrl);
    
                    return articleProducts;
                }
            }
            
            var productsShort = ProductMapper.Map(_mapper, 
                products
                    .Skip(dto.From)
                    .Take(dto.Count)
                    .ToList(), 
                dto.UserId);

            productsShort = UrlParse(productsShort, hostUrl);

            return new GetProductsResponseDto
            {
                Products = productsShort,
                TotalCount = products.Count
            };
        }

        public async Task AddToCartAsync(AddToCartDto dto)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(
                u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user is null)
            {
                throw new NotFoundException(user);
            }

            var entity = _mapper.Map<ProductUser>(dto);
            entity.Product = product;
            entity.User = user;

            await _dbContext.ProductUsers.AddAsync(entity, CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task RemoveFromCartAsync(RemoveFromCartDto dto)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(
                u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user is null)
            {
                throw new NotFoundException(user);
            }

            var entity = await _dbContext.ProductUsers.Where(
                u => u.ProductId == dto.ProductId && u.UserId == dto.UserId).ToListAsync();

            if (dto.IsRemovingAll)
            {
                var count = _dbContext.ProductUsers.Count(u => u.ProductId == dto.ProductId);
                for (var i = 0; i < count; i++)
                {
                    _dbContext.ProductUsers.RemoveRange(entity);
                }
            }
            else
                _dbContext.ProductUsers.Remove(entity.First());

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task AddToFavouriteAsync(AddToFavouriteDto dto)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(
                u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user is null)
            {
                throw new NotFoundException(user);
            }

            var entity = _mapper.Map<Favourite>(dto);
            entity.Product = product;
            entity.User = user;

            await _dbContext.Favourites.AddAsync(entity, CancellationToken.None);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task RemoveFromFavouriteAsync(RemoveFromFavouriteDto dto)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(
                u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user is null)
            {
                throw new NotFoundException(user);
            }

            var entity = await _dbContext.Favourites.FirstOrDefaultAsync(
                u => u.ProductId == dto.ProductId && u.UserId == dto.UserId,
                CancellationToken.None);

            _dbContext.Favourites.Remove(entity!);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<UploadImageResponseDto> UploadImageAsync(UploadImageDto dto,
            string webRootPath, string hostUrl)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            var result = new UploadImageResponseDto();

            _uploader.WebRootPath = webRootPath is null
                ? throw new ArgumentException("Корневой путь проекта " +
                                              "не может быть пустым")
                : webRootPath;
            _uploader.AbsolutePath = product.Id.ToString();

            if (dto.Urls?.Count > 0)
            {
                foreach (var url in dto.Urls)
                {
                    _uploader.FileUrl = url;

                    var imageName = await _uploader.UploadFileAsync();

                    product.Urls.AddRange(imageName);

                    var imagePath = UrlParse(imageName, product.Id.ToString(), hostUrl);
                    result.Files.AddRange(imagePath);
                }
            }
            else if (dto.Files!.Count > 0)
            {
                foreach (var image in dto.Files!)
                {
                    _uploader.File = image;

                    var imageName = await _uploader.UploadFileAsync();

                    product.Urls.AddRange(imageName);

                    var imagePath = UrlParse(imageName, product.Id.ToString(), hostUrl);
                    result.Files.AddRange(imagePath);
                }
            }
            else
            {
                throw new BadRequestException("Хотя бы одно из значений должно " +
                    "быть заполнено: ссылки или файлы");
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);

            return result;
        }

        public async Task RemoveImageAsync(RemoveImageDto dto, string webRootPath)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            if (!dto.FileName.Contains("http"))
            {
                var extension = Path.GetExtension(dto.FileName);
                var fileId = dto.FileName.Substring(0, dto.FileName.IndexOf('_'));
                
                File.Delete(Path.Combine(
                    webRootPath is null
                    ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                    : webRootPath, 
                    product.Id.ToString(), fileId + "_default" + extension));
                
                File.Delete(Path.Combine(
                    webRootPath is null
                        ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                        : webRootPath, 
                    product.Id.ToString(), fileId + "_small" + extension));
                
                product.Urls.Remove(fileId + "_default" + extension);
                product.Urls.Remove(fileId + "_small" + extension);
            }

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task UpdateRating(UpdateRatingDto dto)
        {
            /*var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);*/
            
            
        }

        public async Task SetFileOrderAsync(SetFileOrderDto dto)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            product.Urls = dto.FileNames;

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        private (int favourites, int cart) GetCounts(Guid userId)
        {
            var favouritesCount = _dbContext.Favourites.Count(u => u.UserId == userId);
            var cartCount = _dbContext.ProductUsers.Count(u => u.UserId == userId);

            return (favouritesCount, cartCount);
        }

        private static string? UrlParse(string fileName, string productId, string hostUrl)
        {
            var uri = UrlParser.Parse(hostUrl, productId, fileName);

            return uri;
        }

        private static string[]? UrlParse(string[] fileName, string productId, string hostUrl)
        {
            var uri = UrlParser.Parse(hostUrl, productId, fileName);

            return uri;
        }

        private static List<ProductShortResponseDto> UrlParse(List<ProductShortResponseDto> products,
            string hostUrl)
        {
            foreach (var product in products)
            {
                if (product.Urls is not null)
                {
                    var parsedUrls = new List<string>();

                    foreach (var t in product.Urls)
                    {
                        if (t.Contains("small"))
                            parsedUrls.Add(UrlParser.Parse(hostUrl, product.Id.ToString(), t)!);
                    }

                    product.Urls = parsedUrls.ToArray();
                }
            }
            return products;
        }

        private static ProductResponseDto UrlParse(ProductResponseDto product,
            string hostUrl)
        {
            var files = new List<FileDto>();
            
            foreach (var file in product.Files)
            {
                if (file.Url.Contains("default"))
                    files.Add(new FileDto
                    {
                        Name = file.Name,
                        Url = UrlParser.Parse(hostUrl, product.Id.ToString(), file.Url)!
                    });
            }

            product.Files = files; 
            
            return product;
        }

        private TResult[] FindEnums<TResult>(string query) where TResult : Enum
        {
            List<TResult> enums = new List<TResult>();
            
            var strings = query.ToLower().Split(" ");
            foreach (var str in strings)
            {
                foreach (TResult colorItem in Enum.GetValues(typeof(TResult)))
                {
                    if (str.Contains(colorItem.ToString().ToLower()))
                    {
                        enums.Add(colorItem);
                    }
                }
            }

            return enums.ToArray();
        }

        private string FindArticle(string query)
        {
            const string pattern = @"\d|/";
            var strings = query.ToLower().Split(" ");

            var result = string.Empty;

            foreach (var str in strings)
            {
                if (Regex.IsMatch(str, pattern) && result == string.Empty)
                    result = str;
                else if (result != string.Empty)
                {
                    result += $" {str}";
                }
            }
            
            return result.Trim();
        }

        private async Task<List<Product>> GetOrderedProductsAsync(SortTypes? sortType,
            Expression<Func<Product, bool>>? expression = null, string? searchQuery = null, string? article = null,
            int take = 10000, int skip = 0)
        {
            List<Product> products;

            var normalizedQuery = string.Empty; 
            
            if (searchQuery != null)
                normalizedQuery = NormalizeQuery(searchQuery);
            
            Expression<Func<Product, bool>> defaultExpression =
                u => searchQuery == null || u.SearchVector.Matches(EF.Functions.ToTsQuery("russian", normalizedQuery));

            var productsQuery = _dbContext.Products.AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .Where(expression ?? defaultExpression);
            
            switch (sortType)
            {
                case SortTypes.BY_RATING_ASC:
                    productsQuery = productsQuery
                        .OrderBy(u => u.Rating);
                    break;
                case SortTypes.BY_RATING_DESC:
                    productsQuery = productsQuery
                        .OrderByDescending(u => u.Rating);
                    break;
                case SortTypes.BY_NOVELTY_ASC:
                    productsQuery = productsQuery
                        .OrderBy(u => u.PublicationDate);
                    break;
                case SortTypes.BY_NOVELTY_DESC:
                    productsQuery = productsQuery
                        .OrderByDescending(u => u.PublicationDate);
                    break;
                case SortTypes.BY_POPULARITY:
                    productsQuery = productsQuery
                        .OrderByDescending(u => u.PurchasedCount);
                    break;
                case SortTypes.BY_PRICE_ASC:
                    productsQuery = productsQuery
                        .OrderBy(u => u.Price);
                    break;
                case SortTypes.BY_PRICE_DESC:
                    productsQuery = productsQuery
                        .OrderByDescending(u => u.Price);
                    break;
            }

            products = await productsQuery.Skip(skip).Take(take).ToListAsync(CancellationToken.None);

            return string.IsNullOrEmpty(article) ? products : products.Where(p => p.Article.Contains(article))
                .Skip(skip)
                .Take(take).ToList();
        }

        private string NormalizeQuery(string query)
        {
            return query.Replace(" ", " & ");
        }

        private async Task<List<Brand>> GetBrandsByIdAsync(Guid[] brandsId)
        {
            return await _dbContext.Brands.Where(b => brandsId.Contains(b.Id)).ToListAsync();
        }

        private async Task<List<Color>> GetColorsByIdAsync(IReadOnlyCollection<int> colorIds)
        {
            var colors = await _dbContext.Colors.Where(c => colorIds.Contains(c.Id)).ToListAsync();

            if (colors.Count == 0)
                throw new NotFoundException("Ни один цвет не был найден");

            if (colors.Count != colorIds.Count)
                throw new NotFoundException("Один или несколько цветов не были найдены");

            return colors;
        }
        
        private async Task<List<Category>> GetCategoriesByIdAsync(IReadOnlyCollection<int> categoryIds)
        {
            var categories = await _dbContext.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();

            if (categories.Count == 0)
                throw new NotFoundException("Ни одна категория не была найдена");

            if (categories.Count != categoryIds.Count)
                throw new NotFoundException("Одна или несколько категорий не были найдены");

            return categories;
        }

        private static Expression<Func<Product, bool>> BuildLinqExpression(GetProductByCriteriaDto dto)
        {
            return u => 
                (dto.Colors == null || u.Colors.Any(c => dto.Colors.Contains(c))) &&
                (dto.ProductTypes == null || dto.ProductTypes.Contains(u.ProductType)) &&
                (dto.Categories == null || u.Types.Any(c => dto.Categories.Contains(c))) &&
                (dto.Styles == null || u.Styles == null || u.Styles.Any(c => dto.Styles.Contains(c))) &&
                (u.Price >= dto.MinPrice) &&
                (u.Price <= dto.MaxPrice) &&
                ((u.Diameter ?? 0m) >= dto.MinDiameter) &&
                ((u.Diameter ?? decimal.MaxValue) <= dto.MaxDiameter) &&
                ((u.Length == null || u.Length.Length == 0) || ((u.Length.Length == 1 && dto.MaxLength >= u.Length[0] && dto.MinLength <= u.Length[0]) || 
                    (u.Length.Length == 2 && dto.MaxLength >= u.Length[0] && dto.MinLength <= u.Length[1]))) &&
                ((u.Width == null || u.Width.Length == 0) || ((u.Width.Length == 1 && dto.MaxWidth >= u.Width[0] && dto.MinWidth <= u.Width[0]) || 
                    (u.Width.Length == 2 && dto.MaxWidth >= u.Width[0] && dto.MinWidth <= u.Width[1]))) &&
                ((u.Height == null || u.Height.Length == 0) || ((u.Height.Length == 1 && dto.MaxHeight >= u.Height[0] && dto.MinHeight <= u.Height[0]) || 
                    (u.Height.Length == 2 && dto.MaxHeight >= u.Height[0] && dto.MinHeight <= u.Height[1]))) &&
                ((u.Indent ?? 0m) >= dto.MinIndent) &&
                ((u.Indent ?? decimal.MaxValue) <= dto.MaxIndent) &&
                ((u.LampCount ?? 0) >= dto.MinLampCount) &&
                ((u.LampCount ?? int.MaxValue) <= dto.MaxLampCount) &&
                (dto.ChandelierTypes == null || u.ChandelierTypes == null || u.ChandelierTypes.Length == 0 || u.ChandelierTypes.Any(c => dto.ChandelierTypes.Contains(c))) &&
                (dto.Materials == null || u.Materials == null || u.Materials.Length == 0 || u.Materials.Any(c => dto.Materials.Contains(c))) &&
                (dto.PictureMaterial == null || u.PictureMaterial == null || u.PictureMaterial.Length == 0 || u.PictureMaterial.Any(c => dto.PictureMaterial.Contains(c))) &&
                (dto.IsSale == null || (u.Discount ?? 0) > 0 == dto.IsSale) &&
                (dto.BrandsId == null || dto.BrandsId.Contains(u.BrandId)) &&
                (dto.CollectionsId == null || dto.CollectionsId.Contains((Guid)u.CollectionId!));
        }

        private string ParseFilterNameToString(IEnumerable<string> names)
        {
            return string.Join(",", names);
        }
        
        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByColor(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => p.Colors, _dbContext.Colors.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByProductType(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => new[] { p.ProductType }, _dbContext.ProductTypes.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByCategory(IEnumerable<Product> products, int[]? productTypes)
        {
            var categories = productTypes is not null
                ? _dbContext.Categories.AsNoTracking().Where(c => productTypes.Cast<int?>().Contains(c.ProductTypeId))
                : _dbContext.Categories.AsNoTracking();

            return GetFiltersByCriteria(products, p => p.Types, categories);
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByStyle(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => p.Styles ?? Enumerable.Empty<int>(), _dbContext.Styles.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByChandelierTypes(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => p.ChandelierTypes ?? Enumerable.Empty<int>(), _dbContext.ChandelierTypes.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByMaterials(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => p.Materials ?? Enumerable.Empty<int>(), _dbContext.Materials.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByPictureMaterials(IEnumerable<Product> products)
        {
            return GetFiltersByCriteria(products, p => p.PictureMaterial ?? Enumerable.Empty<int>(), _dbContext.Materials.AsNoTracking());
        }

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByCriteria<T>(
            IEnumerable<Product> products, 
            Func<Product, IEnumerable<int>> productSelector, 
            IQueryable<T> criteria) where T : IBaseFilterItem
        {
            return criteria
                .AsEnumerable()
                .Select(c => new { Id = c.Id, Name = c.Name, Count = products.Count(p => productSelector(p).Contains(c.Id)) })
                .Where(x => x.Count > 0)
                .ToDictionary(x => x.Id, x => new GetFiltersCountResponseDto.FilterDto { Count = x.Count, Name = x.Name });
        }

        private Dictionary<Guid, GetFiltersCountResponseDto.FilterDto> GetFiltersByBrands(IEnumerable<Product> products)
        {
            return GetFiltersBySecondaryEntities(products, p => p.BrandId, _dbContext.Brands.AsNoTracking());
        }

        private Dictionary<Guid, GetFiltersCountResponseDto.FilterDto> GetFiltersByCollections(IEnumerable<Product> products)
        {
            return GetFiltersBySecondaryEntities(products, p => p.CollectionId ?? Guid.Empty, _dbContext.Collections.AsNoTracking());
        }

        private Dictionary<Guid, GetFiltersCountResponseDto.FilterDto> GetFiltersBySecondaryEntities<T>(
            IEnumerable<Product> products, 
            Func<Product, Guid> productSelector, 
            IQueryable<T> criteria) where T : ISecondaryEntity
        {
            return criteria
                .AsEnumerable()
                .Select(c => new { Id = c.Id, Name = c.Name, Count = products.Count(p => productSelector(p) == c.Id) })
                .Where(x => x.Count > 0)
                .ToDictionary(x => x.Id, x => new GetFiltersCountResponseDto.FilterDto { Count = x.Count, Name = x.Name });
        }

        private static bool CompareNullableArrays(decimal[]? firstArray, decimal[]? secondArray)
        {
            if (firstArray == null || secondArray == null)
                return firstArray != secondArray;
            return firstArray.Length != secondArray.Length || firstArray.Where((t, i) => t != secondArray[i]).Any();
        }
    }
}
