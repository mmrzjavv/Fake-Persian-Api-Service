using Carter;
using FakeApiFarsi.Api.ClaimHandler;
using FakeApiFarsi.Application.Commands.User;
using FakeApiFarsi.Application.Queries.user;
using FakeApiFarsi.Domain.User;
using FakeApiFarsi.infrastructure.Helpers.TokenHandler.Model;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class UserEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("api/v1/users").WithTags("Users");

        users.MapGet("/", GetUsersAsync).AllowAnonymous();
        users.MapPost("/", AddUserAsync).AllowAnonymous();
        users.MapPut("/{id}", UpdateUserAsync).AllowAnonymous();
        users.MapDelete("/{id}", DeleteUserAsync).AllowAnonymous();
        users.MapPost("/access-token", CreateAccessToken).AllowAnonymous();
        users.MapGet("/profile", GetUserProfile).RequireAuthorization();
    }

    // Create (POST)
    Task<IResult> AddUserAsync([FromBody] User user)
    {
        OperationResult<bool> op = new("Create");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
    }

    // Update (PUT)
    Task<IResult> UpdateUserAsync(
        [FromRoute] int id
        )
    {
        OperationResult<bool> op = new("Update");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
    }

    // Delete (DELETE)
    Task<IResult> DeleteUserAsync(
        [FromRoute] int id)
    {
        OperationResult<bool> op = new("Delete");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
    }

    // Read (GET)
    async Task<IResult> GetUsersAsync(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new UserQueryRequest.UserQuery()
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

    async Task<IResult> CreateAccessToken(
        [FromBody] AccessTokenCommandRequest.AccessTokenCommand command,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var result = await mediator.Send(command);
        if (!result.Success)
        {
            return Results.BadRequest(result);  
        }
        return Results.Ok(result);
    }

    async Task<IResult> GetUserProfile(
        HttpContext context,
        HttpResponse response
    )
    {
        OperationResult<UserDataClaim.UserTokenData> op = new("GetUserProfile");
        var claim = context.User.Claims.GetClaimData();
        if (claim is null)
        {
            return Results.NotFound(op.Fail("اطلاعات کاربر یافت نشد"));
        }
        return  Results.Ok(op.Succeed("اطلاعات کاربر با موفقیت یافت شد" , claim));
    }
}