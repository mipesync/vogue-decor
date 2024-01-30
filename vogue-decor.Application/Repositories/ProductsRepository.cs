using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.Common.Services;
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

            var counts = GetCounts(dto.UserId);

            var result = _mapper.Map<ProductResponseDto>(product, opt =>
            {
                opt.Items["userId"] = dto.UserId;
            });

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

            return new GetProductsResponseDto 
            { 
                Products = productsShort, 
                TotalCount = products.Count,
                CartCount = counts.cart,
                FavouritesCount = counts.favourites
            };
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
                Prices = GetRangeFilterByPrice(products),
                Length = GetRangeFilterByLength(products),
                Diameter = GetRangeFilterByDiameter(products),
                Height = GetRangeFilterByHeight(products),
                Width = GetRangeFilterByWidth(products),
                Indent = GetRangeFilterByIndent(products),
                LampCount = GetRangeFilterByLampCount(products),
                AdditionalParams = GetFiltersByAdditionalParams(products),
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
                Height = decimal.Compare(firstProduct.Height ?? 0, secondProduct.Height ?? 0) != 0,
                Length = decimal.Compare(firstProduct.Length ?? 0, secondProduct.Length ?? 0) != 0,
                Width = decimal.Compare(firstProduct.Width ?? 0, secondProduct.Width ?? 0) != 0
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
            
            if (long.TryParse(searchQuery, out _))
            {
                var product = await GetByCodeAsync(new GetByCodeDto
                {
                    Code = searchQuery,
                    UserId = dto.UserId
                }, hostUrl);

                return product;
            }
            
            var article = FindArticle(dto.SearchQuery);
            
            var strQueryWithoutArticle = article != string.Empty ? string.Join("", searchQuery.Replace($" {article}", "")).Trim() : searchQuery;
            
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, searchQuery: strQueryWithoutArticle, article: article);
            
            if (products.Count == 0 && article == searchQuery.Trim())
            {
                if (article != string.Empty)
                {
                    var articleProducts = await GetByArticleAsync(new GetByArticleDto
                    {
                        Article = article,
                        UserId = dto.UserId
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
            for (int i = 0; i < products.Count; i++)
            {
                products[i].Urls = UrlParser.Parse(hostUrl, products[i].Id.ToString(), products[i].Urls);
            }
            return products;
        }

        private static ProductResponseDto UrlParse(ProductResponseDto product,
            string hostUrl)
        {
            for (int i = 0; i < product.Files.Count; i++)
            {
                product.Files[i].Url = UrlParser.Parse(hostUrl, product.Id.ToString(), product.Files[i].Url)!;
            }
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
            Expression<Func<Product, bool>>? expression = null, string? searchQuery = null, string? article = null)
        {
            List<Product> products;

            var normalizedQuery = string.Empty; 
            
            if (searchQuery != null)
                normalizedQuery = NormalizeQuery(searchQuery);
            
            Expression<Func<Product, bool>> defaultExpression =
                u => searchQuery == null || u.SearchVector.Matches(EF.Functions.ToTsQuery("russian", normalizedQuery));

            switch (sortType)
            {
                case SortTypes.BY_RATING_ASC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderBy(u => u.Rating)
                        .ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_RATING_DESC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderByDescending(u => u.Rating).ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_NOVELTY_ASC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderBy(u => u.PublicationDate)
                        .ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_NOVELTY_DESC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderByDescending(u => u.PublicationDate)
                        .ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_POPULARITY:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderByDescending(u => u.PurchasedCount)
                        .ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_PRICE_ASC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderBy(u => u.Price)
                        .ToListAsync(CancellationToken.None);
                    break;
                case SortTypes.BY_PRICE_DESC:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .OrderByDescending(u => u.Price)
                        .ToListAsync(CancellationToken.None);
                    break;
                default:
                    products = await _dbContext.Products.AsNoTracking()
                        .Include(u => u.Favourites)
                        .Include(u => u.ProductUsers)
                        .Where(expression ?? defaultExpression)
                        .ToListAsync(CancellationToken.None);
                    break;
            }

            return string.IsNullOrEmpty(article) ? products : products.Where(p => p.Article.Contains(article)).ToList();
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
                (dto.Prices == null || u.Price >= dto.Prices.MinValue && u.Price <= dto.Prices.MaxValue) &&
                (dto.Length == null || u.Length >= dto.Length.MinValue && u.Length <= dto.Length.MaxValue) &&
                (dto.Diameter == null || u.Diameter >= dto.Diameter.MinValue && u.Diameter <= dto.Diameter.MaxValue) &&
                (dto.Height == null || u.Height >= dto.Height.MinValue && u.Height <= dto.Height.MaxValue) &&
                (dto.Width == null || u.Width >= dto.Width.MinValue && u.Width <= dto.Width.MaxValue) &&
                (dto.Indent == null || u.Indent >= dto.Indent.MinValue && u.Indent <= dto.Indent.MaxValue) &&
                (dto.LampCount == null || u.LampCount >= dto.LampCount.MinValue && u.LampCount <= dto.LampCount.MaxValue) &&
                (u.ChandelierTypes == null || dto.AdditionalParams == null || u.ChandelierTypes.Any(c => dto.AdditionalParams.Contains(c))) &&
                (u.Materials == null || dto.Materials == null || u.Materials.Any(c => dto.Materials.Contains(c))) &&
                (u.PictureMaterial == null || dto.PictureMaterial == null || u.PictureMaterial.Any(c => dto.PictureMaterial.Contains(c))) &&
                (dto.IsSale == null || u.Discount > 0 == dto.IsSale || u.Discount != null == dto.IsSale) &&
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

        private Dictionary<int, GetFiltersCountResponseDto.FilterDto> GetFiltersByAdditionalParams(IEnumerable<Product> products)
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
            return GetFiltersBySecondaryEntities(products, p => (Guid)p.CollectionId!, _dbContext.Collections.AsNoTracking());
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

        private RangeFilterDto? GetRangeFilterByPrice(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria<decimal>(products, p => p.Price);
        }

        private RangeFilterDto? GetRangeFilterByLength(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.Length);
        }

        private RangeFilterDto? GetRangeFilterByDiameter(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.Diameter);
        }

        private RangeFilterDto? GetRangeFilterByHeight(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.Height);
        }

        private RangeFilterDto? GetRangeFilterByWidth(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.Width);
        }

        private RangeFilterDto? GetRangeFilterByIndent(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.Indent);
        }

        private RangeFilterDto? GetRangeFilterByLampCount(IEnumerable<Product> products)
        {
            return GetRangeFilterByCriteria(products, p => p.LampCount);
        }

        private RangeFilterDto? GetRangeFilterByCriteria<T>(IEnumerable<Product> products, Func<Product, T?> productSelector) where T : struct
        {
            var values = products.Select(productSelector).Where(v => v is not null).Cast<T>().ToArray();
            return values.Any()
                ? new RangeFilterDto { MinValue = Convert.ToDecimal(values.Min()), MaxValue = Convert.ToDecimal(values.Max()) }
                : null;
        }
    }
}
