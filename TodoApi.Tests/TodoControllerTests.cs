using Microsoft.AspNetCore.Mvc;
using TodoApi.Controllers;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Exceptions;
using TodoApi.Interfaces;
using TodoApi.Models.Enums;

namespace TodoApi.Tests;

public class TodoControllerTests
{
    [Fact]
    public async Task GetAllTodos_ReturnsOkWithTodos()
    {
        var todos = new[]
        {
            CreateResponseDto(id: 1, title: "Review backend")
        };
        var controller = new TodoController(new StubTodoService { Todos = todos });

        var result = await controller.GetAllTodos();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(todos, okResult.Value);
    }

    [Fact]
    public async Task GetTodoById_WhenTodoDoesNotExist_ThrowsNotFoundException()
    {
        var controller = new TodoController(new StubTodoService { TodoById = null });

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => controller.GetTodoById(99));

        Assert.Equal("Todo with id 99 not found", exception.Message);
    }

    [Fact]
    public async Task CreateTodo_ReturnsCreatedAtActionWithCreatedTodo()
    {
        var createdTodo = CreateResponseDto(id: 7, title: "Ship assignment");
        var controller = new TodoController(new StubTodoService { CreatedTodo = createdTodo });

        var result = await controller.CreateTodo(new CreateTodoDto
        {
            Title = "Ship assignment",
            Priority = PriorityLevel.High,
            Category = CategoryType.Work
        });

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(TodoController.GetTodoById), createdResult.ActionName);
        Assert.Equal(7, createdResult.RouteValues?["id"]);
        Assert.Same(createdTodo, createdResult.Value);
    }

    [Fact]
    public async Task UpdateTodo_WhenServiceUpdates_ReturnsSuccessMessage()
    {
        var controller = new TodoController(new StubTodoService { UpdateResult = true });

        var result = await controller.UpdateTodo(1, new UpdateTodoDto
        {
            Title = "Updated todo",
            Priority = PriorityLevel.Medium,
            Category = CategoryType.Work,
            IsCompleted = true
        });

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("updated", okResult.Value?.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task DeleteTodo_WhenServiceCannotDelete_ThrowsNotFoundException()
    {
        var controller = new TodoController(new StubTodoService { DeleteResult = false });

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => controller.DeleteTodo(42));

        Assert.Equal("Todo with id 42 not found", exception.Message);
    }

    [Fact]
    public async Task SearchTodos_WhenKeywordIsMissing_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());

        var result = await controller.SearchTodos(null);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Search keyword is required.", GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task SearchTodos_WhenKeywordIsTooLong_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());
        var keyword = new string('a', 101);

        var result = await controller.SearchTodos(keyword);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Search keyword cannot exceed 100 characters.", GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task SearchTodos_WhenKeywordIsValid_PassesTrimmedKeywordToService()
    {
        var service = new StubTodoService();
        var controller = new TodoController(service);

        var result = await controller.SearchTodos("  tests  ");

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal("tests", service.LastSearchKeyword);
    }

    [Fact]
    public async Task GetTodosByPriority_WhenPriorityIsInvalid_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());

        var result = await controller.GetTodosByPriority("urgent");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid priority. Valid values are: Low, Medium, High.", GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task GetTodosByPriority_WhenPriorityIsNumeric_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());

        var result = await controller.GetTodosByPriority("1");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid priority. Valid values are: Low, Medium, High.", GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task GetTodosByPriority_WhenPriorityIsValid_PassesParsedPriorityToService()
    {
        var service = new StubTodoService();
        var controller = new TodoController(service);

        var result = await controller.GetTodosByPriority("high");

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(PriorityLevel.High, service.LastPriority);
    }

    [Fact]
    public async Task GetTodosByCategory_WhenCategoryIsInvalid_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());

        var result = await controller.GetTodosByCategory("random");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(
            "Invalid category. Valid values are: Work, Personal, Shopping, Health, Finance, Education, Entertainment, Travel, Other.",
            GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task GetTodosByCategory_WhenCategoryIsNumeric_ReturnsBadRequest()
    {
        var controller = new TodoController(new StubTodoService());

        var result = await controller.GetTodosByCategory("1");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(
            "Invalid category. Valid values are: Work, Personal, Shopping, Health, Finance, Education, Entertainment, Travel, Other.",
            GetErrorMessage(badRequest));
    }

    [Fact]
    public async Task GetTodosByCategory_WhenCategoryIsValid_PassesParsedCategoryToService()
    {
        var service = new StubTodoService();
        var controller = new TodoController(service);

        var result = await controller.GetTodosByCategory("personal");

        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(CategoryType.Personal, service.LastCategory);
    }

    private static TodoResponseDto CreateResponseDto(int id, string title)
    {
        return new TodoResponseDto
        {
            Id = id,
            Title = title,
            Description = "Description",
            Priority = PriorityLevel.Medium,
            Category = CategoryType.Work,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    private static string? GetErrorMessage(BadRequestObjectResult result)
    {
        return result.Value?.GetType().GetProperty("error")?.GetValue(result.Value)?.ToString();
    }

    private sealed class StubTodoService : ITodoService
    {
        public IEnumerable<TodoResponseDto> Todos { get; set; } = [];

        public TodoResponseDto? TodoById { get; set; }

        public TodoResponseDto CreatedTodo { get; set; } = CreateResponseDto(1, "Created todo");

        public bool UpdateResult { get; set; } = true;

        public bool DeleteResult { get; set; } = true;

        public string? LastSearchKeyword { get; private set; }

        public CategoryType? LastCategory { get; private set; }

        public PriorityLevel? LastPriority { get; private set; }

        public Task<IEnumerable<TodoResponseDto>> GetAllTodosAsync()
        {
            return Task.FromResult(Todos);
        }

        public Task<TodoResponseDto?> GetTodoByIdAsync(int id)
        {
            return Task.FromResult(TodoById);
        }

        public Task<TodoResponseDto> CreateTodoAsync(CreateTodoDto dto)
        {
            return Task.FromResult(CreatedTodo);
        }

        public Task<bool> UpdateTodoAsync(int id, UpdateTodoDto dto)
        {
            return Task.FromResult(UpdateResult);
        }

        public Task<bool> DeleteTodoAsync(int id)
        {
            return Task.FromResult(DeleteResult);
        }

        public Task<IEnumerable<TodoResponseDto>> SearchTodosAsync(string keyword)
        {
            LastSearchKeyword = keyword;

            return Task.FromResult(Todos);
        }

        public Task<IEnumerable<TodoResponseDto>> GetTodosByCategoryAsync(CategoryType category)
        {
            LastCategory = category;

            return Task.FromResult(Todos);
        }

        public Task<IEnumerable<TodoResponseDto>> GetTodosByPriorityAsync(PriorityLevel priority)
        {
            LastPriority = priority;

            return Task.FromResult(Todos);
        }
    }
}
