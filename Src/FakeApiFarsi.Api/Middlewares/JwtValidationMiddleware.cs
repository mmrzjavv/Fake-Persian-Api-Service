using System.Net;
using FakeApiFarsi.infrastructure.OperationRseult;

namespace FakeApiFarsi.Api.Middlewares;

public class JwtValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/v1") && !context.User.Identity.IsAuthenticated)
        {
            var op = new OperationResult<bool>("JWTValidation");
            var response = op.Fail("توکن به درستی قرار داده نشده است.",
                "Please put the token in the header. The token is not properly initialized or is not valid.", false,
                null, HttpStatusCode.Unauthorized);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(response);
            return;
        }

        await next(context);
    }
}