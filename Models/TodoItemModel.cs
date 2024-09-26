namespace TodoApi.Models;
using System.ComponentModel.DataAnnotations;

public class TodoItemModel
{
    public TodoItemModel(string name, bool isComplete) {
        Name = name;
        IsComplete = isComplete;
    }

    [Required]
    public string? Name { get; set; }
    [Required]
    public bool IsComplete { get; set; }
}