using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

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

    public async Task AddAsync(TodoItem todo)
    {
        await _context.Todos.AddAsync(todo);

        await _context.SaveChangesAsync();
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
        return await _context.Todos
            .Where(t => t.Title.Contains(keyword))
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetByCategoryAsync(string category)
    {
        return await _context.Todos
            .Where(t => t.Category.ToString() == category)
            .ToListAsync();
    }
}