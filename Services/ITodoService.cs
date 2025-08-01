using todo_list.Models;

namespace todo_list.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(string? sortBy);
        Task<TodoItem?> GetByIdAsync(int id);
        Task CreateAsync(TodoItem todoItem);
        Task UpdateAsync(TodoItem todoItem);
        Task DeleteAsync(int id);
        Task ToggleIsCompletedAsync(int id);
    }
}