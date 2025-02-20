using Carter;
using FakeApiFarsi.Application.Queries.Order;
using FakeApiFarsi.Domain.Order;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class OrderEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var orders = app.MapGroup("api/v1/orders").WithTags("Orders");

        orders.MapGet("/", GetOrdersAsync).AllowAnonymous();
        orders.MapPost("/", AddOrderAsync).AllowAnonymous();
        orders.MapPut("/{id}", UpdateOrderAsync).AllowAnonymous();
        orders.MapDelete("/{id}", DeleteOrderAsync).AllowAnonymous();
    }

    // Create (POST)
    Task<IResult> AddOrderAsync([FromBody] Order order)
    {
        OperationResult<bool> op = new("Create");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
    }

    // Update (PUT)
    Task<IResult> UpdateOrderAsync(
        [FromRoute] int id,
        [FromBody] Order order)
    {
        OperationResult<bool> op = new("Update");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
    }

    // Delete (DELETE)
    Task<IResult> DeleteOrderAsync(
        [FromRoute] int id)
    {
        OperationResult<bool> op = new("Delete");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
    }

    // Read (GET)
    async Task<IResult> GetOrdersAsync(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new OrderQueryRequest.OrderQuery()
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
