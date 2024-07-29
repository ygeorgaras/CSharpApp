using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CSharpApp.Api.Endpoints;

public static class TodosEndpoints
{
    public static void MapTodosEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/todos/{userId}:{title}:{completed}", async (
            [FromRoute][Range(1, int.MaxValue)] int userId,
            [FromRoute] string? title,
            [FromRoute] bool completed,
            ITodoService todoService) =>
                {
                    var newRecord = new TodoRecord(userId, 0, title!, completed);
                    var todos = await todoService.AddTodoRecord(newRecord);

                    return Results.Ok(todos);
                })
        .Produces<TodoRecord>(StatusCodes.Status200OK)
        .WithName("AddTodoRecord")
        .WithOpenApi(info =>
        {
            info.Summary = "Add new todo.";
            info.Parameters[0].Description = "Input valid userID between 1 and the max value of an Int32.";
            return info;
        });

        app.MapPut("/todos/{id}:{userId}:{title}:{completed}", async (
            [FromRoute][Range(1, int.MaxValue)] int id,
            [FromRoute][Range(1, int.MaxValue)] int userId,
            [FromRoute] string? title,
            [FromRoute] bool completed,
            ITodoService todoService) =>
        {
            TodoRecord newRecord = new TodoRecord(userId, id, title!, completed);
            var todo = await todoService.PutTodoById(newRecord);

            if (todo == null)
            {
                return Results.NotFound(todo);
            }

            return Results.Ok(todo);
        })
        .Produces<TodoRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("PutTodoById")
        .WithOpenApi(info =>
        {
            info.Summary = "Edit todo by ID.";
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            info.Parameters[1].Description = "Input valid userID between 1 and the max value of an Int32.";
            return info;
        });

        app.MapDelete("/todos/{id}", async (
        [FromRoute][Range(1, int.MaxValue)] int id,
        ITodoService todoService) =>
        {
            var todos = await todoService.DeleteTodoById(id);
            if (todos == HttpStatusCode.NotFound)
            {
                return Results.NotFound($"ID {id} does not exist.");
            }
            return Results.Ok($"Delete Successful: Record with ID:{id} deleted.");
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeleteTodoById")
        .WithOpenApi(info =>
        {
            info.Summary = "Delete todo by ID.";
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            return info;
        });

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

        app.MapGet("/todos/{id}", async (
            [FromRoute][Range(1, int.MaxValue)] int id,
            ITodoService todoService) =>
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
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            return info;
        });
    }
}