using Carter;
using FakeApiFarsi.Application.Queries.Todo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FakeApiFarsi.Api.EndPoints;

public class TodoEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var todos = app.MapGroup("api/v1/todos").WithTags("Todos");

        // todos.MapPost("/", AddTodoAsync);
        // todos.MapPut("/{id}", UpdateTodoAsync);
        // todos.MapDelete("/{id}", DeleteTodoAsync);
        // todos.MapGet("/{id}", GetTodoByIdAsync);
        todos.MapGet("/", GetTodosAsync);
    }

    // Create (POST)
    // async Task<IResult> AddTodoAsync(
    //     [FromBody] TodoDto todoDto,
    //     [FromServices] IToDoManager todoManager
    // )
    // {
    //     var result = await todoManager.AddTodoAsync(todoDto);
    //     return result.IsSuccess ? Results.Created($"/todos/{result.Value.Id}", result.Value) 
    //                             : Results.BadRequest(result.Error);
    // }

    // Update (PUT)
    // async Task<IResult> UpdateTodoAsync(
    //     [FromRoute] int id,
    //     [FromBody] TodoDto todoDto,
    //     [FromServices] IToDoManager todoManager
    // )
    // {
    //     var result = await todoManager.UpdateTodoAsync(id, todoDto);
    //     return result.IsSuccess ? Results.Ok(result.Value) 
    //                             : Results.BadRequest(result.Error);
    // }

    // Delete
    // async Task<IResult> DeleteTodoAsync(
    //     [FromRoute] int id,
    //     [FromServices] IToDoManager todoManager
    // )
    // {
    //     var result = await todoManager.DeleteTodoAsync(id);
    //     return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
    // }

    // Get By ID
    // async Task<IResult> GetTodoByIdAsync(
    //     [FromRoute] int id,
    //     [FromServices] IToDoManager todoManager
    // )
    // {
    //     var result = await todoManager.GetTodoByIdAsync(id);
    //     return result.IsSuccess ? Results.Ok(result.Value) 
    //                             : Results.NotFound(result.Error);
    // }

    // Filter (Pagination)
    async Task<IResult> GetTodosAsync(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromServices] IMediator mediator,
        HttpResponse response)
    {
        var request = new TodoQueryHandler.TodoCommandRequest
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