using TodoApi.DTOs.TodoDTOs;

namespace TodoApi.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync();

    Task<TodoResponseDto?> GetTodoByIdAsync(int id);

    Task CreateTodoAsync(CreateTodoDto dto);

    Task<bool> UpdateTodoAsync(int id, UpdateTodoDto dto);

    Task<bool> DeleteTodoAsync(int id);

    Task<IEnumerable<TodoResponseDto>> SearchTodosAsync(string keyword);

    Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(string category);
}