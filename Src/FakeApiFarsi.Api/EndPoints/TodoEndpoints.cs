using Carter;
using FakeApiFarsi.Application.Queries.Todo;
using FakeApiFarsi.Domain.Todo;
using FakeApiFarsi.infrastructure.OperationRseult;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class TodoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var todos = app.MapGroup("api/v1/todos").WithTags("Todos");

        todos.MapGet("/", GetTodosAsync).AllowAnonymous();
        todos.MapPost("/", AddTodoAsync).AllowAnonymous();  
        todos.MapPut("/{id}", UpdateTodoAsync).AllowAnonymous();
        todos.MapDelete("/{id}", DeleteTodoAsync).AllowAnonymous();
    }

    // Create (POST)
    Task<IResult> AddTodoAsync([FromBody] Todo todo)
    {
        OperationResult<bool> op = new("Create");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت ثبت شد", true)));
    }

    // Update (PUT)
    Task<IResult> UpdateTodoAsync(
        [FromRoute] int id
    )
    {
        OperationResult<bool> op = new("Update");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت به‌روزرسانی شد", true)));
    }

    // Delete (DELETE)
    Task<IResult> DeleteTodoAsync(
        [FromRoute] int id
    )
    {
        OperationResult<bool> op = new("Delete");
        return Task.FromResult(Results.Ok(op.Succeed("اطلاعات با موفقیت حذف شد", true)));
    }

 
    async Task<IResult> GetTodosAsync(
        [FromQuery] int? skip,
        [FromQuery] int? take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new TodoQueryRequest.TodoQuery()
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