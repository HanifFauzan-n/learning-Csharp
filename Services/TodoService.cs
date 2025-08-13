using Microsoft.EntityFrameworkCore;
using todo_list.Data;
using todo_list.Models;
using System.Globalization;

namespace todo_list.Services
{
    public class TodoService : ITodoService
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        private readonly ApplicationDbContext _context;

        public TodoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TodoItem todoItem)
        {
            todoItem.Title = textInfo.ToTitleCase(todoItem.Title);
            _context.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem != null)
            {
                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(string? sortBy, string? searchString)
        {
            var query = _context.TodoItems.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Title!.Contains(searchString));
                return await query.ToListAsync();
            }

            var items = _context.TodoItems.AsQueryable();
            items = sortBy switch
            {
                "title" => items.OrderBy(s => s.Title),
                "date" => items.OrderBy(s => s.StartDate),
                "desc_date" => items.OrderByDescending(s => s.StartDate),
                "desc_title" => items.OrderByDescending(s => s.Title),
                "priority" => items.OrderBy(s => s.Priority),
                "desc_priority" => items.OrderByDescending(s => s.Priority),
                _ => items.OrderBy(s => s.Title),
            };
            // Console.WriteLine($"DEBUG Service Menerima: '{sortBy ?? "null"}'");
            Console.WriteLine($"DEBUG Service Menerima: '{searchString ?? "null"}'");
            return await items.ToListAsync();
        }


        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateAsync(TodoItem todoItem)
        {
            todoItem.Title = textInfo.ToTitleCase(todoItem.Title);
            _context.Update(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task ToggleIsCompletedAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem != null)
            {
                todoItem.IsCompleted = !todoItem.IsCompleted;
                await _context.SaveChangesAsync();
            }
        }

    }
}