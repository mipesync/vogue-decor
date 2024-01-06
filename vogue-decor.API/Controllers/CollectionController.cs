using vogue_decor.Application.Common.Attributes;
using vogue_decor.Application.DTOs.CollectionDTOs;
using vogue_decor.Application.DTOs.CollectionDTOs.ResponseDTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain.Enums;
using vogue_decor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using vogue_decor.Attributes;

namespace vogue_decor.Controllers
{
    /// <summary>
    /// Контроллер коллекций
    /// </summary>
    [Route("api/collection")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class CollectionController : Controller
    {
        private string UrlRaw => $"{Request.Scheme}://{Request.Host}";
        private readonly ICollectionRepository _collectionsRepository;
        private readonly IWebHostEnvironment _environment;
        private const string WebRootPath = "/app/wwwroot";

        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        /// <param name="collectionsRepository">Репозиторий товаров</param>
        /// <param name="environment">Текущее окружение</param>
        public CollectionController(ICollectionRepository collectionsRepository, IWebHostEnvironment environment)
        {
            _collectionsRepository = collectionsRepository;
            _environment = environment;
        }

        /// <summary>
        /// Получить список всех коллекций
        /// </summary>
        /// <returns><see cref="GetCollectionsResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet()]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetCollectionsResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetAll([FromQuery] GetColectionsDto dto)
        {
            var result = await _collectionsRepository.GetAll(dto, UrlRaw);

            return Ok(result);
        }

        /*/// <summary>
        /// Получить коллекцию по идентификатору
        /// </summary>
        /// <returns><see cref="CollectionLookupDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("details")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CollectionLookupDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetById([FromQuery] GetCollectionByIdDto dto)
        {
            var result = await _collectionsRepository.GetById(dto, UrlRaw);

            return Ok(result);
        }*/

        /// <summary>
        /// Получить коллекцию по названию
        /// </summary>
        /// <returns><see cref="CollectionLookupDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet("details/{name}")]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CollectionLookupDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var dto = new GetCollectionByNameDto { Name = name };
            var result = await _collectionsRepository.GetByName(dto, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Добавить новую коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="AddCollectionResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost()]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(AddCollectionResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Add([FromBody] AddCollectionDto dto)
        {
            var result = await _collectionsRepository.Add(dto);

            return Ok(result);
        }

        /// <summary>
        /// Обновить детали коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPut("details")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Update([FromBody] UpdateCollectionDto dto)
        {
            await _collectionsRepository.Update(dto);

            return Ok();
        }

        /// <summary>
        /// Удалить коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete()]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> Delete([FromBody] DeleteCollectionDto dto)
        {
            await _collectionsRepository.Delete(dto);

            return Ok();
        }

        /// <summary>
        /// Загрузить обложку коллекции
        /// </summary>
        /// <remarks>
        /// Поддерживаемые расширения: .jpg, .jpeg, .png
        /// </remarks>
        /// <param name="dto">Входные данные</param>
        /// <returns><see cref="UploadPreviewResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Отсутствует файл</response>
        /// <response code="400">Недопустимое расширение файла</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("preview")]
        [RoleValidate(Roles.ADMIN)]
        [DisableRequestSizeLimit]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UploadPreviewResponseDto))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> UploadPreview([FromForm] UploadPreviewDto dto)
        {
            var result = await _collectionsRepository.UploadPreview(dto, WebRootPath, UrlRaw);

            return Ok(result);
        }

        /// <summary>
        /// Удалить обложку коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("preview")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> RemovePreview([FromBody] RemovePreviewDto dto)
        {
            await _collectionsRepository.RemovePreview(dto, WebRootPath);

            return Ok();
        }

        /// <summary>
        /// Добавить товар в коллекцию
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("product")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddProduct([FromBody] AddProductToCollectionDto dto)
        {
            await _collectionsRepository.AddProduct(dto);

            return Ok();
        }

        /// <summary>
        /// Удалить товар из коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="404">Товар не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("product")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductFromCollectionDto dto)
        {
            await _collectionsRepository.DeleteProduct(dto);

            return Ok();
        }

        /// <summary>
        /// Установить индекс порядка очереди коллекции
        /// </summary>
        /// <param name="dto">Входные данные</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Коллекция не найдена</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("setIndex")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> SetIndex([FromBody] SetCollectionIndexDto dto)
        {
            await _collectionsRepository.SetIndex(dto);

            return Ok();
        }
    }
}
