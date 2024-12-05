using Carter;
using FakeApiFarsi.Application.Queries.Product;
using FakeApiFarsi.Domain.Product;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class ProductEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("api/v1/products").WithTags("Products");

        products.MapGet("/", GetProductsAsync).AllowAnonymous();
        products.MapPost("/", AddProductAsync).AllowAnonymous();
        products.MapPut("/{id}", UpdateProductAsync).AllowAnonymous();
        products.MapDelete("/{id}", DeleteProductAsync).AllowAnonymous();
    }

    // Create (POST)
    Task<IResult> AddProductAsync([FromBody] Product product)
    {
        OperationResult<bool> op = new("Create");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
    }

    // Update (PUT)
    Task<IResult> UpdateProductAsync(
        [FromRoute] int id,
        [FromBody] Product product)
    {
        OperationResult<bool> op = new("Update");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
    }

    // Delete (DELETE)
    Task<IResult> DeleteProductAsync(
        [FromRoute] int id)
    {
        OperationResult<bool> op = new("Delete");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
    }

    // Read (GET)
    async Task<IResult> GetProductsAsync(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new ProductQueryRequest.ProductQuery
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
