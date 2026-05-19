using TodoApi.Models;

namespace TodoApi.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();

    Task<TodoItem?> GetByIdAsync(int id);

    Task AddAsync(TodoItem todo);

    Task UpdateAsync(TodoItem todo);

    Task DeleteAsync(TodoItem todo);

    Task<IEnumerable<TodoItem>> SearchAsync(string keyword);

    Task<IEnumerable<TodoItem>> GetByCategoryAsync(string category);
}