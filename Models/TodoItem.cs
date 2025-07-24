using System.ComponentModel.DataAnnotations;

namespace todo_list.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}