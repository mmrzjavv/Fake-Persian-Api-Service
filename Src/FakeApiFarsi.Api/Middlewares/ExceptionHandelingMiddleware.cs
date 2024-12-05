using System.Net;
using FakeApiFarsi.infrastructure.OperationRseult;
using Newtonsoft.Json;

namespace FakeApiFarsi.Api.Middlewares;

public class ExceptionHandelingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); 
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response =
            new OperationResult<object>("An error occurred while processing your request.").Fail("خطای شناسایی نشده", exception.Message);

        using var writer = new StringWriter();
        using var jsonWriter = new JsonTextWriter(writer);

        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, response);

        await context.Response.WriteAsync(writer.ToString());
    }
}