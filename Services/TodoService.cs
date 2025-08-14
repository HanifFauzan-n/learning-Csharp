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
                
            }

            
            query = sortBy switch
            {
                "title" => query.OrderBy(s => s.Title),
                "date" => query.OrderBy(s => s.StartDate),
                "desc_date" => query.OrderByDescending(s => s.StartDate),
                "desc_title" => query.OrderByDescending(s => s.Title),
                "priority" => query.OrderBy(s => s.Priority),
                "desc_priority" => query.OrderByDescending(s => s.Priority),
                _ => query.OrderBy(s => s.Title),
            };
            // Console.WriteLine($"DEBUG Service Menerima: '{sortBy ?? "null"}'");
            Console.WriteLine($"DEBUG Service Menerima: '{searchString ?? "null"}'");
            return await query.ToListAsync();
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