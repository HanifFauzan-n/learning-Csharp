using System.ComponentModel.DataAnnotations;
using todo_list.Enums;

namespace todo_list.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        public bool IsCompleted { get; set; }

        [Display(Name = "Waktu Mulai")]
        [DataType(DataType.Time)]
        public TimeOnly StartDate { get; set; }

        [Display(Name = "Waktu Berakhir")]
        [DataType(DataType.Time)]
        public TimeOnly EndDate { get; set; } 
        
        public PriorityLevels Priority { get; set; }

    }
}