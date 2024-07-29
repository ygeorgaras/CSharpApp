using System.Net;

namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClientWrapper _httpClientWrapper;

    private const string TODO_ENDPOINT = "/todos";

    public TodoService(ILogger<TodoService> logger, 
        HttpClientWrapper httpClientWrapper)
    {
        _logger = logger;
        _httpClientWrapper = httpClientWrapper;
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        try
        {
            return await _httpClientWrapper.GetAsync<TodoRecord>($"{TODO_ENDPOINT}/{id}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to get Todo by ID: {id}.");
            return null;
        }
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        try
        {
            return await _httpClientWrapper.GetAllAsync<TodoRecord>($"{TODO_ENDPOINT}/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get list of all Todo records.");
            return new ReadOnlyCollection<TodoRecord>(new List<TodoRecord>());
        }
    }

    /// <summary>
    /// Checks if the record is valid and then trys posting the record from the HttpClientWrapper.
    /// </summary>
    /// <param name="userId">New userId we want to assign.</param>
    /// <param name="title">New title to use.</param>
    /// <param name="body">New body to use.</param>
    /// <returns>Returns the new record along with the new ID.</returns>
    public async Task<TodoRecord?> AddTodoRecord(TodoRecord newRecord)
    {
        try
        {
            return await _httpClientWrapper.PostAsync<TodoRecord, TodoRecord>($"{TODO_ENDPOINT}", newRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create new Todo.");
            return null;
        }
    }

    /// <summary>
    /// This will search using the input ID if the record exists. If it exists then it will issue a Delete to the API.
    /// </summary>
    /// <param name="id">ID we wish to delete.</param>
    /// <returns>The HTTP Status Code we receive from the API.</returns>
    public async Task<HttpStatusCode> DeleteTodoById(int id)
    {
        return await _httpClientWrapper.DeleteByIdAsync($"{TODO_ENDPOINT}", id);
    }

    /// <summary>
    /// This will overwrite an existing record to the todo endpoint.
    /// </summary>
    /// <param name="newRecord">New record we want to use to overwrite the old one.</param>
    /// <returns>Returns the new record entered.</returns>
    public async Task<TodoRecord?> PutTodoById(TodoRecord newRecord)
    {
        try
        {
            var response = await _httpClientWrapper.PutAsync<TodoRecord, TodoRecord>(TODO_ENDPOINT, newRecord, newRecord.Id);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to put new post");
            return null;
        }
    }
}