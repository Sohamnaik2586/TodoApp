using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using TodoApi.Exceptions;
using TodoApi.Middleware;

namespace TodoApi.Tests;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionIsThrown_ReturnsNotFoundJson()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ExceptionMiddleware(_ => throw new NotFoundException("Todo not found"));

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBodyAsync(context);
        using var json = JsonDocument.Parse(body);

        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        Assert.Equal("Todo not found", json.RootElement.GetProperty("error").GetString());
    }

    [Fact]
    public async Task InvokeAsync_WhenUnexpectedExceptionIsThrown_ReturnsInternalServerErrorJson()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        var middleware = new ExceptionMiddleware(_ => throw new InvalidOperationException("Unexpected failure"));

        await middleware.InvokeAsync(context);

        var body = await ReadResponseBodyAsync(context);
        using var json = JsonDocument.Parse(body);

        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
        Assert.Equal("Unexpected failure", json.RootElement.GetProperty("error").GetString());
    }

    private static async Task<string> ReadResponseBodyAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(context.Response.Body);

        return await reader.ReadToEndAsync();
    }
}
