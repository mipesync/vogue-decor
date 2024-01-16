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

        public async Task<CreateProductResponseDto> Create(CreateProductDto dto, string webRootPath, string hostUrl)
        {
            var brand = await GetBrandByIdAsync(dto.BrandId);
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
                await UploadImage(new UploadImageDto { Urls = urls.ToList(), ProductId = product.Id}, webRootPath, hostUrl);

            return new CreateProductResponseDto { ProductId = product.Id };
        }

        public async Task<ImportProductsResponseDto> Import(ImportProductsDto dto, string webRootPath, string hostUrl)
        {
            var products = await _fileParser.ParseAsync(dto.File);
            var ids = new List<Guid>();

            foreach (var product in products.ProductList)
            {
                var result = await Create(product, webRootPath, hostUrl);
                ids.Add(result.ProductId);
            }
            
            return new ImportProductsResponseDto
            {
                ProductIds = ids
            };
        }

        public async Task Update(UpdateProductDto dto)
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

        public async Task Delete(DeleteProductDto dto)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<GetProductsResponseDto> GetAll(GetAllProductsDto dto, string hostUrl)
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

        public async Task<ProductResponseDto> GetById(GetProductByIdDto dto, string hostUrl)
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

        public async Task<GetProductsResponseDto> GetByCriteria(GetProductByCriteriaDto dto, 
            string hostUrl)
        {
            Expression <Func<Product, bool>> filterExpression = u => 
                (dto.Types == null || dto.Types.Contains(u.ProductType)) &&
                (u.Price >= dto.MinPrice && u.Price <= dto.MaxPrice) &&
                (dto.Colors == null || u.Colors.Any(c => dto.Colors.Contains(c))) &&
                (u.Diameter >= dto.MinDiameter && u.Diameter <= dto.MaxDiameter) &&
                (u.LampCount >= dto.MinLampCount && u.LampCount <= dto.MaxLampCount) &&
                (dto.IsSale == null || u.Discount > 0 == dto.IsSale) &&
                (dto.CollectionId == null || u.CollectionId == dto.CollectionId);
            
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, expression: filterExpression);

            var counts = GetCounts(dto.UserId);

            var productsShort = ProductMapper.Map(_mapper, 
                products.Skip(dto.From).Take(dto.Count).ToList(), 
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
        
        public async Task<GetProductsResponseDto> GetByArticle(GetByArticleDto dto, string hostUrl)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .Where(u => u.Article.Contains(dto.Article)).ToListAsync();

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

        public async Task<GetProductsResponseDto> GetByCollectionId(GetProductByCollectionIdDto dto,
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
        
        public async Task<GetProductsResponseDto> Search(SearchProductDto dto, string hostUrl)
        {
            var searchQuery = dto.SearchQuery.Trim();
            var article = FindArticle(dto.SearchQuery);
            
            var strQueryWithoutArticle = article != string.Empty ? string.Join("", searchQuery.Replace($" {article}", "")).Trim() : searchQuery;
            
            var products = await GetOrderedProductsAsync(sortType: dto.SortType, searchQuery: strQueryWithoutArticle, article: article);
            
            if (products.Count == 0 && article == searchQuery.Trim())
            {
                if (article != string.Empty)
                {
                    var articleProducts = await GetByArticle(new GetByArticleDto
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

        public async Task AddToCart(AddToCartDto dto)
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

        public async Task RemoveFromCart(RemoveFromCartDto dto)
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

        public async Task AddToFavourite(AddToFavouriteDto dto)
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

        public async Task RemoveFromFavourite(RemoveFromFavouriteDto dto)
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

        public async Task<UploadImageResponseDto> UploadImage(UploadImageDto dto,
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

                    product.Urls.Add(imageName);

                    var imagePath = UrlParse(imageName, product.Id.ToString(), hostUrl);
                    result.Files.Add(imagePath);
                }
            }
            else if (dto.Files!.Count > 0)
            {
                foreach (var image in dto.Files!)
                {
                    _uploader.File = image;

                    var imageName = await _uploader.UploadFileAsync();

                    product.Urls.Add(imageName);

                    var imagePath = UrlParse(imageName, product.Id.ToString(), hostUrl);
                    result.Files.Add(imagePath);
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

        public async Task RemoveImage(RemoveImageDto dto, string webRootPath)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.Id == dto.ProductId, CancellationToken.None);

            if (product is null)
                throw new NotFoundException(product);

            if (!dto.FileName.Contains("http"))
            {
                File.Delete(Path.Combine(
                    webRootPath is null
                    ? throw new ArgumentException("Корневой путь проекта не может быть пустым")
                    : webRootPath, 
                    product.Id.ToString(), dto.FileName));
            }

            product.Urls.Remove(dto.FileName);

            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<ProductsCountResponseDto> GetCount(GetProductsCountDto dto)
        {
            Expression<Func<Product, bool>> defaultExpression = u => 
                (dto.Types == null || dto.Types.Contains(u.ProductType)) &&
                (u.Price >= dto.MinPrice && u.Price <= dto.MaxPrice) &&
                (dto.Colors == null || u.Colors.Any(c => dto.Colors.Contains(c))) &&
                (u.Diameter >= dto.MinDiameter && u.Diameter <= dto.MaxDiameter) &&
                (u.LampCount >= dto.MinLampCount && u.LampCount <= dto.MaxLampCount) &&
                (!dto.IsSale || u.Discount > 0) &&
                (dto.CollectionId == null || u.CollectionId == dto.CollectionId);

            Expression<Func<Product, bool>> expressionWithCollectionId = u =>
                u.CollectionId == dto.CollectionId;

            if (dto.CollectionId != Guid.Empty)
            {
                InvocationExpression invocationExpression = Expression.Invoke(expressionWithCollectionId,
                defaultExpression.Parameters.Cast<Expression>());

                defaultExpression = Expression.Lambda<Func<Product, bool>>(
                    Expression.AndAlso(defaultExpression.Body, invocationExpression),
                    defaultExpression.Parameters);
            }

            var productsCount = await _dbContext.Products
                .AsNoTracking()
                .Include(u => u.Favourites)
                .Include(u => u.ProductUsers)
                .Where(defaultExpression)
                .CountAsync(CancellationToken.None);

            return new ProductsCountResponseDto { Count = productsCount };
        }

        public async Task SetFileOrder(SetFileOrderDto dto)
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

        private async Task<int[]> GetColorsIdFromDbAsync(string query)
        {
            var colors = await _dbContext.Colors.AsNoTracking()
                .Where(u => u.SearchVector.Matches(query))
                .Select(u => u.Id).ToArrayAsync();

            return colors;
        }

        private async Task<int[]> GetProdTypesIdFromDbAsync(string query)
        {
            var types = await _dbContext.ProductTypes.AsNoTracking()
                .Where(u => u.SearchVector.Matches(query))
                .Select(u => u.Id).ToArrayAsync();

            return types;
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

        private async Task<Brand> GetBrandByIdAsync(Guid brandId)
        {
            var brand = await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == brandId);

            if (brand is null)
                throw new NotFoundException(brand);

            return brand;
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

        private string ParseFilterNameToString(IEnumerable<string> names)
        {
            return string.Join(",", names);
        }
    }
}
