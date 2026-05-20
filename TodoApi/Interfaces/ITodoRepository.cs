using TodoApi.Models;

using TodoApi.Models.Enums;

namespace TodoApi.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<TodoItem>> GetAllAsync();

    Task<TodoItem?> GetByIdAsync(int id);

    Task<TodoItem> AddAsync(TodoItem todo);

    Task UpdateAsync(TodoItem todo);

    Task DeleteAsync(TodoItem todo);

    Task<IEnumerable<TodoItem>> SearchAsync(string keyword);

    Task<IEnumerable<TodoItem>> GetByCategoryAsync(CategoryType category);

    Task<IEnumerable<TodoItem>> GetByPriorityAsync(PriorityLevel priority);
}
