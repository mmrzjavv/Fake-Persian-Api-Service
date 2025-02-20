using Carter;
using FakeApiFarsi.Application.Queries.Address;
using FakeApiFarsi.Domain.Address;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class AddressEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var address = app.MapGroup("app/v1/addresses").WithTags("Addresses");

        address.MapGet("/", GetAddressesAsync).AllowAnonymous();
        address.MapPost("/", AddAddressAsync).AllowAnonymous();
        address.MapPut("/{id}", UpdateAddressAsync).AllowAnonymous();
        address.MapDelete("/{id}", DeleteAddressAsync).AllowAnonymous();
    }

    // Create (POST)
    Task<IResult> AddAddressAsync([FromBody] Address address)
    {
        OperationResult<bool> op = new("Create");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
    }

    // Update (PUT)
    Task<IResult> UpdateAddressAsync(
        [FromRoute] int id
    )
    {
        OperationResult<bool> op = new("Update");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
    }

    // Delete (DELETE)
    Task<IResult> DeleteAddressAsync(
        [FromRoute] int id)
    {
        OperationResult<bool> op = new("Delete");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
    }

    // Read (GET)
    async Task<IResult> GetAddressesAsync(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new AddressQueryRequest.AddressQuery()
        {
            Skip = skip ?? 0,
            Take = take ?? 10
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