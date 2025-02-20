using Carter;
using FakeApiFarsi.Application.Queries.Internet;
using FakeApiFarsi.Domain.Internet;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints
{
    public class InternetEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var internets = app.MapGroup("api/v1/internets").WithTags("Internets");

            internets.MapGet("/", GetInternetsAsync).AllowAnonymous();
            internets.MapPost("/", AddInternetAsync).AllowAnonymous();
            internets.MapPut("/{id}", UpdateInternetAsync).AllowAnonymous();
            internets.MapDelete("/{id}", DeleteInternetAsync).AllowAnonymous();
        }

        // Create (POST)
        Task<IResult> AddInternetAsync([FromBody] Internet internet)
        {
            OperationResult<bool> op = new("Create");
            return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
        }

        // Update (PUT)
        Task<IResult> UpdateInternetAsync(
            [FromRoute] int id,
            [FromBody] Internet internet)
        {
            OperationResult<bool> op = new("Update");
            return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
        }

        // Delete (DELETE)
        Task<IResult> DeleteInternetAsync(
            [FromRoute] int id)
        {
            OperationResult<bool> op = new("Delete");
            return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
        }

        // Read (GET)
        async Task<IResult> GetInternetsAsync(
            [FromQuery] int? skip,
            [FromQuery] int? take,
            [FromServices] IMediator mediator,
            HttpResponse response)
        {
            var request = new InternetQueryRequest.InternetQuery
            {
                Skip = skip??0,
                Take = take??10
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
