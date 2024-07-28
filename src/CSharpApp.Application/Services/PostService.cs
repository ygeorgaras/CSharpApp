using CSharpApp.Core.Dtos;
using System.Net;
using System.Text.Json;

namespace CSharpApp.Application.Services;
public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly HttpClient _client;
    private readonly string? _baseUrl;

    public PostService(ILogger<PostService> logger,
    IConfiguration configuration)
    {
        _logger = logger;
        _client = new HttpClient();
        _baseUrl = configuration["BaseUrl"];
        _client.BaseAddress = new Uri(_baseUrl!);
    }

    private static bool ValidatePost(PostRecord postRecord)
    {
        return !string.IsNullOrWhiteSpace(postRecord.Title)
            && !string.IsNullOrWhiteSpace(postRecord.Body)
            && postRecord.UserId >= 1;
    }

    /// <summary>
    /// This method will try to retrieve a Post record if it exists.
    /// </summary>
    /// <param name="id">ID we are searching for.</param>
    /// <returns>Existing TodoRecord or null if record doesn't exist.</returns>
    public async Task<PostRecord?> GetPostById(int id)
    {
        var response = await _client.GetAsync($"posts/{id}");
        if (response.IsSuccessStatusCode)
        {
            var postRecord = await response.Content.ReadFromJsonAsync<PostRecord>();
            return postRecord;
        }
        //TODO: Handle invalid ID.
        return null;
    }

    /// <summary>
    /// This method will get a list of all Post records existing.
    /// </summary>
    /// <returns>All Post records. Or a null list if it cannot find any records.</returns>
    public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
    {
        var response = await _client.GetAsync($"posts");
        if (response.IsSuccessStatusCode)
        {
            var postRecord = await response.Content.ReadFromJsonAsync<List<PostRecord>>();
            return new ReadOnlyCollection<PostRecord>(postRecord!);
        }

        //TODO: Handle any other results.
        return new ReadOnlyCollection<PostRecord>(new List<PostRecord>());
    }

    /// <summary>
    /// Adds a new Post Record.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <returns>Returns the new record along with the new ID.</returns>
    public async Task<PostRecord?> AddPostRecord(int userId, string title, string body)
    {
        var record = new PostRecord(userId, 0, title, body);

        //Checks if input is valid.
        if (!ValidatePost(record))
        {
            //TODO: Handle invalid post
            return null;
        }

        var response = await _client.PostAsJsonAsync<PostRecord>($"posts", record);

        if (response.IsSuccessStatusCode)
        {
            var postRecord = await response.Content.ReadFromJsonAsync<PostRecord>();
            return postRecord;
        }
        else
        {
            //TODO: Handle error response appropriately.
            return null;
        }
    }

    /// <summary>
    /// This will search using the input ID if the record exists. If it exists then it will issue a Delete to the API.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The Status Code we receive from the API.</returns>
    public async Task<HttpStatusCode> DeletePostById(int id)
    {
        var response = await _client.GetAsync($"posts/{id}");

        if (response.IsSuccessStatusCode)
        {
            var deleteResponse = await _client.DeleteAsync($"posts/{id}");
            return deleteResponse.StatusCode;
        }
        else
        {
            //TODO: Handle error response appropriately, e.g., logging or throwing an exception
        }
        return response.StatusCode;
    }
}