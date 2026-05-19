using AutoMapper;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync()
    {
        var todos = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }

    public async Task<TodoResponseDto?> GetTodoByIdAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);

        if (todo == null)
        {
            return null;
        }

        return _mapper.Map<TodoResponseDto>(todo);
    }

    public async Task CreateTodoAsync(CreateTodoDto dto)
    {
        var todo = _mapper.Map<TodoItem>(dto);

        await _repository.AddAsync(todo);
    }

    public async Task<bool> UpdateTodoAsync(int id, UpdateTodoDto dto)
    {
        var existingTodo = await _repository.GetByIdAsync(id);

        if (existingTodo == null)
        {
            return false;
        }

        _mapper.Map(dto, existingTodo);

        await _repository.UpdateAsync(existingTodo);

        return true;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await _repository.GetByIdAsync(id);

        if (todo == null)
        {
            return false;
        }

        await _repository.DeleteAsync(todo);

        return true;
    }

    public async Task<IEnumerable<TodoResponseDto>> SearchTodosAsync(string keyword)
    {
        var todos = await _repository.SearchAsync(keyword);

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(string category)
    {
        var todos = await _repository.GetByCategoryAsync(category);

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }
}