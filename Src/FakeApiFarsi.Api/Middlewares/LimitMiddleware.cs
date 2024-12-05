using FakeApiFarsi.infrastructure.OperationRseult;

namespace FakeApiFarsi.Api.Middlewares;

public class LimitMiddleware
{
    private readonly RequestDelegate _next;

    public LimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        OperationResult<bool> op = new("Limit");

        if (context.Request.Query.TryGetValue("skip", out var skipValues))
        {
            if (int.TryParse(skipValues.FirstOrDefault(), out var skip) && skip > 100)
            {
                var response = op.Fail("پارامتر 'skip' باید کمتر یا مساوی ۱۰۰ باشد.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }
        }

        if (context.Request.Query.TryGetValue("take", out var takeValues))
        {
            if (int.TryParse(takeValues.FirstOrDefault(), out var take) && take > 100)
            {
                var response = op.Fail("پارامتر 'take' باید کمتر یا مساوی ۱۰۰ باشد.");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
                return;
            }
        }

        await _next(context);
    }
}