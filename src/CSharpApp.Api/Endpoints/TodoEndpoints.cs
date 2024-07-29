using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CSharpApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void MapTodosEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/todos", async (ITodoService todoService) =>
        {
            var todos = await todoService.GetAllTodos();
            return todos;
        })
            .Produces<TodoRecord>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetTodos")
            .WithSummary("Get all todos.")
            .WithOpenApi();

        app.MapGet("/todos/{id}", async ([FromRoute] int id, ITodoService todoService) =>
        {
            var todos = await todoService.GetTodoById(id);
            
            if(todos == null)
            {
               return Results.NotFound(todos);
            }

            return Results.Ok(todos);
        })
            .Produces<TodoRecord>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetTodosById")
            .WithOpenApi(info =>
            {
                info.Summary = "Get todo by ID.";
                info.Parameters[0].Description = "Input valid ID greater than 0.";
                return info;
            });
    }
}