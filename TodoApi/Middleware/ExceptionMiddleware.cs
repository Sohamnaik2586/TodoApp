using System.Net;
using System.Text.Json;
using TodoApi.Exceptions;

namespace TodoApi.Middleware;

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
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            await HandleExceptionAsync(context, ex.Message);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await HandleExceptionAsync(context, ex.Message);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string message)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}