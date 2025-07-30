using Microsoft.EntityFrameworkCore;
using todo_list.Data;
using todo_list.Models;

namespace todo_list.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;

        public TodoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TodoItem todoItem)
        {
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

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }


        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateAsync(TodoItem todoItem)
        {
            _context.Update(todoItem);
            await _context.SaveChangesAsync();
        }

    }
}