namespace TodoApi.Models;
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public TodoItem(string name, bool isComplete) {
        Id = 0;
        Name = name;
        IsComplete = isComplete;
    }

    [Key]
    public long Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public bool IsComplete { get; set; }
}