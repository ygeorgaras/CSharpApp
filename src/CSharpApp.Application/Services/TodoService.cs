namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly ILogger<TodoService> _logger;
    private readonly HttpClient _client;

    private readonly string? _baseUrl;

    public TodoService(ILogger<TodoService> logger, 
        IConfiguration configuration)
    {
        _logger = logger;
        _client = new HttpClient();
        _baseUrl = configuration["BaseUrl"];
        _client.BaseAddress = new Uri(_baseUrl!);
    }

    /// <summary>
    /// This method will try to retrieve a Todo record if it exists.
    /// </summary>
    /// <param name="id">ID we are searching for.</param>
    /// <returns>Existing TodoRecord or null if record doesn't exist.</returns>
    public async Task<TodoRecord?> GetTodoById(int id)
    {
        var response = await _client.GetAsync($"todos/{id}");

        if (response.IsSuccessStatusCode)
        {
            var todoRecord = await response.Content.ReadFromJsonAsync<TodoRecord>();

            return todoRecord;
        }

        return null;
    }

    /// <summary>
    /// This method will get a list of all Todo records existing.
    /// </summary>
    /// <returns>All Todo records. Or a null list if it cannot find any records.</returns>
    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        var response = await _client.GetAsync($"todos");
        if (response.IsSuccessStatusCode)
        {
            var todoRecord = await response.Content.ReadFromJsonAsync<List<TodoRecord>>();

            return new ReadOnlyCollection<TodoRecord>(todoRecord!);
        }
        return new ReadOnlyCollection<TodoRecord>(new List<TodoRecord>());
    }
}