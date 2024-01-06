using System.Net;
using System.Security.Claims;
using vogue_decor.Application.Common.Attributes;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain.Enums;
using vogue_decor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Ocsp;
using vogue_decor.Attributes;

namespace vogue_decor.Controllers
{
    /// <summary>
    /// Контроллер товаров
    /// </summary>
    [Route("api/product")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {
        private Guid UserId
        {
            get
            {
                var claimNameId = Request.HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
                if (claimNameId is null)
                    return Guid.Empty;
            
                return Guid.Parse(claimNameId.Value);
            }
        }

        private string UrlRaw => $"{Request.Scheme}://{Request.Host}";
        private readonly IProductsRepository _productsRepository;
        private readonly IWebHostEnvironment _environment;
        private const string WebRootPath = "/app/wwwroot";
        
        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        /// <param name="productsRepository">Репозиторий товаров</param>
        /// <param name="environment">Текущее окружение</param>
        public ProductController(IProductsRepository productsRepository,
            IWebHostEnvironment environment)
        {
            _productsRepository = productsRepository;
            _environment = environment;
        }

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="CreateProductResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CreateProductResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _productsRepository.Create(dto, WebRootPath, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Импорт товаров из XML файла
        /// </summary>
        /// <param name="dto">Входные данные для импорта</param>
        /// <returns><see cref="ImportProductsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Отсутствует файл</response>
        /// <response code="400">Недопустимое расширение файла</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("import")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(ImportProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> ImportProducts([FromForm] ImportProductsDto dto)
        {
            var result = await _productsRepository.Import(dto, WebRootPath, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Обновить информацию о товаре
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("details")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto dto)
        {
            await _productsRepository.Update(dto);

            return Ok();
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete()]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Delete([FromQuery] DeleteProductDto dto)
        {
            await _productsRepository.Delete(dto);

            return Ok();
        }
/*
        /// <summary>
        /// Получение всех товаров
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet()]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsDto dto)
        {
            var result = await _productsRepository.GetAll(dto, UrlRaw);

            return Ok(result);
        }*/

        /// <summary>
        /// Получение товара по идентификатору
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="ProductResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("details")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(ProductResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetById([FromQuery] GetProductByIdDto dto)
        {
            dto.UserId = UserId;
            var result = await _productsRepository.GetById(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Получение товара по критериям
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetByCriteria([FromQuery] GetProductByCriteriaDto dto)
        {
            dto.UserId = UserId;
            var result = await _productsRepository.GetByCriteria(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Получение товаров по идентификатору коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("collection")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetByCollectionId([FromQuery] GetProductByCollectionIdDto dto)
        {
            dto.UserId = UserId;
            var result = await _productsRepository.GetByCollectionId(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Получение товара по артикулу
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="ProductResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("article")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(ProductResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetByArticle([FromQuery] GetByArticleDto dto)
        {
            dto.UserId = UserId;
            var result = await _productsRepository.GetByArticle(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Поиск товаров по запросу или артикулу
        /// </summary>
        /// <remarks>
        /// Результат поиска по артикулу смотреть в "Получение товара по артикулу" 
        /// (предыдущий)
        /// </remarks>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="GetProductsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("search")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> SearchByQuery([FromQuery] SearchProductDto dto)
        {
            dto.UserId = UserId;
            if (Regex.IsMatch(dto.SearchQuery, "^(?=.*[a-zA-Z])(?=.*[0-9])(?=.*[-])[a-zA-Z0-9-]+$"))
            {
                return await GetByArticle(new GetByArticleDto 
                {
                    Article = dto.SearchQuery,
                    UserId = dto.UserId
                });
            }
            var result = await _productsRepository.Search(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Добавление товара в корзину
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("cart")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            dto.UserId = UserId;
            await _productsRepository.AddToCart(dto);

            return Ok();
        }

        /// <summary>
        /// Удаление товара из корзины
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("cart")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartDto dto)
        {
            dto.UserId = UserId;
            await _productsRepository.RemoveFromCart(dto);

            return Ok();
        }

        /// <summary>
        /// Добавление товара в избранные
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("favourite")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddToFavourite([FromBody] AddToFavouriteDto dto)
        {
            dto.UserId = UserId;
            await _productsRepository.AddToFavourite(dto);

            return Ok();
        }

        /// <summary>
        /// Удаление товара из избранных
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("favourite")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> RemoveFromFavourite([FromBody] RemoveFromFavouriteDto dto)
        {
            dto.UserId = UserId;
            await _productsRepository.RemoveFromFavourite(dto);

            return Ok();
        }

        /// <summary>
        /// Загрузить изображения товара или добавить ссылку на него
        /// </summary>
        /// <remarks>
        /// Поддерживаемые расширения: .jpg, .jpeg, .png, .mp4
        /// </remarks>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="UploadImageResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Отсутствует файл</response>
        /// <response code="400">Недопустимое расширение файла</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("image")]
        [RoleValidate(Roles.ADMIN)]
        [DisableRequestSizeLimit]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UploadImageResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDto dto)
        {
            var result = await _productsRepository.UploadImage(dto, WebRootPath, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Удалить изображение товара
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("image")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> RemoveImage([FromQuery] RemoveImageDto dto)
        {
            await _productsRepository.RemoveImage(dto, WebRootPath);

            return Ok();
        }

        /*/// <summary>
        /// Получить количество товаров по критериям
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("count")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetProductsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetCount([FromQuery] GetProductsCountDto dto)
        {
            var result = await _productsRepository.GetCount(dto);

            return Ok(result);
        }*/

        /// <summary>
        /// Изменить порядок файлов товара
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("details/files")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> SetFileOrder([FromBody] SetFileOrderDto dto)
        {
            await _productsRepository.SetFileOrder(dto);

            return Ok();
        }
    }
}
