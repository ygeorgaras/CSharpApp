using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace CSharpApp.Api.Endpoints;

public static class PostsEndpoints
{
    public static void MapPostsEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/posts/{userId}:{title}:{body}", async (
            [FromRoute] int userId,
            [FromRoute] string title,
            [FromRoute] string body,
            IPostService postService) =>
        {
            var posts = await postService.AddPostRecord(userId, title, body);
            if(posts == null)
            {
                return Results.UnprocessableEntity($"Invalid userId: {userId}");
            }
            return Results.Ok(posts);
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status422UnprocessableEntity)
        .WithName("AddPostRecord")
        .WithOpenApi(info =>
        {
            info.Summary = "Add new post.";
            info.Parameters[0].Description = "Input valid userID greater than 0.";
            return info;
        });

        app.MapPut("/posts/{id}:{userId}:{title}:{body}", async(
            [FromRoute] int id,
            [FromRoute] int userId,
            [FromRoute] string title,
            [FromRoute] string body,
            IPostService postService) =>
                {
                    var posts = await postService.PutById(userId, title, body, id);
                    return posts;
                })
        .WithName("PutById")
        .WithOpenApi(info =>
        {
            info.Summary = "Edit post by ID.";
            info.Parameters[0].Description = "Input valid ID greater than 0.";
            info.Parameters[1].Description = "Input valid userID greater than 0.";
            return info;
        });

        app.MapDelete("/posts/{id}", async (
        [FromRoute] int id,
        IPostService postService) =>
        {
            var posts = await postService.DeletePostById(id);
            if(posts == HttpStatusCode.NotFound)
            {
                return Results.NotFound($"ID {id} does not exist.");
            }
            return Results.Ok($"Delete Successful: Record with ID:{id} deleted.");
        })
        .WithName("DeletePostById")
        .WithOpenApi(info =>
        {
            info.Summary = "Delete post by ID.";
            info.Parameters[0].Description = "Input valid ID greater than 0.";
            return info;
        });

        app.MapGet("/posts/", async (
        IPostService postService) =>
        {
            var posts = await postService.GetAllPosts();
            return posts;
        })
        .Produces<PostRecord>(StatusCodes.Status200OK)
        .WithName("GetPosts")
        .WithSummary("Get all posts.")
        .WithOpenApi();

        app.MapGet("/posts/{id}", async (
            [FromRoute] int id,
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
            info.Parameters[0].Description = "Input valid ID greater than 0.";
            return info;
        });
    }
}