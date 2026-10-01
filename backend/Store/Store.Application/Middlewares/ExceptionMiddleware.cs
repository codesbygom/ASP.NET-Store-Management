using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Store.Application.Exceptions;
using Store.Application.Responses;

namespace Store.Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            Response<string> res;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    res = new Response<string>(ResponseStatus.BadRequest);
                    res.Errors.Add(badRequestException.Message);
                    break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    res = new Response<string>(ResponseStatus.BadRequest);
                    res.Errors = validationException.Errors;
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    res = new Response<string>(ResponseStatus.NotFound);
                    res.Errors.Add(notFoundException.Message);
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    res = new Response<string>(ResponseStatus.Unauthorized);
                    res.Errors.Add(unauthorizedAccessException.Message);
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    res = new Response<string>(ResponseStatus.Failed);
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(res, SerializerOptions));
        }
    }
}
