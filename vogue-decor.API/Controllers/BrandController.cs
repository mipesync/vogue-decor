using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using vogue_decor.Application.DTOs.BrandDTOs;
using vogue_decor.Application.DTOs.BrandDTOs.ResponseDTOs;
using vogue_decor.Application.DTOs.ProductDTOs.Response_DTOs;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Attributes;
using vogue_decor.Domain.Enums;
using vogue_decor.Models;

namespace vogue_decor.Controllers;

/// <summary>
/// Контроллер брендов
/// </summary>
[Route("api/brand")]
[Produces("application/json")]
[ApiController]
[Authorize]
public class BrandController : Controller
{
    private Guid UserId
    {
        get
        {
            var claimNameId = Request.HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier);
            return claimNameId is null ? Guid.Empty : Guid.Parse(claimNameId.Value);
        }
    }

    private string UrlRaw => $"{Request.Scheme}://{Request.Host}";
    private readonly IBrandRepository _brandRepository;
    private readonly IWebHostEnvironment _environment;
    //private const string WebRootPath = "/app/wwwroot";
    private string WebRootPath => _environment.WebRootPath;

    public BrandController(IBrandRepository productsRepository, IWebHostEnvironment environment)
    {
        _brandRepository = productsRepository;
        _environment = environment;
    }
    
    /// <summary>
    /// Создать новый бренд
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <returns><see cref="CreateBrandResponseDto"/></returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CreateBrandResponseDto))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> CreateAsync([FromForm] CreateBrandDto dto)
    {
        var result = await _brandRepository.CreateAsync(dto, WebRootPath, UrlRaw);

        return Ok(result);
    }

    /// <summary>
    /// Обновить информацию о бренде
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Бренд не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> UpdateAsync([FromForm] UpdateBrandDto dto)
    {
        await _brandRepository.UpdateAsync(dto, WebRootPath, UrlRaw);

        return Ok();
    }

    /// <summary>
    /// Удалить бренд
    /// </summary>
    /// <param name="brandId">Идентификатор бренда</param>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Бренд не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{brandId:guid}")]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid brandId)
    {
        await _brandRepository.DeleteAsync(brandId, WebRootPath);

        return Ok();
    }

    /// <summary>
    /// Удалить обложку бренда
    /// </summary>
    /// <param name="dto">Входные данные</param>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Бренд не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("/preview")]
    [RoleValidate(Roles.ADMIN)]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> DeleteImageAsync([FromBody] DeleteBrandImageDto dto)
    {
        await _brandRepository.DeleteImageAsync(dto, WebRootPath);

        return Ok();
    }

    /// <summary>
    /// Получение всех брендов
    /// </summary>
    /// <param name="skip">Кол-во брендов, которое надо пропустить</param>
    /// <param name="take">Кол-во брендов, которое надо получить</param>
    /// <returns><see cref="GetBrandsResponseDto"/></returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Бренд не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet]
    [AllowAnonymous]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(GetBrandsResponseDto))]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> GetAllAsync([FromQuery] int skip, [FromQuery] int take)
    {
        var result = await _brandRepository.GetAllAsync(UrlRaw, skip, take);

        return Ok(result);
    }

    /// <summary>
    /// Получение бренда по идентификатору
    /// </summary>
    /// <param name="brandId">Идентификатор бренда</param>
    /// <returns><see cref="BrandResponseDto"/></returns>
    /// <response code="200">Запрос выполнен успешно</response>
    /// <response code="404">Бренд не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{brandId:guid}")]
    [AllowAnonymous]
    [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(BrandResponseDto))]
    [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
    [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid brandId)
    {
        var result = await _brandRepository.GetByIdAsync(brandId, UrlRaw);

        return Ok(result);
    }
}