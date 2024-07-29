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
}