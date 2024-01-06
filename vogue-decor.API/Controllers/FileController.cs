using vogue_decor.Application.Common.Attributes;
using vogue_decor.Application.DTOs.FilesDTOs;
using vogue_decor.Application.DTOs.FilesDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain.Enums;
using vogue_decor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using vogue_decor.Attributes;

namespace vogue_decor.Controllers;

/// <summary>
/// Контроллер файлов
/// </summary>
[Route("api/file")]
[Produces("application/json")]
[ApiController]
[Authorize]
public class FileController : Controller
{
    private string UrlRaw => $"{Request.Scheme}://{Request.Host}";
    private readonly IFilesRepository _filesRepository;
    private readonly IWebHostEnvironment _environment;
    private const string WebRootPath = "/app/wwwroot";

    public FileController(IFilesRepository filesRepository, IWebHostEnvironment environment)
    {
        _filesRepository = filesRepository;
        _environment = environment;
    }

    /// <summary>
    /// Загрузить новые файлы
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <returns><see cref="UploadFileResponseDto"/></returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(UploadFileResponseDto))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> Upload([FromForm] UploadFileDto dto)
    {
        var result = await _filesRepository.Upload(dto, WebRootPath, UrlRaw);

        return Ok(result);
    }

    /// <summary>
    /// Удалить файл
    /// </summary>
    /// <param name="fileName">Название файла</param>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Файл не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{fileName}")]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> Delete(string fileName)
    {
        await _filesRepository.Delete(fileName, WebRootPath);

        return Ok();
    }
    
    /// <summary>
    /// Получение списка файлов
    /// </summary>
    /// <returns><see cref="GetAllFilesResponseDto"/></returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetAllFilesResponseDto))]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllFilesDto dto)
    {
        var result = await _filesRepository.GetAll(dto, UrlRaw);

        return Ok(result);
    }
}