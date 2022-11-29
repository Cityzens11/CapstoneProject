using System.ComponentModel.DataAnnotations;
using CapstoneProject.Enum;

namespace CapstoneProject.DomainModels
{
    /// <summary>
    /// A base class for ToDoEntry model.
    /// </summary>
    public class ToDoEntry
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        [StringLength(100, MinimumLength = 5)]
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public ToDoEntryStatus Status { get; set; }

        public int ToDoListId { get; set; }
        public ToDoList? ToDoList { get; set; }
    }
}
