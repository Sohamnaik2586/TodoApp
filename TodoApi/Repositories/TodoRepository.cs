using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Models.Enums;

namespace TodoApi.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(int id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task<TodoItem> AddAsync(TodoItem todo)
    {
        await _context.Todos.AddAsync(todo);

        await _context.SaveChangesAsync();

        return todo;
    }

    public async Task UpdateAsync(TodoItem todo)
    {
        _context.Todos.Update(todo);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem todo)
    {
        _context.Todos.Remove(todo);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TodoItem>> SearchAsync(string keyword)
    {
        keyword = keyword.ToLower();

        return await _context.Todos
            .Where(t =>
                t.Title.ToLower().Contains(keyword) ||
                t.Description.ToLower().Contains(keyword))
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetByCategoryAsync(CategoryType category)
    {
        return await _context.Todos
            .Where(t => t.Category == category)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetByPriorityAsync(PriorityLevel priority)
    {
        return await _context.Todos
            .Where(t => t.Priority == priority)
            .ToListAsync();
    }
}
