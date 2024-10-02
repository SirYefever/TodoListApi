namespace TodoApi.Models;
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public TodoItem(long id, string name, bool isComplete) {
        Id = id;
        Name = name;
        IsComplete = isComplete;
    }

    [Key]
    public long Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public bool IsComplete { get; set; }
    [Required]
    public DateTime TimeAdded { get; set; }
}