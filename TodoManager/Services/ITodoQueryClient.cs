using TodoManager.Contracts;


namespace TodoManager.Services;

public interface ITodoQueryClient
{
    Task<TodoItemDto?> GetTodoAsync(Guid id);
}