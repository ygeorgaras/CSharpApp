namespace CSharpApp.Core.Interfaces;

/// <summary>
/// Holds the interface implentation used by the TodoService.
/// </summary>
public interface ITodoService
{
    Task<TodoRecord?> GetTodoById(int id);
    Task<ReadOnlyCollection<TodoRecord>> GetAllTodos();
}