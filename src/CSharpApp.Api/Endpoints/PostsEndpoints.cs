using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace CSharpApp.Api.Endpoints;

public static class PostsEndpoints
{
    public static void MapPostsEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/posts/{userId}:{title}:{body}", async (
            [FromRoute][Range (1, int.MaxValue)] int userId,
            [FromRoute] string? title,
            [FromRoute] string? body,
            IPostService postService) =>
        {
            var newRecord = new PostRecord(userId, 0, title!, body!);
            var posts = await postService.AddPostRecord(newRecord);
            
            return Results.Ok(posts);
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .WithName("AddPostRecord")
        .WithOpenApi(info =>
        {
            info.Summary = "Add new post.";
            info.Parameters[0].Description = "Input valid userID between 1 and the max value of an Int32.";
            
            return info;
        });

        app.MapPut("/posts/{id}:{userId}:{title}:{body}", async(
            [FromRoute][Range(1, int.MaxValue)] int id,
            [FromRoute][Range(1, int.MaxValue)] int userId,
            [FromRoute] string? title,
            [FromRoute] string? body,
            IPostService postService) =>
        {
            PostRecord newRecord = new PostRecord(userId, id, title!, body!);
            var post = await postService.PutPostById(newRecord);

            if(post == null)
            {
                return Results.NotFound(post);
            }
            return Results.Ok(post);
        })
        .Produces<TodoRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("PutById")
        .WithOpenApi(info =>
        {
            info.Summary = "Edit post by ID.";
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            info.Parameters[1].Description = "Input valid userID between 1 and the max value of an Int32.";
            return info;
        });

        app.MapDelete("/posts/{id}", async (
        [FromRoute][Range(1, int.MaxValue)] int id,
        IPostService postService) =>
        {
            var posts = await postService.DeletePostById(id);
            if(posts == HttpStatusCode.NotFound)
            {
                return Results.NotFound($"ID {id} does not exist.");
            }
            return Results.Ok($"Delete Successful: Record with ID:{id} deleted.");
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("DeletePostById")
        .WithOpenApi(info =>
        {
            info.Summary = "Delete post by ID.";
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            return info;
        });

        app.MapGet("/posts/", async (
        IPostService postService) =>
        {
            var posts = await postService.GetAllPosts();
            return posts;
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetPosts")
        .WithSummary("Get all posts.")
        .WithOpenApi();

        app.MapGet("/posts/{id}", async (
            [FromRoute][Range(1, int.MaxValue)] int id,
            IPostService postService) =>
        {
            var posts = await postService.GetPostById(id);

            if(posts == null)
            {
                return Results.NotFound(posts);
            }
            return Results.Ok(posts);
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetPostsById")
        .WithOpenApi(info =>
        {
            info.Summary = "Get post by ID.";
            info.Parameters[0].Description = "Input valid ID between 1 and the max value of an Int32.";
            return info;
        });
    }
}