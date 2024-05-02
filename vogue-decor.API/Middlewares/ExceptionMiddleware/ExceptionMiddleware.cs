using FluentValidation;
using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Models;
using Newtonsoft.Json;
using System.Net;
using System.Xml;

namespace vogue_decor.Middlewares.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            int statusCode = 0;
            var message = string.Empty;

            switch (exception)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                case ValidationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case XmlException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Некорректный файл";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(new ErrorModel
            {
                StatusCode = statusCode,
                Message = message == string.Empty ? exception.Message : message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            Console.WriteLine($"Message: {exception.Message}\n InnerMessage: {exception.InnerException}");
            
            return context.Response.WriteAsync(result);
        }
    }
}
