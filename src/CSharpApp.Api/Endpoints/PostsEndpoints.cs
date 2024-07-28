using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using Microsoft.Extensions.Hosting;

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
            return posts;
        })
        .WithName("AddPostRecord")
        .WithOpenApi();

        app.MapDelete("/posts/{id}", async (
        [FromRoute] int id,
        IPostService postService) =>
        {
            var posts = await postService.DeletePostById(id);
            return posts;
        })
        .WithName("DeletePostById")
        .WithOpenApi();

        app.MapGet("/posts", async (
        IPostService postService) =>
        {
            var posts = await postService.GetAllPosts();
            return posts;
        })
        .WithName("GetPosts")
        .WithOpenApi();

        app.MapGet("/posts/{id}", async (
            [FromRoute] int id,
            IPostService postService) =>
        {
            var posts = await postService.GetPostById(id);
            return posts;
        })
        .WithName("GetPostsById")
        .WithOpenApi();
    }
}