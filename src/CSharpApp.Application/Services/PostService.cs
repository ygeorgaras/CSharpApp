using CSharpApp.Core.Dtos;
using System.Net;
using System.Text.Json;

namespace CSharpApp.Application.Services;
public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly HttpClientWrapper _httpClientWrapper;

    private const string POSTS_ENDPOINT = "/posts";

    public PostService(ILogger<PostService> logger,
    HttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClientWrapper = httpClientWrapper;
    }

    private static bool ValidatePost(PostRecord postRecord)
    {
        return !string.IsNullOrWhiteSpace(postRecord.Title)
            && !string.IsNullOrWhiteSpace(postRecord.Body)
            && postRecord.UserId >= 1;
    }

    public async Task<PostRecord?> GetPostById(int id)
    {
        try
        {
            return await _httpClientWrapper.GetAsync<PostRecord>($"{POSTS_ENDPOINT}/{id}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"Failed to get Todo from posts/{id}.");
            return null;
        }
    }

    public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
    {
        try
        {
            return await _httpClientWrapper.GetAllAsync<PostRecord>($"{POSTS_ENDPOINT}/");
        }
        catch (InvalidOperationException ex)
        {
            //_logger.LogError(ex, "Failed to get Todo by ID.");
            return default;
        }
    }

    /// <summary>
    /// Checks if the record is valid and then trys posting the record from the HttpClientWrapper.
    /// </summary>
    /// <param name="userId">New userId we want to assign.</param>
    /// <param name="title">New title to use.</param>
    /// <param name="body">New body to use.</param>
    /// <returns>Returns the new record along with the new ID.</returns>
    public async Task<PostRecord?> AddPostRecord(int userId, string title, string body)
    {
        var record = new PostRecord(userId, 0, title, body);

        //Checks if input is valid.
        if (!ValidatePost(record))
        {
            return default;
        }

        try
        {
            return await _httpClientWrapper.PostAsync<PostRecord, PostRecord>($"{POSTS_ENDPOINT}", record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create new Post.");
            return null;
        }
    }

    /// <summary>
    /// This will search using the input ID if the record exists. If it exists then it will issue a Delete to the API.
    /// </summary>
    /// <param name="id">ID we wish to delete.</param>
    /// <returns>The HTTP Status Code we receive from the API.</returns>
    public async Task<HttpStatusCode> DeletePostById(int id)
    {
        return await _httpClientWrapper.DeleteByIdAsync($"{POSTS_ENDPOINT}", id);
    }

    public async Task<PostRecord> PutById(int userId, string title, string body, int id)
    {
        PostRecord record = new PostRecord(userId, id, title, body);
        //Checks if input is valid.
        if (!ValidatePost(record))
        {
            //TODO: Handle invalid post
            return default;
        }
        return await _httpClientWrapper.PutAsync<PostRecord, PostRecord>(POSTS_ENDPOINT, record, id);
    }
}