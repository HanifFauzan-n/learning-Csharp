using Microsoft.EntityFrameworkCore;
using todo_list.Models;

namespace todo_list.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}