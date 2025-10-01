using Microsoft.EntityFrameworkCore;
using todo_list.Data;
using todo_list.Models;
using System.Globalization;
using System.Security.Claims;

namespace todo_list.Services
{
    public class TodoService : ITodoService
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public TodoService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
        }

        private string GetCurrentUserId()
        {
            var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? throw new Exception("User not authentication");
        }

        public async Task CreateAsync(TodoItem todoItem)
        {
            todoItem.UserId = GetCurrentUserId();
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
            var currentUserId = GetCurrentUserId();
            var query = _context.TodoItems
                                .Where(t => t.UserId == currentUserId).
                                AsQueryable();

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
            var currentUserId = GetCurrentUserId();

            return await _context.TodoItems
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == currentUserId);
        }

        public async Task UpdateAsync(TodoItem todoItem)
        {
            var currentUserId = GetCurrentUserId();
            var originalTodoItem = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == todoItem.Id && t.UserId == currentUserId);

            if (originalTodoItem == null)
            {
                return ;
            }


            originalTodoItem.Title = textInfo.ToTitleCase(todoItem.Title);
            originalTodoItem.IsCompleted = todoItem.IsCompleted;
            originalTodoItem.Priority = todoItem.Priority;
            originalTodoItem.StartDate = todoItem.StartDate;
            originalTodoItem.EndDate = todoItem.EndDate;
            
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