using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs.TodoDTOs;
using TodoApi.Interfaces;
using TodoApi.Exceptions;
using TodoApi.Models.Enums;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private const int MaxSearchKeywordLength = 100;

    private readonly ITodoService _service;

    public TodoController(ITodoService service)
    {
        _service = service;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<IActionResult> GetAllTodos()
    {
        var todos = await _service.GetAllTodosAsync();

        return Ok(todos);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoById(int id)
    {
        var todo = await _service.GetTodoByIdAsync(id);

        if (todo == null)
        {
            throw new NotFoundException($"Todo with id {id} not found");
        }

        return Ok(todo);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("priority/{priority}")]
    public async Task<IActionResult> GetTodosByPriority(string priority)
    {
        if (!TryParseDefinedEnum(priority, out PriorityLevel parsedPriority))
        {
            return BadRequest(new
            {
                error = "Invalid priority. Valid values are: Low, Medium, High."
            });
        }

        var todos = await _service.GetTodosByPriorityAsync(parsedPriority);

        return Ok(todos);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateTodo(CreateTodoDto dto)
    {
        var createdTodo = await _service.CreateTodoAsync(dto);

        return CreatedAtAction(
            nameof(GetTodoById),
            new { id = createdTodo.Id },
            createdTodo);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto dto)
    {
        var updated = await _service.UpdateTodoAsync(id, dto);

        if (!updated)
        {
            throw new NotFoundException($"Todo with id {id} not found");
        }

        return Ok(new
        {
            message = "Todo updated successfully"
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var deleted = await _service.DeleteTodoAsync(id);

        if (!deleted)
        {
            throw new NotFoundException($"Todo with id {id} not found");
        }

        return Ok(new
        {
            message = "Todo deleted successfully"
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("search")]
    public async Task<IActionResult> SearchTodos([FromQuery] string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return BadRequest(new
            {
                error = "Search keyword is required."
            });
        }

        var trimmedKeyword = keyword.Trim();

        if (trimmedKeyword.Length > MaxSearchKeywordLength)
        {
            return BadRequest(new
            {
                error = $"Search keyword cannot exceed {MaxSearchKeywordLength} characters."
            });
        }

        var todos = await _service.SearchTodosAsync(trimmedKeyword);

        return Ok(todos);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetTodosByCategory(string category)
    {
        if (!TryParseDefinedEnum(category, out CategoryType parsedCategory))
        {
            return BadRequest(new
            {
                error = "Invalid category. Valid values are: Work, Personal, Shopping, Health, Finance, Education, Entertainment, Travel, Other."
            });
        }

        var todos = await _service.GetTodosByCategoryAsync(parsedCategory);

        return Ok(todos);
    }

    private static bool TryParseDefinedEnum<TEnum>(string? value, out TEnum parsedValue)
        where TEnum : struct, Enum
    {
        parsedValue = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmedValue = value.Trim();

        if (!Enum.GetNames<TEnum>().Contains(trimmedValue, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        return Enum.TryParse(trimmedValue, ignoreCase: true, out parsedValue);
    }
}
