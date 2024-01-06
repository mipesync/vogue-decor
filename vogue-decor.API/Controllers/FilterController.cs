﻿using System.Security.Claims;
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
using vogue_decor.Application.DTOs.FilterDTOs;
using Org.BouncyCastle.Ocsp;
using vogue_decor.Attributes;

namespace vogue_decor.Controllers
{
    /// <summary>
    /// Контроллер фильтров
    /// </summary>
    [Route("api/filter")]
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    public class FilterController : Controller
    {
        private readonly IFiltersRepository _filtersRepository;
        
        /// <summary>
        /// Инициализация начальных параметров
        /// </summary>
        /// <param name="filtersRepository">Репозиторий фильтров</param>
        public FilterController(IFiltersRepository filtersRepository)
        {
            _filtersRepository = filtersRepository;
        }

        /// <summary>
        /// Добавление цвета
        /// </summary>
        /// <param name="name">Название цвета на русском</param>
        /// <param name="engName">Название цвета на английском</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("color")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddColor([FromQuery] string name, [FromQuery] string engName)
        {
            await _filtersRepository.AddColor(name, engName);

            return Ok();
        }
        
        /// <summary>
        /// Удаление цвета
        /// </summary>
        /// <param name="colorId">Идентификатор цвета</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Цвет не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("color")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> DeleteColor([FromQuery] int colorId)
        {
            await _filtersRepository.DeleteColor(colorId);

            return Ok();
        }
        
        /// <summary>
        /// Добавление типа люстры
        /// </summary>
        /// <param name="name">Название типа люстры</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("chandelierType")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddChanType([FromQuery] string name)
        {
            await _filtersRepository.AddChanType(name);

            return Ok();
        }
        
        /// <summary>
        /// Удаление типа люстры
        /// </summary>
        /// <param name="chandelierTypeId">Идентификатор типа люстры</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Тип люстры не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("chandelierType")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> DeleteChanType([FromQuery] int chandelierTypeId)
        {
            await _filtersRepository.DeleteChanType(chandelierTypeId);

            return Ok();
        }
        
        /// <summary>
        /// Добавление типа товара
        /// </summary>
        /// <param name="name">Название типа товара</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpPost("productType")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> AddProductType([FromQuery] string name)
        {
            await _filtersRepository.AddProductType(name);

            return Ok();
        }
        
        /// <summary>
        /// Удаление типа товара
        /// </summary>
        /// <param name="productTypeId">Идентификатор типа товара</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="404">Тип товара не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpDelete("productType")]
        [RoleValidate(Roles.ADMIN)]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, type: typeof(ErrorModel))]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> DeleteProductType([FromQuery] int productTypeId)
        {
            await _filtersRepository.DeleteProductType(productTypeId);

            return Ok();
        }

        /// <summary>
        /// Получения списка фильтров
        /// </summary>
        /// <returns><see cref="FiltersResponseDto"/></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: null)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError, type: typeof(ErrorModel))]
        public async Task<IActionResult> GetFilters()
        {
            var result = await _filtersRepository.GetFilters();

            return Ok(result);
        }
    }
}
