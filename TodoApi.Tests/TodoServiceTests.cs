using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Interfaces;
using TodoApi.Mappings;
using TodoApi.Models;
using TodoApi.Models.Enums;
using TodoApi.Services;

namespace TodoApi.Tests;

public class TodoServiceTests
{
    [Fact]
    public async Task GetAllTodosAsync_MapsEntitiesToResponseDtos()
    {
        var repository = new FakeTodoRepository
        {
            Todos =
            [
                CreateTodoItem(id: 1, title: "Learn Web API"),
                CreateTodoItem(id: 2, title: "Add tests")
            ]
        };
        var service = CreateService(repository);

        var result = await service.GetAllTodosAsync();

        var todos = result.ToList();
        Assert.Equal(2, todos.Count);
        Assert.Equal("Learn Web API", todos[0].Title);
        Assert.Equal(PriorityLevel.High, todos[0].Priority);
        Assert.Equal(CategoryType.Education, todos[0].Category);
    }

    [Fact]
    public async Task GetTodoByIdAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var service = CreateService(new FakeTodoRepository { TodoById = null });

        var result = await service.GetTodoByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTodoAsync_MapsDtoAndReturnsCreatedTodo()
    {
        var repository = new FakeTodoRepository();
        var service = CreateService(repository);
        var dto = new CreateTodoDto
        {
            Title = "Create from test",
            Description = "Mapped by AutoMapper",
            Priority = PriorityLevel.Medium,
            Category = CategoryType.Work
        };

        var result = await service.CreateTodoAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal(dto.Title, result.Title);
        Assert.Equal(dto.Priority, result.Priority);
        Assert.Equal(dto.Category, result.Category);
        Assert.NotNull(repository.AddedTodo);
    }

    [Fact]
    public async Task UpdateTodoAsync_WhenTodoExists_UpdatesExistingEntity()
    {
        var existingTodo = CreateTodoItem(id: 5, title: "Old title");
        var repository = new FakeTodoRepository { TodoById = existingTodo };
        var service = CreateService(repository);
        var dto = new UpdateTodoDto
        {
            Title = "New title",
            Description = "New description",
            Priority = PriorityLevel.Low,
            Category = CategoryType.Personal,
            IsCompleted = true
        };

        var result = await service.UpdateTodoAsync(5, dto);

        Assert.True(result);
        Assert.True(repository.WasUpdated);
        Assert.Equal("New title", existingTodo.Title);
        Assert.Equal(CategoryType.Personal, existingTodo.Category);
        Assert.True(existingTodo.IsCompleted);
    }

    [Fact]
    public async Task DeleteTodoAsync_WhenTodoDoesNotExist_ReturnsFalse()
    {
        var service = CreateService(new FakeTodoRepository { TodoById = null });

        var result = await service.DeleteTodoAsync(404);

        Assert.False(result);
    }

    private static TodoService CreateService(ITodoRepository repository)
    {
        return new TodoService(
            repository,
            CreateMapper(),
            NullLogger<TodoService>.Instance);
    }

    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<TodoMappingProfile>());

        return config.CreateMapper();
    }

    private static TodoItem CreateTodoItem(int id, string title)
    {
        return new TodoItem
        {
            Id = id,
            Title = title,
            Description = "Description",
            Priority = PriorityLevel.High,
            Category = CategoryType.Education,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    private sealed class FakeTodoRepository : ITodoRepository
    {
        public IEnumerable<TodoItem> Todos { get; set; } = [];

        public TodoItem? TodoById { get; set; }

        public TodoItem? AddedTodo { get; private set; }

        public bool WasUpdated { get; private set; }

        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return Task.FromResult(Todos);
        }

        public Task<TodoItem?> GetByIdAsync(int id)
        {
            return Task.FromResult(TodoById);
        }

        public Task<TodoItem> AddAsync(TodoItem todo)
        {
            todo.Id = 1;
            AddedTodo = todo;

            return Task.FromResult(todo);
        }

        public Task UpdateAsync(TodoItem todo)
        {
            WasUpdated = true;

            return Task.CompletedTask;
        }

        public Task DeleteAsync(TodoItem todo)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TodoItem>> SearchAsync(string keyword)
        {
            return Task.FromResult(Todos);
        }

        public Task<IEnumerable<TodoItem>> GetByCategoryAsync(CategoryType category)
        {
            return Task.FromResult(Todos);
        }

        public Task<IEnumerable<TodoItem>> GetByPriorityAsync(PriorityLevel priority)
        {
            return Task.FromResult(Todos);
        }
    }
}
