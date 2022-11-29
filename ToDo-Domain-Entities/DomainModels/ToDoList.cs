using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.DomainModels
{
    public class ToDoList
    {
        public int Id { get; set; }
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<ToDoEntry> ToDoEntries { get; set; } = new List<ToDoEntry> { };
    }
}
