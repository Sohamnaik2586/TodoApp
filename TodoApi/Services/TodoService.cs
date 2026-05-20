using AutoMapper;
using Microsoft.Extensions.Logging;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Models.Enums;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<TodoService> _logger;

    public TodoService(
        ITodoRepository repository,
        IMapper mapper,
        ILogger<TodoService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync()
    {
        _logger.LogInformation("Fetching all todos");

        var todos = await _repository.GetAllAsync();

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }

    public async Task<TodoResponseDto?> GetTodoByIdAsync(int id)
    {
        _logger.LogInformation(
            "Fetching todo with id: {Id}",
            id);

        var todo = await _repository.GetByIdAsync(id);

        if (todo == null)
        {
            _logger.LogWarning(
                "Todo with id: {Id} not found",
                id);

            return null;
        }

        return _mapper.Map<TodoResponseDto>(todo);
    }

    public async Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto dto)
    {
        _logger.LogInformation(
            "Creating todo with title: {Title}",
            dto.Title);

        var todo = _mapper.Map<TodoItem>(dto);

        var createdTodo = await _repository.AddAsync(todo);

        return _mapper.Map<TodoResponseDto>(createdTodo);
    }

    public async Task<bool> UpdateTodoAsync(int id, UpdateTodoDto dto)
    {
        _logger.LogInformation(
            "Updating todo with id: {Id}",
            id);

        var existingTodo = await _repository.GetByIdAsync(id);

        if (existingTodo == null)
        {
            _logger.LogWarning(
                "Todo with id: {Id} not found for update",
                id);

            return false;
        }

        _mapper.Map(dto, existingTodo);

        await _repository.UpdateAsync(existingTodo);

        return true;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        _logger.LogInformation(
            "Deleting todo with id: {Id}",
            id);

        var todo = await _repository.GetByIdAsync(id);

        if (todo == null)
        {
            _logger.LogWarning(
                "Todo with id: {Id} not found for deletion",
                id);

            return false;
        }

        await _repository.DeleteAsync(todo);

        return true;
    }

    public async Task<IEnumerable<TodoResponseDto>> SearchTodosAsync(string keyword)
    {
        _logger.LogInformation(
            "Searching todos with keyword: {Keyword}",
            keyword);

        var todos = await _repository.SearchAsync(keyword);

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(CategoryType category)
    {
        _logger.LogInformation(
            "Fetching todos by category: {Category}",
            category);

        var todos = await _repository.GetByCategoryAsync(category);

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }

    public async Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(PriorityLevel priority)
    {
        _logger.LogInformation(
            "Fetching todos by priority: {Priority}",
            priority);

        var todos = await _repository.GetByPriorityAsync(priority);

        return _mapper.Map<IEnumerable<TodoResponseDto>>(todos);
    }
}
