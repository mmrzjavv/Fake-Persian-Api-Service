using Carter;
using FakeApiFarsi.Application.Queries.Internet;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints
{
    public class InternetEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var internets = app.MapGroup("api/v1/internets").WithTags("Internets");
            
            internets.MapGet("/", GetInternetsAsync);
        }

        async Task<IResult> GetInternetsAsync(
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IMediator mediator,
            HttpResponse response)
        {
            // درخواست برای گرفتن داده‌ها
            var request = new InternetQueryRequest.InternetQueryCommand
            {
                Skip = skip,
                Take = take
            };

            var result = await mediator.Send(request);

            if (!result.Success)
            {
                return Results.BadRequest(result);  
            }

            response.Headers.Append("X-TotalRecordCount", result.List.Count.ToString());

            return Results.Ok(result);
        }
    }
}